// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision: 955 $</version>
// </file>

using ICSharpCode.SharpDevelop.Project;
using System;
using System.Collections.Generic;

namespace ICSharpCode.SharpDevelop.Tests.WebReferences
{
	/// <summary>
	/// Helper methods used when testing web references
	/// </summary>
	public class WebReferenceTestHelper
	{
		WebReferenceTestHelper()
		{
		}
		
		public static ProjectItem GetProjectItem(List<ProjectItem> items, string include, ItemType itemType) {
			foreach (ProjectItem item in items) {
				if (item.ItemType == itemType) {
					if (item.Include == include) {
						return item;
					}
				}
			}
			return null;
		}
		
		public static FileProjectItem GetFileProjectItem(List<ProjectItem> items, string include, ItemType itemType) {
			foreach (ProjectItem item in items) {
				if (item.ItemType == itemType) {
					if (item.Include == include) {
						return (FileProjectItem)item;
					}
				}
			}
			return null;
		}
		
		public static ProjectItem GetProjectItem(List<ProjectItem> items, ItemType itemType)
		{
			foreach (ProjectItem item in items) {
				if (item.ItemType == itemType) {
					return item;
				}
			}
			return null;
		}
		
		public static WebReferencesFolderNode GetWebReferencesFolderNode(ProjectNode projectNode)
		{
			foreach (AbstractProjectBrowserTreeNode node in projectNode.Nodes) {
				if (node is WebReferencesFolderNode) {
					return (WebReferencesFolderNode)node;
				}
			}
			return null;
		}
		
		public static WebReferenceNode GetWebReferenceNode(WebReferencesFolderNode webReferencesFolderNode) {
			foreach (AbstractProjectBrowserTreeNode node in webReferencesFolderNode.Nodes) {
				if (node is WebReferenceNode) {
					return (WebReferenceNode)node;
				}
			}
			return null;
		}
		
		public static FileNode GetFileNode(AbstractProjectBrowserTreeNode parent, string fileName)
		{
			foreach (AbstractProjectBrowserTreeNode node in parent.Nodes) {
				FileNode fileNode = node as FileNode;
				if (fileNode != null && fileNode.FileName == fileName) {
					return fileNode;
				}
			}
			return null;
		}
	}
}