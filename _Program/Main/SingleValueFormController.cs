using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace Altaxo.Main
{
	#region Interfaces

	public interface ISingleValueFormView
	{
		/// <summary>
		/// Returns either the view itself if the view is a form, or the form where this view is contained into, if it is a control or so.
		/// </summary>
		System.Windows.Forms.Form Form { get; }
	
		string EditBoxContents { get; set; }

		/// <summary>
		/// Get / sets the controler of this view.
		/// </summary>
		ISingleValueFormController Controller { get; set; }
	}

	public interface ISingleValueFormController
	{
		void EhView_EditBoxValidating(System.ComponentModel.CancelEventArgs e);
	}

	#endregion

	#region Controller classes

	public class IntegerValueInputController : ISingleValueFormController
	{
		ISingleValueFormView m_View;

		int m_InitialContents;

		int m_EnteredContents;

		private IIntegerValidator m_Validator;

		public IntegerValueInputController(int initialcontents,ISingleValueFormView view)
		{
			m_InitialContents = initialcontents;
			View = view;
		}

		ISingleValueFormView View
		{
			get { return m_View; }
			set
			{
				m_View = value;
				m_View.Controller = this;
			}
		}

		public int EnteredContents
		{
			get { return m_EnteredContents; }
		}

		public IIntegerValidator Validator
		{
			set { m_Validator = value; }
		}

		public bool ShowDialog(System.Windows.Forms.Form owner)
		{
			return System.Windows.Forms.DialogResult.OK==m_View.Form.ShowDialog(owner);
		}

		#region ISingleValueFormController Members

		public void EhView_EditBoxValidating(CancelEventArgs e)
		{
			string err=null;
			try
			{
				m_EnteredContents = System.Convert.ToInt32(m_View.EditBoxContents);
				
				if(null!=this.m_Validator)
					err=m_Validator.Validate(m_EnteredContents);
				
				e.Cancel = null!=err;
			}
			catch(Exception )
			{
				err = "You must enter a integer value!";
				e.Cancel = true;
			}

			if(null!=err)
			{
				System.Windows.Forms.MessageBox.Show(this.View.Form,err,"Attention");
			}
		}
		#endregion


		/// <summary>
		/// Provides an interface to a validator to validates the user input
		/// </summary>
		public interface IIntegerValidator
		{
			/// <summary>
			/// Validates if the user input number i is valid user input.
			/// </summary>
			/// <param name="i">The number entered by the user.</param>
			/// <returns>Null if this input is valid, error message else.</returns>
			string Validate(int i);
		}

		public class ZeroOrPositiveIntegerValidator : IIntegerValidator
		{
			public string Validate(int i)
			{
				if(i<0)
					return "The provided number must be zero or positive!";
				else
					return null;			
			}
		}
	} // end of class IntegerValueInputController





	public class TextValueInputController : ISingleValueFormController
	{
		ISingleValueFormView m_View;
		string m_InitialContents;
		string m_Contents;

		private IStringValidator m_Validator;


		public TextValueInputController(string initialcontents,ISingleValueFormView view)
		{
			m_InitialContents = initialcontents;
			View = view;
		}


		ISingleValueFormView View
		{
			get { return m_View; }
			set
			{
				m_View = value;
				m_View.Controller = this;

				m_View.EditBoxContents = m_InitialContents;
			}
		}

		public string InputText
		{
			get { return m_Contents; }
		}

		public IStringValidator Validator
		{
			set { m_Validator = value; }
		}

		public bool ShowDialog(System.Windows.Forms.Form owner)
		{
			return System.Windows.Forms.DialogResult.OK==m_View.Form.ShowDialog(owner);
		}


		#region ISingleValueFormController Members

		public void EhView_EditBoxValidating(CancelEventArgs e)
		{
			m_Contents = m_View.EditBoxContents;
			if(m_Validator!=null)
			{
				string err = m_Validator.Validate(m_Contents);
				if(null!=err)
				{
					e.Cancel = true;
					System.Windows.Forms.MessageBox.Show(this.View.Form,err,"Attention");
				}
			}
			else // if no validating handler, use some default validation
			{
				if(null==m_Contents || 0==m_Contents.Length)
				{
					e.Cancel = true;
				}
			}
		}

		#endregion

		#region Validator classes

		/// <summary>
		/// Provides an interface to a validator to validates the user input
		/// </summary>
		public interface IStringValidator
		{
			/// <summary>
			/// Validates if the user input in txt is valid user input.
			/// </summary>
			/// <param name="txt">The text entered by the user.</param>
			/// <returns>Null if this input is valid, error message else.</returns>
			string Validate(string txt);
		}

		/// <summary>
		/// Provides a validator that tests if the string entered is empty.
		/// </summary>
		public class NonEmptyStringValidator : IStringValidator
		{
			protected string m_EmptyMessage = "You have not entered text. Please enter text!";


			public NonEmptyStringValidator()
			{
			}

			public NonEmptyStringValidator(string errmsg)
			{
				m_EmptyMessage = errmsg;
			}

			public virtual string Validate(string txt)
			{
				if(txt==null || txt.Trim().Length==0)
					return m_EmptyMessage;
				else
					return null;
			}
		}

		#endregion
	} // end of class TextValueInputController
		
	#endregion
}