﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2014 Dr. Dirk Lellinger
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

namespace Altaxo.Main
{

  /// <summary>
  /// DocNodeProxy holds a reference to an object. If the object is a document node (implements <see cref="IDocumentLeafNode" />), then special
  /// measures are used in the case the document node is disposed. In this case the path to the node is stored, and if a new document node with
  /// that path exists, the reference to the object is restored.
  /// </summary>
  [Serializable]
  public class DocNodeProxy
    :
    Main.SuspendableDocumentLeafNodeWithEventArgs,
    IProxy,
    System.ICloneable
  {
    /// <summary>
    /// The full path to the document node.
    /// </summary>
    protected Main.AbsoluteDocumentPath _docNodePath;

    /// <summary>
    /// The (weak) reference to the document node.
    /// </summary>
    [NonSerialized]
    protected WeakReference _docNodeRef;

    /// <summary>
    /// Holds the (weak) event handler for changed events from the document node.
    /// </summary>
    [NonSerialized]
    protected WeakEventHandler _weakDocNodeChangedHandler;

    /// <summary>
    /// Holds the (weak) event handler for tunneling events from the document node.
    /// </summary>
    [NonSerialized]
    protected WeakActionHandler<object, object, TunnelingEventArgs> _weakDocNodeTunneledEventHandler;

#if DOCNODEPROXY_CONCURRENTDEBUG

		[NonSerialized]
		private System.Collections.Concurrent.ConcurrentQueue<string> _debug = new System.Collections.Concurrent.ConcurrentQueue<string>();

		private int _debugUSN;

#endif

    #region Serialization

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor("AltaxoBase", "Altaxo.Main.DocNodeProxy", 0)]
    private class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        throw new InvalidOperationException("Serialization of old version not supported");
        /*
				DocNodeProxy s = (DocNodeProxy)obj;

				if (null != s.InternalDocNode)
					info.AddValue("Node", Main.DocumentPath.GetAbsolutePath(s.InternalDocumentObject));
				else if (s._docNodePath != null)
					info.AddValue("Node", s._docNodePath);
				*/
      }

      public virtual object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        object node = info.GetValue("Node", null);

        if (node is AbsoluteDocumentPath)
        {
          var s = (DocNodeProxy)o ?? new DocNodeProxy((AbsoluteDocumentPath)node);
          s.InternalDocumentPath = (AbsoluteDocumentPath)node;
          info.DeserializationFinished += new Altaxo.Serialization.Xml.XmlDeserializationCallbackEventHandler(s.EhXmlDeserializationFinished);
          return s;
        }
        else
        {
          return node;
        }
      }
    }

    /// <summary>
    /// 2014-12-26 Only references are supported now
    /// </summary>
    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(DocNodeProxy), 1)]
    private class XmlSerializationSurrogate1 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        var s = (DocNodeProxy)obj;

        var node = s.InternalDocumentNode;

        if (null != node && !node.IsDisposeInProgress)
          s.InternalDocumentPath = Main.AbsoluteDocumentPath.GetAbsolutePath(node);

        if (!(null != s._docNodePath))
          throw new InvalidProgramException();

        info.AddValue("Path", s._docNodePath);
      }

      public virtual object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        var s = (DocNodeProxy)o ?? new DocNodeProxy(info);

        var nodePath = (Main.AbsoluteDocumentPath)info.GetValue("Path", s);

        s.InternalDocumentPath = nodePath;

        if (!(null != s._docNodePath))
          throw new InvalidProgramException();

        // create a callback to resolve the instance as early as possible
        info.DeserializationFinished += new Altaxo.Serialization.Xml.XmlDeserializationCallbackEventHandler(s.EhXmlDeserializationFinished);

        return s;
      }
    }

    #endregion Serialization

    public DocNodeProxy(IDocumentLeafNode docNode)
    {
      if (null == docNode)
        throw new ArgumentNullException(nameof(docNode));

      InternalSetDocNode(docNode);
    }

    /// <summary>
    /// For deserialization purposes only.
    /// </summary>
    /// <param name="info"></param>
    protected DocNodeProxy(Altaxo.Serialization.Xml.IXmlDeserializationInfo info)
    {
    }

    protected DocNodeProxy(Main.AbsoluteDocumentPath docNodePath)
    {
      InternalDocumentPath = docNodePath ?? throw new ArgumentNullException(nameof(docNodePath));
      InternalCheckAbsolutePath();
    }

    /// <summary>
    /// Cloning constructor.
    /// </summary>
    /// <param name="from">Object to clone from.</param>
    public DocNodeProxy(DocNodeProxy from)
    {
      if (null == from)
        throw new ArgumentNullException(nameof(from));
      if (from.IsDisposeInProgress)
        throw new ObjectDisposedException(nameof(from));

      if (null != from.InternalDocumentNode)
      {
        InternalSetDocNode(from.InternalDocumentNode); // than the new Proxy refers to the same document node
      }
      else
      {
        InternalDocumentPath = from._docNodePath.Clone(); // if no current document available, clone only the path
        InternalCheckAbsolutePath();
      }
    }

    /// <summary>
    /// Disposing this instance is special - we must not dispose the reference this instance holds.
    /// Instead, we remove all references to the holded document node and also all event handlers-
    /// </summary>
    /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool isDisposing)
    {
      if (null != _weakDocNodeChangedHandler)
      {
        _weakDocNodeChangedHandler.Remove();
        _weakDocNodeChangedHandler = null;
      }
      if (null != _weakDocNodeTunneledEventHandler)
      {
        _weakDocNodeTunneledEventHandler.Remove();
        _weakDocNodeTunneledEventHandler = null;
      }
      _docNodeRef = null;

#if DEBUG_DOCNODEPROXYLOGGING
			Current.Console.WriteLine("DocNodeProxy.Dispose, path was >>>{0}<<<", _docNodePath);
#endif

      _docNodePath = null;

      base.Dispose(isDisposing);
    }

    /// <summary>
    /// True when both the document and the stored document path are <c>null</c>.
    /// </summary>
    public bool IsEmpty
    {
      get
      {
        if (!(_docNodePath != null || IsDisposeInProgress))
          throw new InvalidProgramException();
        return false;
      }
    }

    /// <summary>
    /// Can be overriden by derived classes to ensure that the right type of document is stored in
    /// this proxy.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>True if the <c>obj</c> has the right type to store in this proxy, false otherwise.</returns>
    protected virtual bool IsValidDocument(object obj)
    {
      return obj is IDocumentLeafNode;
    }

    /// <summary>
    /// Can be overriden by derived classes to ensure that the right type of document is stored in
    /// this proxy.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>True if the <c>obj</c> has the right type to store in this proxy, false otherwise.</returns>
    protected virtual bool InternalIsValidDocument(object obj)
    {
      return IsValidDocument(obj);
    }

    /// <summary>
    /// Is called after a document has been set, but before OnChanged() is called. Can be used to set up
    /// additional things, like event handlers, in derived classes.
    /// </summary>
    protected virtual void OnAfterSetDocNode()
    {
    }

    /// <summary>
    /// Is called before the doc node of this proxy is set to null. Can be used in derived classes
    /// to remove additional event handlers.
    /// </summary>
    protected virtual void OnBeforeClearDocNode()
    {
    }

    [System.Diagnostics.Conditional("Debug_CheckDocNodePath")]
    protected void InternalCheckAbsolutePath()
    {
      var path = _docNodePath;

      if (path.Count == 0)
        throw new InvalidProgramException(string.Format("Path is expected to be non-empty. Path = {0}", path));

      var path0 = path[0];

      if (path0 != "Tables" && path0 != "Graphs" && path0 != "TableLayouts" && path0 != "FitFunctionScripts" && path0 != "FolderProperties")
        throw new InvalidProgramException(string.Format("Path is expected to start with Tables or Graphs. Path = {0}", path));
    }

    /// <summary>
    /// Sets the document node that is held by this proxy.
    /// </summary>
    /// <param name="value">The document node. If <c>docNode</c> implements <see cref="Main.IDocumentLeafNode" />,
    /// the document path is stored for this object in addition to the object itself.</param>
    public virtual void SetDocNode(IDocumentLeafNode value)
    {
      InternalSetDocNode(value);
    }

    /// <summary>
    /// Sets the document node that is held by this proxy.
    /// </summary>
    /// <param name="value">The document node. If <c>docNode</c> implements <see cref="Main.IDocumentLeafNode" />,
    /// the document path is stored for this object in addition to the object itself.</param>
    protected virtual void InternalSetDocNode(IDocumentLeafNode value)
    {
      if (null == value)
        throw new ArgumentNullException(nameof(value));

      var oldValue = InternalDocumentNode;
      if (object.ReferenceEquals(oldValue, value))
        return;

      if (!InternalIsValidDocument(value))
        throw new ArgumentException("This type of document is not allowed for the proxy of type " + GetType().ToString());

#if DOCNODEPROXY_CONCURRENTDEBUG
			_debug.Enqueue("START InternalSetDocNode");
#endif

      if (null != _weakDocNodeChangedHandler)
      {
        _weakDocNodeChangedHandler.Remove();
        _weakDocNodeChangedHandler = null;
      }
      if (null != _weakDocNodeTunneledEventHandler)
      {
        _weakDocNodeTunneledEventHandler.Remove();
        _weakDocNodeTunneledEventHandler = null;
      }

#if DOCNODEPROXY_CONCURRENTDEBUG
			_debug.Enqueue("MIDDL InternalSetDocNode EventHandlers removed");
#endif

      if (oldValue != null)
      {
        ClearDocNode();
      }

      InternalDocumentPath = Main.AbsoluteDocumentPath.GetAbsolutePath(value);
      _docNodeRef = new WeakReference(value);

#if DEBUG_DOCNODEPROXYLOGGING
			Current.Console.WriteLine("DocNodeProxy.InternalSetDocNode, path is <<{0}>>", _docNodePath);
#endif

      InternalCheckAbsolutePath();

      value.TunneledEvent += (_weakDocNodeTunneledEventHandler = new WeakActionHandler<object, object, TunnelingEventArgs>(EhDocNode_TunneledEvent, handler => value.TunneledEvent -= handler));
      value.Changed += (_weakDocNodeChangedHandler = new WeakEventHandler(EhDocNode_Changed, handler => value.Changed -= handler));

      OnAfterSetDocNode();

      EhSelfChanged(new Main.InstanceChangedEventArgs(oldValue, value));

#if DOCNODEPROXY_CONCURRENTDEBUG
			_debug.Enqueue("STOP  InternalSetDocNode");
#endif
    }

    /// <summary>
    /// Replaces parts of the part of the document node by another part. If the replacement was successful, the original document node is cleared.
    /// See <see cref="M:DocumentPath.ReplacePathParts"/> for details of the part replacement.
    /// </summary>
    /// <param name="partToReplace">Part of the path that should be replaced. This part has to match the beginning of this part. The last item of the part
    /// is allowed to be given only partially.</param>
    /// <param name="newPart">The new part to replace that piece of the path, that match the <c>partToReplace</c>.</param>
    /// <param name="rootNode">Any document node in the hierarchy that is used to find the root node of the hierarchy.</param>
    /// <returns>True if the path could be replaced. Returns false if the path does not fulfill the presumptions given above.</returns>
    /// <remarks>
    /// As stated above, the last item of the partToReplace can be given only partially. As an example, the path (here separated by space)
    /// <para>Tables Preexperiment1/WDaten Time</para>
    /// <para>should be replaced by </para>
    /// <para>Tables Preexperiment2\WDaten Time</para>
    /// <para>To make this replacement, the partToReplace should be given by</para>
    /// <para>Tables Preexperiment1/</para>
    /// <para>and the newPart should be given by</para>
    /// <para>Tables Preexperiment2\</para>
    /// <para>Note that Preexperiment1\ and Preexperiment2\ are only partially defined items of the path.</para>
    /// </remarks>
    public bool ReplacePathParts(AbsoluteDocumentPath partToReplace, AbsoluteDocumentPath newPart, IDocumentLeafNode rootNode)
    {
      if (!(_docNodePath != null || IsDisposeInProgress))
        throw new InvalidProgramException();
      if (null == rootNode)
        throw new ArgumentNullException(nameof(rootNode));

      var success = _docNodePath.ReplacePathParts(partToReplace, newPart, out var newPath);
      if (success)
      {
        _docNodePath = newPath;
        ClearDocNode();
        ResolveDocumentObject(rootNode);
      }

      return success;
    }

    /// <summary>
    /// Sets the document node to null, but keeps the doc node path.
    /// </summary>
    protected virtual void ClearDocNode()
    {
      if (_docNodeRef == null)
        return;

      OnBeforeClearDocNode();

      if (null != _weakDocNodeTunneledEventHandler)
      {
        _weakDocNodeTunneledEventHandler.Remove();
        _weakDocNodeTunneledEventHandler = null;
      }

      if (null != _weakDocNodeChangedHandler)
      {
        _weakDocNodeChangedHandler.Remove();
        _weakDocNodeChangedHandler = null;
      }
      _docNodeRef = null;
    }

    /// <summary>
    /// Removes the event handlers for the watch  (watch is the object that is the last object that currently exists along the document path).
    /// </summary>
    protected void ClearWatch()
    {
#if DEBUG_DOCNODEPROXYLOGGING
			Current.Console.WriteLine("DocNodeProxy.ClearWatch: path={0}", _docNodePath.ToString());
#endif
      if (null != _weakDocNodeTunneledEventHandler)
      {
        _weakDocNodeTunneledEventHandler.Remove();
        _weakDocNodeTunneledEventHandler = null;
      }

      if (null != _weakDocNodeChangedHandler)
      {
        _weakDocNodeChangedHandler.Remove();
        _weakDocNodeChangedHandler = null;
      }
    }

    /// <summary>
    /// Event handler that is called when the document node has disposed or name changed. Because the path to the node can have changed too,
    /// the path is renewed in this case.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="source">Source of the tunneled event.</param>
    /// <param name="e"></param>
    private void EhDocNode_TunneledEvent(object sender, object source, Main.TunnelingEventArgs e)
    {
      if (IsDisposeInProgress)
        return;

#if DEBUG_DOCNODEPROXYLOGGING
			Current.Console.WriteLine("DocNodeProxy.EhDocNode_TunneledEvent: sender={0}, source={1} e={2}", sender, source, e);
#endif

      bool shouldFireChangedEvent = false;

      var senderAsNode = source as IDocumentLeafNode;
      if (!(senderAsNode != null))
        throw new InvalidProgramException();

      if (e is DisposeEventArgs)
      {
        // when our DocNode was disposed, it is probable that the parent of this node (and further parents) are disposed too
        // thus we need to watch the first node that is not disposed
        var docNode = InternalDocumentNode;
        ClearDocNode();

        if (!(sender is IProject)) // if the whole project is disposed, there is no point in trying to watch something
        {
          // note Dispose is designed to let the hierarchy from child to parent (root) valid, but not from root to child!
          // thus trying to get an actual document path here is in must cases unsuccessfull. We have to rely on our stored path, and that it was always updated!
          // the only case were it is successfull if a new node immediately replaces an old document node
          var node = AbsoluteDocumentPath.GetNodeOrLeastResolveableNode(_docNodePath, senderAsNode, out var wasResolvedCompletely);
          if (wasResolvedCompletely)
          {
            InternalSetDocNode(node);
          }
          else
          {
            SetWatchOnNode(node);
          }

          shouldFireChangedEvent = true;
        }
      }
      else if (e is DocumentPathChangedEventArgs)
      {
        if (null != InternalDocumentNode)
        {
          InternalDocumentPath = Main.AbsoluteDocumentPath.GetAbsolutePath(InternalDocumentNode);
          InternalCheckAbsolutePath();
        }

        shouldFireChangedEvent = true;
      }

      shouldFireChangedEvent |= OnDocNode_TunnelingEvent(sender, source, e);

      if (shouldFireChangedEvent)
        EhSelfChanged(EventArgs.Empty);
    }

    /// <summary>
    /// Called when the doc node has changed. Can be overwritten in derived classes to implement additional functionality.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="source">The source of this event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <returns>True if the something has changed, thus the Changed event of the proxy (!) should be fired. Otherwise, false.</returns>
    protected virtual bool OnDocNode_TunnelingEvent(object sender, object source, Main.TunnelingEventArgs e)
    {
      return false;
    }

    /// <summary>
    /// Called when the doc node has changed. Can be overwritten in derived classes to implement additional functionality.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected virtual void OnDocNode_Changed(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Event handler that is called when the document node has changed. Because the path to the node can have changed too,
    /// the path is renewed in this case.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EhDocNode_Changed(object sender, EventArgs e)
    {
      if (IsDisposeInProgress)
        return;

#if DEBUG_DOCNODEPROXYLOGGING
			Current.Console.WriteLine("DocNodeProxy.EhDocNode_Changed: sender={0}, e={1}", sender, e);
#endif
      var iNode = InternalDocumentNode;
      if (null != iNode && !iNode.IsDisposeInProgress)
      {
        OnDocNode_Changed(sender, e);

        InternalDocumentPath = Main.AbsoluteDocumentPath.GetAbsolutePath(iNode);
        InternalCheckAbsolutePath();
        EhSelfChanged(EventArgs.Empty);
      }
    }

    /// <summary>
    /// Gets the internal document node instance, without changing anything and without trying to resolve the path.
    /// </summary>
    /// <value>
    /// The internal document node.
    /// </value>
    protected IDocumentLeafNode InternalDocumentNode
    {
      get
      {
        return (_docNodeRef == null ? null : _docNodeRef.Target) as IDocumentLeafNode;
      }
    }

    protected AbsoluteDocumentPath InternalDocumentPath
    {
      get
      {
        return _docNodePath;
      }
      set
      {
        if (null == value)
          throw new ArgumentNullException(nameof(value));

        _docNodePath = value;
      }
    }



    /// <summary>
    /// Returns the document node. If the stored doc node is null, it is tried to resolve the stored document path.
    /// If that fails too, null is returned.
    /// </summary>
    public virtual object DocumentObject()
    {

      var currentProject = Current.IProjectService.CurrentProject;
      if (null == currentProject) // probably we are loading the project now, and it is not set yet
      {
        var rootNode = AbsoluteDocumentPath.GetRootNode(this);
        currentProject = rootNode as IProject;

        if (null == currentProject)
        {
          throw new ApplicationException("Could not find document root. Please debug to find the node which has not set its ParentObject.");
        }
      }

      return ResolveDocumentObject(currentProject);
    }


    public virtual Main.AbsoluteDocumentPath DocumentPath()
    {

      var docNode = InternalDocumentNode;
      if (null != docNode)
      {
        InternalDocumentPath = Main.AbsoluteDocumentPath.GetAbsolutePath(docNode);
      }

      return InternalDocumentPath;
    }


    /// <summary>
    /// Resolves the document object.
    /// </summary>
    /// <param name="startnode">The startnode.</param>
    /// <returns></returns>
    /// <exception cref="InvalidProgramException">
    /// node should always be != null, since we use absolute paths, and at least an AltaxoDocument should be resolved here.
    /// </exception>
    protected virtual IDocumentLeafNode ResolveDocumentObject(Main.IDocumentLeafNode startnode)
    {
      if (IsDisposeInProgress)
        return null;

      if (!(_docNodePath != null || IsDisposeInProgress))
        throw new InvalidProgramException();

      var docNode = InternalDocumentNode;
      if (docNode == null)
      {
#if DEBUG_DOCNODEPROXYLOGGING
				Current.Console.WriteLine("DocNodeProxy.ResolveDocumentObject, path is <<{0}>>", _docNodePath);
#endif

#if DOCNODEPROXY_CONCURRENTDEBUG
				_debug.Enqueue("START ResolveDocumentObject");
#endif

        var node = Main.AbsoluteDocumentPath.GetNodeOrLeastResolveableNode(_docNodePath, startnode, out var wasCompletelyResolved);
        if (null == node)
          throw new InvalidProgramException("node should always be != null, since we use absolute paths, and at least an AltaxoDocument should be resolved here.");

        if (wasCompletelyResolved)
        {
          InternalSetDocNode(node);
          docNode = InternalDocumentNode;
        }
        else // not completely resolved
        {
          SetWatchOnNode(node);
        }

#if DOCNODEPROXY_CONCURRENTDEBUG
				_debug.Enqueue("STOP  ResolveDocumentObject");
#endif
      }
      return docNode;
    }

    /// <summary>
    /// Sets the watch on a node that is not our document node, but a node lower in the hierarchy. We watch both the Changed event and the TunneledEvent of this node.
    /// </summary>
    /// <param name="node">The node to watch.</param>
    protected virtual void SetWatchOnNode(IDocumentLeafNode node)
    {
#if DOCNODEPROXY_CONCURRENTDEBUG
			int debugUsn = System.Threading.Interlocked.Increment(ref _debugUSN);
			_debug.Enqueue("START SetWatchOnNode " + debugUsn.ToString());
#endif

      if (null == node)
        throw new ArgumentNullException(nameof(node));

      if (null != _weakDocNodeChangedHandler)
      {
        _weakDocNodeChangedHandler.Remove();
        _weakDocNodeChangedHandler = null;
      }
      if (null != _weakDocNodeTunneledEventHandler)
      {
        _weakDocNodeTunneledEventHandler.Remove();
        _weakDocNodeTunneledEventHandler = null;
      }

      node.TunneledEvent += (_weakDocNodeTunneledEventHandler = new WeakActionHandler<object, object, TunnelingEventArgs>(EhWatchedNode_TunneledEvent, handler => node.TunneledEvent -= handler));
      node.Changed += (_weakDocNodeChangedHandler = new WeakEventHandler(EhWatchedNode_Changed, handler => node.Changed -= handler));

#if DEBUG_DOCNODEPROXYLOGGING
			Current.Console.WriteLine("Start watching node <<{0}>> of total path <<{1}>>", AbsoluteDocumentPath.GetAbsolutePath(node), _docNodePath);
#endif

#if DOCNODEPROXY_CONCURRENTDEBUG
			_debug.Enqueue("STOP  SetWatchOnNode " + debugUsn.ToString() + (_docNodeRef == null).ToString());
#endif
    }

    /// <summary>
    /// Event handler that is called when the watched node (a node that is not the document node) has changed. Maybe this watched node had now created a parent node, and our
    /// document path can resolved now. That's why we try to resolve our document path now.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EhWatchedNode_Changed(object sender, EventArgs e)
    {
      if (IsDisposeInProgress)
        return;

#if DEBUG_DOCNODEPROXYLOGGING
			Current.Console.WriteLine("DocNodeProxy.EhWatchedNode_Changed: sender={0}, e={1}", sender, e);
#endif

      if (!(InternalDocumentNode == null))
        throw new InvalidProgramException();

      var senderAsDocNode = sender as IDocumentLeafNode;
      if (!(senderAsDocNode != null))
        throw new InvalidProgramException();


      var node = AbsoluteDocumentPath.GetNodeOrLeastResolveableNode(_docNodePath, senderAsDocNode, out var wasResolvedCompletely);
      if (null == node)
        throw new InvalidProgramException("node should always be != null, since we use absolute paths, and at least an AltaxoDocument should be resolved here.");

      if (wasResolvedCompletely)
      {
        ClearWatch();
        InternalSetDocNode(node);
      }
      else // not completely resolved
      {
        if (!object.ReferenceEquals(sender, node))
        {
          ClearWatch();
          SetWatchOnNode(node);
        }
      }
    }

    /// <summary>
    /// Event handler that is called when the watched node or a parent node below has disposed or its name changed. We then try to resolve the path again.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="source">Source of the tunneled event.</param>
    /// <param name="e"></param>
    private void EhWatchedNode_TunneledEvent(object sender, object source, Main.TunnelingEventArgs e)
    {
      if (IsDisposeInProgress)
        return;

      if (!(InternalDocumentNode == null))
        throw new InvalidProgramException();
      var senderAsDocNode = sender as IDocumentLeafNode;
      var sourceAsDocNode = source as IDocumentLeafNode;
      if (!(senderAsDocNode != null))
        throw new InvalidProgramException();
      if (!(sourceAsDocNode != null))
        throw new InvalidProgramException();

      if (e is DocumentPathChangedEventArgs) // here, we activly change our stored path, if the watched node or a parent has changed its name
      {
        var watchedPath = AbsoluteDocumentPath.GetAbsolutePath(senderAsDocNode);
        watchedPath = watchedPath.Append(_docNodePath.SubPath(watchedPath.Count, _docNodePath.Count - watchedPath.Count));
        var oldPath = _docNodePath;
        _docNodePath = watchedPath;

#if DEBUG_DOCNODEPROXYLOGGING
				Current.Console.WriteLine("DocNodeProxy.EhWatchedNode_TunneledEvent: Modified path, oldpath={0}, newpath={1}", oldPath, _docNodePath);
#endif
      }

      // then we try to resolve the path again
      if ((e is DisposeEventArgs) || (e is DocumentPathChangedEventArgs))
      {
#if DEBUG_DOCNODEPROXYLOGGING
				Current.Console.WriteLine("DocNodeProxy.EhWatchedNode_TunneledEvent");
#endif

        var node = AbsoluteDocumentPath.GetNodeOrLeastResolveableNode(_docNodePath, sourceAsDocNode, out var wasResolvedCompletely);
        if (null == node)
          throw new InvalidProgramException(nameof(node) + " should always be != null, since we use absolute paths, and at least an AltaxoDocument should be resolved here.");

        if (wasResolvedCompletely)
        {
          ClearWatch();
          InternalSetDocNode(node);
        }
        else // not completely resolved
        {
          if (!object.ReferenceEquals(sender, node))
          {
            ClearWatch();
            SetWatchOnNode(node);
          }
        }
      }
    }

    protected void EhXmlDeserializationFinished(Altaxo.Serialization.Xml.IXmlDeserializationInfo info, Main.IDocumentNode documentRoot, bool isFinallyCall)
    {
      if (null != ResolveDocumentObject(documentRoot) || isFinallyCall)
        info.DeserializationFinished -= new Altaxo.Serialization.Xml.XmlDeserializationCallbackEventHandler(EhXmlDeserializationFinished);
    }

    #region ICloneable Members

    public virtual object Clone()
    {
      return new DocNodeProxy(this);
    }

    #endregion ICloneable Members
  }
}
