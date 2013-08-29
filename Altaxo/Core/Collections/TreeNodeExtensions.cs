﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altaxo.Collections
{
	public interface ITreeNode<T> where T : ITreeNode<T>
	{
		IEnumerable<T> Nodes { get; }
	}

	public interface ITreeListNode<T> : ITreeNode<T> where T : ITreeListNode<T>
	{
		new IList<T> Nodes { get; }
	}

	public interface NodeWithParentNode<T>
	{
		T ParentNode { get; }
	}

	public interface ITreeNodeWithParent<T> : ITreeNode<T>, NodeWithParentNode<T> where T : ITreeNodeWithParent<T>
	{
	}

	public interface ITreeListNodeWithParent<T> : ITreeListNode<T>, NodeWithParentNode<T> where T : ITreeListNodeWithParent<T>
	{
	}

	/// <summary>
	/// Implements algorithms common for all trees.
	/// </summary>
	public static class TreeNodeExtensions
	{
		public static void FromHereToLeavesDo<T>(this T node, Action<T> action) where T : ITreeNode<T>
		{
			action(node);
			var childNodes = node.Nodes;
			if (null != childNodes)
			{
				foreach (var childNode in childNodes)
				{
					FromHereToLeavesDo<T>(childNode, action);
				}
			}
		}

		public static void FromLeavesToHereDo<T>(this T node, Action<T> action) where T : ITreeNode<T>
		{
			var childNodes = node.Nodes;
			if (null != childNodes)
			{
				foreach (var childNode in childNodes)
				{
					FromLeavesToHereDo<T>(childNode, action);
				}
			}
			action(node);
		}

		public static T AnyBetweenHereAndLeaves<T>(this T node, Func<T, bool> condition) where T : ITreeNode<T>
		{
			if (condition(node))
				return node;

			var childNodes = node.Nodes;
			if (null != childNodes)
			{
				foreach (var childNode in childNodes)
				{
					var result = AnyBetweenHereAndLeaves(childNode, condition);
					if (null != result)
						return result;
				}
			}
			return default(T);
		}

		public static T AnyBetweenLeavesAndHere<T>(this T node, Func<T, bool> condition) where T : ITreeNode<T>
		{
			var childNodes = node.Nodes;
			if (null != childNodes)
			{
				foreach (var childNode in childNodes)
				{
					var result = AnyBetweenLeavesAndHere(childNode, condition);
					if (null != result)
						return result;
				}
			}

			if (condition(node))
				return node;

			return default(T);
		}

		/// <summary>
		/// Enumerates through all tree nodes from (and including) the provided node <paramref name="node"/> up to the leaves of the tree.
		/// </summary>
		/// <typeparam name="T">Type of node</typeparam>
		/// <param name="node">The node to start the enumeration with.</param>
		/// <returns>All tree nodes from <paramref name="node"/> up to the leaves of the tree./returns>
		public static IEnumerable<T> TakeFromHereToFirstLeaves<T>(this T node) where T : ITreeNode<T>
		{
			return TakeFromHereToFirstLeaves(node, true);
		}

		/// <summary>
		/// Enumerates through all tree nodes from the provided node <paramref name="node"/> up to the leaves of the tree. If <paramref name="includeThisNode"/> is <c>true</c>, the provided node <paramref name="node"/> is included in the enumeration.
		/// </summary>
		/// <typeparam name="T">Type of node</typeparam>
		/// <param name="node">The node to start the enumeration with.</param>
		/// <param name="includeThisNode">If set to <c>true</c> the node <paramref name="node"/> is included in the enumeration, otherwise, it is not part of the enumeration.</param>
		/// <returns>All tree nodes from <paramref name="node"/> up to the leaves of the tree./returns>
		public static IEnumerable<T> TakeFromHereToFirstLeaves<T>(this T node, bool includeThisNode) where T : ITreeNode<T>
		{
			if (includeThisNode)
				yield return node;

			var childNodes = node.Nodes;
			if (null != childNodes)
			{
				foreach (var childNode in childNodes)
				{
					foreach (var subchild in TakeFromHereToFirstLeaves(childNode, true))
						yield return subchild;
				}
			}
		}


		/// <summary>
		/// Enumerates through all tree nodes from (and including) the provided node <paramref name="node"/> to the leaves of the tree. The downmost leaves will be enumerated first.
		/// </summary>
		/// <typeparam name="T">Type of node</typeparam>
		/// <param name="node">The node to start the enumeration with.</param>
		/// <returns>All tree nodes from <paramref name="node"/> up to the leaves of the tree. The downmost leaves will be enumerated first.</returns>
		public static IEnumerable<T> TakeFromHereToLastLeaves<T>(this T node) where T : ITreeNode<T>
		{
			return TakeFromHereToLastLeaves(node, true);
		}

		/// <summary>
		/// Enumerates through all tree nodes from the provided node <paramref name="node"/> to the leaves of the tree. The downmost leaves will be enumerated first. If <paramref name="includeThisNode"/> is <c>true</c>, the provided node <paramref name="node"/> is included in the enumeration.
		/// Attention: Since the order of the nodes must be reversed, this enumeration is only efficient for <see cref="ITreeListNode{T}"/> types.
		/// </summary>
		/// <typeparam name="T">Type of node</typeparam>
		/// <param name="node">The node to start the enumeration with.</param>
		/// <param name="includeThisNode">If set to <c>true</c> the node <paramref name="node"/> is included in the enumeration, otherwise, it is not part of the enumeration.</param>
		/// <returns>All tree nodes from <paramref name="node"/> to the leaves of the tree. The downmost leaves will be enumerated first.</returns>
		public static IEnumerable<T> TakeFromHereToLastLeaves<T>(this T node, bool includeThisNode) where T : ITreeNode<T>
		{
			if (includeThisNode)
				yield return node;

			var childNodes = node.Nodes;
			if (null != childNodes)
			{
				foreach (var childNode in childNodes.Reverse())
				{
					foreach (var subchild in TakeFromHereToLastLeaves(childNode, true))
						yield return subchild;
				}
			}
		}


		/// <summary>
		/// Enumerates through all tree nodes from the upmost leaf of the tree down to the provided node <paramref name="node"/>. The provided node <paramref name="node"/> is included in the enumeration.
		/// Attention: Since the order of the nodes must be reversed, this enumeration is only efficient for <see cref="ITreeListNode{T}"/> types.
		/// </summary>
		/// <typeparam name="T">Type of node</typeparam>
		/// <param name="node">The node to start the enumeration with.</param>
		/// <returns>All tree nodes from the upmost leaf of the tree down to the provided node <paramref name="node"/>.</returns>
		public static IEnumerable<T> TakeFromFirstLeavesToHere<T>(this T node) where T : ITreeNode<T>
		{
			return TakeFromFirstLeavesToHere(node, true);
		}

		/// <summary>
		/// Enumerates through all tree nodes from the upmost leaf of the tree down to the provided node <paramref name="node"/>. If <paramref name="includeThisNode"/> is <c>true</c>, the provided node <paramref name="node"/> is included in the enumeration.
		/// </summary>
		/// <typeparam name="T">Type of node</typeparam>
		/// <param name="node">The node to start the enumeration with.</param>
		/// <param name="includeThisNode">If set to <c>true</c> the node <paramref name="node"/> is included in the enumeration, otherwise, it is not part of the enumeration.</param>
		/// <returns>All tree nodes from the upmost leaf of the tree down to the provided node <paramref name="node"/>.</returns>
		public static IEnumerable<T> TakeFromFirstLeavesToHere<T>(this T node, bool includeThisNode) where T : ITreeNode<T>
		{
			var childNodes = node.Nodes;
			if (null != childNodes)
			{
				foreach (var childNode in childNodes)
				{
					foreach (var subchild in TakeFromFirstLeavesToHere(childNode, true))
						yield return subchild;
				}
			}

			if (includeThisNode)
				yield return node;
		}



		/// <summary>
		/// Enumerates through all tree nodes from the downmost leaf of the tree down to the provided node <paramref name="node"/>. The provided node <paramref name="node"/> is included in the enumeration.
		/// Attention: Since the order of the nodes must be reversed, this enumeration is only efficient for <see cref="ITreeListNode{T}"/> types.
		/// </summary>
		/// <typeparam name="T">Type of node</typeparam>
		/// <param name="node">The node to start the enumeration with.</param>
		/// <returns>All tree nodes from the downmost leaf of the tree down to the provided node <paramref name="node"/>./returns>
		public static IEnumerable<T> TakeFromLeavesToHere<T>(this T node) where T : ITreeNode<T>
		{
			return TakeFromLastLeavesToHere(node, true);
		}


		/// <summary>
		/// Enumerates through all tree nodes from the downmost leaf of the tree down to the provided node <paramref name="node"/>. If <paramref name="includeThisNode"/> is <c>true</c>, the provided node <paramref name="node"/> is included in the enumeration.
		/// Attention: Since the order of the nodes must be reversed, this enumeration is only efficient for <see cref="ITreeListNode{T}"/> types.
		/// </summary>
		/// <typeparam name="T">Type of node</typeparam>
		/// <param name="node">The node to start the enumeration with.</param>
		/// <param name="includeThisNode">If set to <c>true</c> the node <paramref name="node"/> is included in the enumeration, otherwise, it is not part of the enumeration.</param>
		/// <returns>All tree nodes from the downmost leaf of the tree down to the provided node <paramref name="node"/>./returns>
		public static IEnumerable<T> TakeFromLastLeavesToHere<T>(this T node, bool includeThisNode) where T : ITreeNode<T>
		{
			var childNodes = node.Nodes;
			if (null != childNodes)
			{
				foreach (var childNode in childNodes.Reverse())
				{
					foreach (var subchild in TakeFromLastLeavesToHere(childNode, true))
						yield return subchild;
				}
			}

			if (includeThisNode)
				yield return node;
		}

		/// <summary>
		/// Projects a tree (source tree) to a new tree (destination tree).
		/// </summary>
		/// <typeparam name="S">Type of the source tree node.</typeparam>
		/// <typeparam name="D">Type of the destination tree node.</typeparam>
		/// <param name="sourceRoot">The source root tree node.</param>
		/// <param name="createDestinationNodeFromSourceNode">Function used to create a destination node from a source node.</param>
		/// <param name="addChildToDestinationNode">Procedure to add a child node to a destination node (first argument is the parent node, 2nd argument is the child node).</param>
		/// <returns>The root node of the newly created destination tree that reflects the structure of the source tree.</returns>
		public static D ProjectTreeToNewTree<S, D>(this S sourceRoot, Func<S, D> createDestinationNodeFromSourceNode, Action<D, D> addChildToDestinationNode)
			where D : ITreeNode<D>
			where S : ITreeNode<S>
		{
			var destRoot = createDestinationNodeFromSourceNode(sourceRoot);

			var sourceChildNodes = sourceRoot.Nodes;
			if (null != sourceChildNodes)
			{
				foreach (var sourceChild in sourceChildNodes)
				{
					var destChild = ProjectTreeToNewTree(sourceChild, createDestinationNodeFromSourceNode, addChildToDestinationNode);
					addChildToDestinationNode(destRoot, destChild);
				}
			}
			return destRoot;
		}

		/// <summary>
		/// Projects a tree (source tree) to a new tree (destination tree). The creation function for the new tree nodes gets information about the node indices.
		/// </summary>
		/// <typeparam name="S">Type of the source tree node.</typeparam>
		/// <typeparam name="D">Type of the destination tree node.</typeparam>
		/// <param name="sourceRoot">The source root tree node.</param>
		/// <param name="indices">List of indices that describes the destination root node. If this parameter is null, an internal list will be created, and the destination root node will get the index 0.</param>
		/// <param name="createDestinationNodeFromSourceNode">Function used to create a destination node from a source node. First parameter is the source node, 2nd parameter is a list of indices that describe the destination node.</param>
		/// <param name="addChildToDestinationNode">Procedure to add a child node to a destination node (first argument is the parent node, 2nd argument is the child node).</param>
		/// <returns>The root node of the newly created destination tree that reflects the structure of the source tree.</returns>
		public static D ProjectTreeToNewTree<S, D>(this S sourceRoot, IList<int> indices, Func<S, IList<int>, D> createDestinationNodeFromSourceNode, Action<D, D> addChildToDestinationNode)
			where D : ITreeNode<D>
			where S : ITreeNode<S>
		{
			if (null == indices)
				indices = new List<int>();
			if (0 == indices.Count)
				indices.Add(0);
			var destRoot = createDestinationNodeFromSourceNode(sourceRoot, indices);

			var sourceChildNodes = sourceRoot.Nodes;
			if (null != sourceChildNodes)
			{
				indices.Add(0);
				foreach (var sourceChild in sourceChildNodes)
				{
					var destChild = ProjectTreeToNewTree(sourceChild, indices, createDestinationNodeFromSourceNode, addChildToDestinationNode);
					addChildToDestinationNode(destRoot, destChild);
					++indices[indices.Count - 1];
				}
				indices.RemoveAt(indices.Count - 1);
			}

			return destRoot;
		}

		/// <summary>
		/// Ensures that a list of indices that point to a node in a tree is valid.
		/// </summary>
		/// <typeparam name="T">Type of the tree node.</typeparam>
		/// <param name="rootNode">The root node of the tree.</param>
		/// <param name="index">The index list. On return, it is ensured that this index list designates a valid index of a node inside the tree.</param>
		/// <returns><c>True</c> if the index list was changed to ensure validity, <c>False</c> if it was not neccessary to change the index list.</returns>
		public static bool EnsureValidityOfNodeIndex<T>(this T rootNode, IList<int> index) where T : ITreeListNode<T>
		{
			if (null == rootNode)
				throw new ArgumentNullException("rootNode");
			if (null == index)
				throw new ArgumentNullException("index");

			if (index.Count > 0)
				return EnsureValidityOfNodeIndex(rootNode.Nodes, index, 0);

			return false;
		}

		/// <summary>
		/// Ensures that a list of indices that point to a node in a tree is valid. Here, only the childs of the root node are tested.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="nodes">The nodes collection of a node of the tree.</param>
		/// <param name="index">The index list. On return, it is ensured that this index list designates a valid index of a node inside the tree.</param>
		/// <param name="level">The level. Must be always greater than 0.</param>
		/// <returns><c>True</c> if the index list was changed to ensure validity, <c>False</c> if it was not neccessary to change the index list.</returns>
		private static bool EnsureValidityOfNodeIndex<T>(IList<T> nodes, IList<int> index, int level) where T : ITreeListNode<T>
		{
			if (null == nodes || 0 == nodes.Count)
			{
				for (int i = index.Count - 1; i >= level; --i)
					index.RemoveAt(i);
				return true;
			}

			if (index[level] < 0)
			{
				index[level] = 0;
				for (int i = index.Count - 1; i > level; --i)
					index.RemoveAt(i);
				return true;
			}
			else if (index[level] >= nodes.Count)
			{
				index[level] = nodes.Count - 1;
				for (int i = index.Count - 1; i > level; --i)
					index.RemoveAt(i);
				return true;
			}

			if (index.Count > (level + 1))
			{
				return EnsureValidityOfNodeIndex(nodes[index[level]].Nodes, index, level + 1);
			}

			return false;
		}

		/// <summary>
		/// Gets a node inside a tree by using an index array.
		/// </summary>
		/// <typeparam name="T">Type of node.</typeparam>
		/// <param name="rootNode">The root node of the tree.</param>
		/// <param name="index">The index array. The member at index 0 must always be 0, since this indicates the provided root node. Examples: {0} designates the root node; {0, 1} designates the 2nd child of the root node; {0,1,0} designates the first child of the second child of the root node.</param>
		/// <returns>The node that is designated by the provided index.</returns>
		/// <exception cref="System.ArgumentNullException">rootNode</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Index list is null or empty; or index[0] is not 0; or index is otherwise invalid.
		/// </exception>
		public static T ElementAt<T>(this T rootNode, IEnumerable<int> index) where T : ITreeListNode<T>
		{
			if (null == rootNode)
				throw new ArgumentNullException("rootNode");

			if (null == index)
				throw new ArgumentOutOfRangeException("index is null ");

			T result;
			using (var it = index.GetEnumerator())
			{
				if (it.MoveNext())
					result = ElementAtInternal(rootNode.Nodes, it, 1);
				else
					result = rootNode; // List is empty => return the root node.
			}
			return result;
		}

		private static T ElementAtInternal<T>(IList<T> nodes, IEnumerator<int> it, int level) where T : ITreeListNode<T>
		{
			// check this in a public function before making a call to this: rootNode != null, index.Count>0, level>0

			if (null == nodes || 0 == nodes.Count)
				throw new ArgumentOutOfRangeException(string.Format("node at level {0} does not has children as expected", level - 1));

			var idx = it.Current;
			if (idx < 0)
				throw new ArgumentOutOfRangeException(string.Format("index at level {0} is < 0", level));
			else if (idx >= nodes.Count)
				throw new ArgumentOutOfRangeException(string.Format("index at level {0} is greater than number of child nodes", level));

			if (it.MoveNext())
				return ElementAtInternal(nodes[idx].Nodes, it, level + 1);
			else
				return nodes[idx];
		}


		/// <summary>
		/// Determines whether the given <paramref name="index"/> is valid or not.
		/// </summary>
		/// <typeparam name="T">Type of nodes.</typeparam>
		/// <param name="rootNode">The root node of the tree.</param>
		/// <param name="index">The index that points to a node inside the tree.</param>
		/// <returns><c>true</c> if the given index is valid; otherwise, <c>false</c>.</returns>
		public static bool IsValidIndex<T>(this T rootNode, IEnumerable<int> index) where T : ITreeListNode<T>
		{
			T nodeAtIndex;
			return IsValidIndex(rootNode, index, out nodeAtIndex);
		}

		/// <summary>
		/// Determines whether the given <paramref name="index"/> is valid or not.
		/// </summary>
		/// <typeparam name="T">Type of nodes.</typeparam>
		/// <param name="rootNode">The root node of the tree.</param>
		/// <param name="index">The index that points to a node inside the tree.</param>
		/// <param name="nodeAtIndex">If the return value was true, this parameter contains the node at the given index.</param>
		/// <returns><c>true</c> if the given index is valid; otherwise, <c>false</c>.</returns>
		public static bool IsValidIndex<T>(this T rootNode, IEnumerable<int> index, out T nodeAtIndex) where T : ITreeListNode<T>
		{
			if (null == rootNode)
				throw new ArgumentNullException("rootNode");

			if (null == index)
			{
				nodeAtIndex = default(T);
				return false;
			}

			bool result;
			using (var it = index.GetEnumerator())
			{
				if (it.MoveNext())
					result = IsValidIndex(rootNode.Nodes, it, 1, out nodeAtIndex);
				else
				{
					nodeAtIndex = rootNode;
					result = true; // List is empty => return true, since this is the root node.
				}
			}
			return result;
		}


		private static bool IsValidIndex<T>(IList<T> nodes, IEnumerator<int> it, int level, out T nodeAtIndex) where T : ITreeListNode<T>
		{
			nodeAtIndex = default(T);

			if (null == nodes || 0 == nodes.Count)
				return false;

			var idx = it.Current;
			if (idx < 0)
				return false; // throw new ArgumentOutOfRangeException(string.Format("index at level {0} is < 0", level));
			else if (idx >= nodes.Count)
				return false; //  throw new ArgumentOutOfRangeException(string.Format("index at level {0} is greater than number of child nodes", level));

			if (it.MoveNext())
			{
				return IsValidIndex(nodes[idx].Nodes, it, level + 1, out nodeAtIndex);
			}
			else
			{
				nodeAtIndex = nodes[idx];
				return true;
			}
		}


		/// <summary>
		/// Inserts the specified node at a certain index in the tree.
		/// </summary>
		/// <typeparam name="T">Type of node.</typeparam>
		/// <param name="rootNode">The root node of the tree.</param>
		/// <param name="index">The index inside the tree where the node should be inserted.</param>
		/// <param name="nodeToInsert">The node to insert.</param>
		public static void Insert<T>(this T rootNode, IEnumerable<int> index, T nodeToInsert) where T : ITreeListNode<T>
		{
			if (null == rootNode)
				throw new ArgumentNullException("rootNode");
			if (null == index)
				throw new ArgumentNullException("index");
			if (null == nodeToInsert)
				throw new ArgumentNullException("nodeToInsert");
		
			var parent = ElementAt(rootNode, index.TakeAllButLast());
			parent.Nodes.Insert(index.Last(), nodeToInsert);
		}

		/// <summary>
		/// Inserts the specified node after a certain index in the tree.
		/// </summary>
		/// <typeparam name="T">Type of node.</typeparam>
		/// <param name="rootNode">The root node of the tree.</param>
		/// <param name="index">The index inside the tree after which the node should be inserted.</param>
		/// <param name="nodeToInsert">The node to insert.</param>
		public static void InsertAfter<T>(this T rootNode, IEnumerable<int> index, T nodeToInsert) where T : ITreeListNode<T>
		{
			if (null == rootNode)
				throw new ArgumentNullException("rootNode");
			if (null == index)
				throw new ArgumentNullException("index");
			if (null == nodeToInsert)
				throw new ArgumentNullException("nodeToInsert");

			var parent = ElementAt(rootNode, index.TakeAllButLast());
			var idx = index.Last();
			parent.Nodes.Insert(idx+1, nodeToInsert);
		}


		/// <summary>
		/// Inserts the specified node after all other siblings of the node at a certain index in the tree.
		/// </summary>
		/// <typeparam name="T">Type of node.</typeparam>
		/// <param name="rootNode">The root node of the tree.</param>
		/// <param name="index">The index inside the tree that points to a node. The <paramref name="nodeToInsert"/> is inserted at the end of the same collection that this node belongs to.</param>
		/// <param name="nodeToInsert">The node to insert.</param>
		public static void InsertLast<T>(this T rootNode, IEnumerable<int> index, T nodeToInsert) where T : ITreeListNode<T>
		{
			if (null == rootNode)
				throw new ArgumentNullException("rootNode");
			if (null == index)
				throw new ArgumentNullException("index");
			if (null == nodeToInsert)
				throw new ArgumentNullException("nodeToInsert");

			var parent = ElementAt(rootNode, index.TakeAllButLast());
			var idx = index.Last();
			parent.Nodes.Add(nodeToInsert);
		}

		/// <summary>
		/// Gets the index of a given node inside a tree.
		/// </summary>
		/// <typeparam name="T">Type of the node.</typeparam>
		/// <param name="node">The node for which the index is determined.</param>
		/// <returns>Index of the node inside the tree.</returns>
		public static IList<int> IndexOf<T>(this T node) where T : ITreeListNodeWithParent<T>
		{
			var result = new List<int>();
			IndexOfInternal(node, result);
			return result;
		}

		/// <summary>
		/// Gets the index of a given node inside a tree.
		/// </summary>
		/// <typeparam name="T">Type of the node.</typeparam>
		/// <param name="node">The node for which the index is determined.</param>
		/// <param name="existingList">List that can be used to hold the indices. If this parameter is null, a new List will be created.</param>
		/// <returns>Index of the node inside the tree. If <paramref name="existingList"/> was not null, the  <paramref name="existingList"/> is returned. Otherwise, a new list is returned.</returns>
		public static IList<int> IndexOf<T>(this T node, IList<int> existingList) where T : ITreeListNodeWithParent<T>
		{
			if (existingList == null)
				existingList = new List<int>();
			IndexOfInternal(node, existingList);
			return existingList;
		}

		private static void IndexOfInternal<T>(this T node, IList<int> list) where T : ITreeListNodeWithParent<T>
		{
			if (node.ParentNode == null) // node is the root node of the tree
			{
				list.Clear();
			}
			else
			{
				IndexOfInternal(node.ParentNode, list);
				var childColl = node.ParentNode.Nodes;
				int idx = null == childColl ? -1 : childColl.IndexOf(node);
				if (idx < 0)
					throw new InvalidOperationException(string.Format("Tree node {0} is not contained in the Nodes collection of its parent", node));
				list.Add(idx);
			}
		}


		/// <summary>
		/// Fixes the and test the parent-child relationship in a tree.
		/// </summary>
		/// <typeparam name="T">Type of node of the tree.</typeparam>
		/// <param name="node">The node where the test starts (normally the root node of the tree).</param>
		/// <param name="Set1stArgParentNodeTo2ndArg">Action to set the Parent node property of a node given as the 1st argument to a node given as 2nd argument.</param>
		/// <returns>True if something changed (i.e. the parent-child relationship was broken), false otherwise.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// node is null 
		/// or
		/// Set1stArgParentNodeTo2ndArg is null.
		/// </exception>
		public static bool FixAndTestParentChildRelations<T>(this T node, Action<T, T> Set1stArgParentNodeTo2ndArg) where T : ITreeListNodeWithParent<T>
		{
			if (null == node)
				throw new ArgumentNullException("node");
			if (null == Set1stArgParentNodeTo2ndArg)
				throw new ArgumentNullException("Set1stArgParentNodeTo2ndArg");
			return FixAndTestParentChildRelationsInternal(node, Set1stArgParentNodeTo2ndArg);
		}

		private static bool FixAndTestParentChildRelationsInternal<T>(this T node, Action<T,T> Set1stArgParentNodeTo2ndArg) where T : ITreeListNodeWithParent<T>
		{
			bool changed = false;
			if (null != node.Nodes)
			{
				foreach (var child in node.Nodes)
				{
					if (!object.ReferenceEquals(node, child.ParentNode))
					{
						Set1stArgParentNodeTo2ndArg(child, node);
						changed = true;
					}

					changed |= FixAndTestParentChildRelations(child, Set1stArgParentNodeTo2ndArg);
				}
			}
			return changed;
		}



		/// <summary>
		/// Enumerates through all tree nodes from (and including) the provided node <paramref name="node"/> down to the root node.
		/// </summary>
		/// <typeparam name="T">Type of node</typeparam>
		/// <param name="node">The node to start the enumeration with.</param>
		/// <returns>All tree nodes from <paramref name="node"/> down to the root of the tree.</returns>
		public static IEnumerable<T> TakeFromHereToRoot<T>(this T node) where T : NodeWithParentNode<T>
		{
			if (null == node)
				throw new ArgumentNullException("node");

			yield return node;

			node = node.ParentNode;
			while (null != node)
			{
				yield return node;
				node = node.ParentNode;
			}
		}


		/// <summary>
		/// Enumerates through all tree nodes from the root node of the tree to (and including) the provided node <paramref name="node"/>.
		/// </summary>
		/// <typeparam name="T">Type of node</typeparam>
		/// <param name="node">The node inside a tree that the enumeration ends with.</param>
		/// <returns>All tree nodes from the root node of the tree up to <paramref name="node"/>.</returns>
		/// <exception cref="System.ArgumentNullException">Node is null.</exception>
		public static IEnumerable<T> TakeFromRootToHere<T>(this T node)	where T : NodeWithParentNode<T>
		{
			if (null == node)
				throw new ArgumentNullException("node");

			var list = new List<T>();

			list.Add(node);
			node = node.ParentNode;
			while (null != node)
			{
				list.Add(node);
				node = node.ParentNode;
			}
			for (int i = list.Count - 1; i >= 0; --i)
				yield return list[i];
		}

		/// <summary>
		/// Gets the root node of a tree to which the given node <paramref name="node"/> belongs.
		/// </summary>
		/// <param name="node">The node to start with.</param>
		/// <returns>The root node of the tree to which the given node <paramref name="node"/> belongs.</returns>
		public static T RootNode<T>(this T node)	where T : NodeWithParentNode<T>
		{
			if (null == node)
				throw new ArgumentNullException("node");

			var parent = node.ParentNode;
			while (null != parent)
			{
				node = parent;
				parent = node.ParentNode;
			}

			return node;
		}
	}
}