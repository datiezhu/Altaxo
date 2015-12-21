﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2015 Dr. Dirk Lellinger
//
//    This program is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 2 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program; if not, write to the Free Software
//    Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
/////////////////////////////////////////////////////////////////////////////

#endregion Copyright

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altaxo.Gui
{
	/// <summary>
	/// Base of all controllers that edit the original document directly (live).
	/// </summary>
	/// <typeparam name="TModel">The type of the document to edit.</typeparam>
	/// <typeparam name="TView">The type of the view.</typeparam>
	public abstract class MVCANControllerEditOriginalDocBase<TModel, TView> : IMVCANController
		where TView : class
		where TModel : ICloneable
	{
		/// <summary>The document to edit.</summary>
		protected TModel _doc;

		/// <summary>Cloned copy of the document (is null if  <see cref="_useDocumentCopy"/> is false). Used to revert the edited document to the state before editing.</summary>
		protected TModel _clonedCopyOfDoc;

		/// <summary>The Gui view of this controller</summary>
		protected TView _view;

		/// <summary>If true, a copy of the document is made before editing; this copy can later be used to revert the state of the document to the original state.</summary>
		protected bool _useDocumentCopy;

		/// <summary>Set to true if this controller is already disposed.</summary>
		private bool _isDisposed;

		/// <summary>
		/// The suspend token of the document being edited. If <see cref="_useDocumentCopy"/> is false, we assume that a controller higher in hierarchy has made a copy
		/// of the document, thus we do not use a suspendToken for the document.
		/// </summary>
		protected Altaxo.Main.ISuspendToken _suspendToken;

		/// <summary>
		/// Initialize the controller with the document. If successfull, the function has to return true.
		/// </summary>
		/// <param name="args">The arguments neccessary to create the controller. Normally, the first argument is the document, the second can be the parent of the document and so on.</param>
		/// <returns>
		/// Returns <see langword="true" /> if successfull; otherwise <see langword="false" />.
		/// </returns>
		/// <exception cref="System.ObjectDisposedException"></exception>
		public virtual bool InitializeDocument(params object[] args)
		{
			if (IsDisposed)
				throw new ObjectDisposedException(this.GetType().FullName);

			if (null == args || 0 == args.Length || !(args[0] is TModel))
				return false;

			_doc = (TModel)args[0];

			if (_useDocumentCopy && _doc is ICloneable)
				_clonedCopyOfDoc = (TModel)((ICloneable)_doc).Clone();

			Initialize(true);
			return true;
		}

		/// <summary>
		/// Basic initialization of the document.
		/// Here, it is tried to suspend the event handling of the documnt by calling <see cref="GetSuspendTokenForControllerDocument"/> (but only if <see cref="_useDocumentCopy"/> is <c>true</c>).
		/// </summary>
		/// <param name="initData">If set to <c>true</c>, it indicates that the controller should initialize its model classes..</param>
		/// <exception cref="System.InvalidOperationException">This controller was not initialized with a document.</exception>
		/// <exception cref="System.ObjectDisposedException">The controller was already disposed.</exception>
		protected virtual void Initialize(bool initData)
		{
			if (null == _doc)
				throw new InvalidOperationException("This controller was not initialized with a document.");
			if (IsDisposed)
				throw new ObjectDisposedException("The controller was already disposed. Type: " + this.GetType().FullName);

			if (initData)
			{
				if (_useDocumentCopy && null == _suspendToken)
					_suspendToken = GetSuspendTokenForControllerDocument();
			}
		}

		/// <summary>
		/// Override this function to attach the view to the controller, either by subscribing to events of the view, or by setting the controller object on the view.
		/// </summary>
		protected virtual void AttachView()
		{
		}

		/// <summary>
		/// Override this function to detach the view from the controller, either by unsubscribing to events of the view, or by setting the controller object on the view to null.
		/// </summary>
		protected virtual void DetachView()
		{
		}

		/// <summary>
		/// Called when the user input has to be applied to the document being controlled. Returns true if Apply is successfull.
		/// </summary>
		/// <param name="disposeController">If the Apply operation was successfull, and this argument is <c>true</c>, the controller should release all temporary resources, because they are not needed any more.
		/// If this argument is <c>false</c>, the controller should be reinitialized with the current model (the model that results from the Apply operation).</param>
		/// <returns>
		/// True if the apply operation was successfull, otherwise false. If false is returned, the <paramref name="disposeController" /> argument is ignored: thus the controller is not disposed.
		/// </returns>
		/// <remarks>
		/// This function is called in two cases: Either the user pressed OK or the user pressed Apply.
		/// </remarks>
		public abstract bool Apply(bool disposeController);

		/// <summary>
		/// Standard procedure at the end of the Apply phase. If the <paramref name="applyResult"/> is <c>true</c>, the controller is either disposed (if <c>disposeController</c> is <c>true</c>) or
		/// the document is shortly resumed (if <c>disposeController</c> is <c>false</c>. Nothing is done if <c>applyResult</c> is <c>false</c>.
		/// </summary>
		/// <param name="applyResult">If set to <c>true</c>, the apply operation was successful.</param>
		/// <param name="disposeController">If set to <c>true</c>, the controller is no longer needed and should be disposed.</param>
		/// <returns>The same value as the parameter <c>applyResult</c>. (for the convenience that you can use this function in the return statement of Apply).</returns>
		protected bool ApplyEnd(bool applyResult, bool disposeController)
		{
			if (applyResult == true)
			{
				if (disposeController)
				{
					Dispose();
				}
				else
				{
					if (null != _suspendToken)
					{
						_suspendToken.ResumeCompleteTemporarily();
					}
				}
			}
			return applyResult;
		}

		/// <summary>
		/// Try to revert changes to the model, i.e. restores the original state of the model.
		/// </summary>
		/// <param name="disposeController">If set to <c>true</c>, the controller should release all temporary resources, since the controller is not needed anymore.</param>
		/// <returns>
		///   <c>True</c> if the revert operation was successfull; <c>false</c> if the revert operation was not possible (i.e. because the controller has not stored the original state of the model).
		/// </returns>
		public virtual bool Revert(bool disposeController)
		{
			foreach (var subControllerItem in GetSubControllers())
			{
				if (null != subControllerItem.Controller)
					subControllerItem.Controller.Revert(disposeController);
			}

			bool reverted = false;
			if (null != _clonedCopyOfDoc && !object.ReferenceEquals(_doc, _clonedCopyOfDoc))
			{
				CopyHelper.Copy(ref _doc, _clonedCopyOfDoc);
				reverted = true;
			}

			if (disposeController)
			{
				Dispose();
			}
			else // not disposing means we have to show the reverted data
			{
				var viewTmp = ViewObject; // store view
				ViewObject = null; // detach view temporarily
				Initialize(true); // initialize data
				ViewObject = viewTmp; // attach view again
			}

			return reverted;
		}

		/// <summary>
		/// Gets the suspend token for the controller document. This default implementation calls SuspendGetToken() on the document.
		/// By overriding this function you can suspend parent nodes in case it is neccessary to modify nodes at lower levels of the hierarchy.
		/// </summary>
		/// <returns>The suspend token, provided by the document.</returns>
		protected virtual Altaxo.Main.ISuspendToken GetSuspendTokenForControllerDocument()
		{
			if (_doc is Altaxo.Main.ISuspendableByToken)
				return ((Altaxo.Main.ISuspendableByToken)_doc).SuspendGetToken();
			else
				return null;
		}

		/// <summary>Get a value indication whether  this controller is already disposed.</summary>
		public bool IsDisposed { get { return _isDisposed; } }

		/// <summary>
		/// Sets whether or not a copy of the document is used. If set to true, a copy of the document is used, so if the controller is not applied,
		/// all changes can be reverted. If set to false, no copy must be made. The document is directly changed by the controller, and changes can not be reverted.
		/// Use the last option if a controller up in the hierarchie has already made a copy of the document.
		/// </summary>
		public UseDocument UseDocumentCopy
		{
			set { _useDocumentCopy = value == UseDocument.Copy; }
		}

		/// <summary>
		/// Returns the Gui element that shows the model to the user.
		/// </summary>
		public virtual object ViewObject
		{
			get
			{
				return _view;
			}
			set
			{
				if (null != _view)
				{
					DetachView();
				}

				_view = value as TView;

				if (null != _view)
				{
					Initialize(false);
					AttachView();
				}
			}
		}

		/// <summary>
		/// Returns the document that this controller manages to edit.
		/// </summary>
		public virtual object ModelObject
		{
			get { return _doc; }
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Enumerates the sub controllers. This function is called on <see cref="Dispose(bool)"/> of this controller to dispose the subcontrollers too.
		/// By overriding this function, there is no need to override <see cref="Dispose(bool)"/>
		/// </summary>
		/// <returns></returns>
		public abstract IEnumerable<ControllerAndSetNullMethod> GetSubControllers();

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		public virtual void Dispose(bool isDisposing)
		{
			if (!IsDisposed)
			{
				foreach (var subControllerItem in this.GetSubControllers())
				{
					if (null != subControllerItem.Controller)
						subControllerItem.Controller.Dispose();
					if (null != subControllerItem.SetMemberToNullAction)
						subControllerItem.SetMemberToNullAction();
				}

				ViewObject = null;

				if (null != _suspendToken)
				{
					_suspendToken.Dispose();
					_suspendToken = null;
				}

				if ((_clonedCopyOfDoc is IDisposable) && !object.ReferenceEquals(_doc, _clonedCopyOfDoc))
				{
					((IDisposable)_clonedCopyOfDoc).Dispose();
					_clonedCopyOfDoc = default(TModel);
				}

				_isDisposed = true;
			}
		}

		#region Helper functions

		/// <summary>
		/// Helper function to dispose a member and set it to null.
		/// </summary>
		/// <typeparam name="T">Type of the object to dispose.</typeparam>
		/// <param name="objectToDispose">The object to dispose.</param>
		protected static void DisposeAndSetToNull<T>(ref T objectToDispose) where T : class, IDisposable
		{
			if (null != objectToDispose)
			{
				objectToDispose.Dispose();
				objectToDispose = null;
			}
		}

		#endregion Helper functions
	}
}