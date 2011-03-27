#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2007 Dr. Dirk Lellinger
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
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Altaxo.Graph.Plot.Groups
{
  public class PlotGroupStyleCollectionBase : IPlotGroupStyleCollection
  {
    #region Internal Class
    protected class GroupInfo : ICloneable
    {
      public bool WasApplied;
      public System.Type ChildGroupType;
      public System.Type ParentGroupType;

      public GroupInfo()
      {
      }

      public GroupInfo(GroupInfo from)
      {
        this.WasApplied = false;
        this.ChildGroupType = from.ChildGroupType;
        this.ParentGroupType = from.ParentGroupType;
      }

      #region ICloneable Members

      object ICloneable.Clone()
      {
        return new GroupInfo(this);
      }
      public GroupInfo Clone()
      {
        return new GroupInfo(this);
      }

      #endregion
    }
    #endregion

		/// <summary>Dictionary, which stores exactly one instance of a plot group style per type. Thus it is ensured, that only one instance per type is existent in this collection.</summary>
    protected Dictionary<System.Type, IPlotGroupStyle> _typeToInstance;

		/// <summary>Dictionary, which stores additional information for each plot group style. Since only one instance of each style can be stored, the key here is the type of the plot group style.</summary>
		protected Dictionary<System.Type, GroupInfo> _typeToInfo;
  
		/// <summary>Strictness of the plot group collection. This determines how the styles are distributed among the plot items.</summary>
		protected PlotGroupStrictness _plotGroupStrictness;


		protected bool _inheritFromParentGroups;
    
		protected bool _distributeToChildGroups;

    #region Serialization
    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(PlotGroupStyleCollectionBase), 0)]
    class XmlSerializationSurrogate0 : Altaxo.Serialization.Xml.IXmlSerializationSurrogate
    {
      public virtual void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        PlotGroupStyleCollectionBase s = (PlotGroupStyleCollectionBase)obj;

        int savedStyles = 0; // for test of consistency
        info.CreateArray("Styles", s.Count);

        foreach (System.Type t in s._typeToInstance.Keys)
        {
          if (s._typeToInfo[t].ParentGroupType != null)
            continue;

          info.AddValue("Style", s._typeToInstance[t]);
          info.AddValue("HasChild", null != s._typeToInfo[t].ChildGroupType);
          savedStyles++;

          System.Type childtype = t;
          while (null != (childtype = s._typeToInfo[childtype].ChildGroupType))
          {
            info.AddValue("Style", s._typeToInstance[childtype]);
            info.AddValue("HasChild", null != s._typeToInfo[childtype].ChildGroupType);
            savedStyles++;
          }
        }

        info.CommitArray();

        if (s.Count != savedStyles)
          throw new ApplicationException("Inconsistency in parent-child relationship in this PlotGroupStyleCollection. Please inform the author");

        info.AddEnum("Strictness", s._plotGroupStrictness);
      }
      public object Deserialize(object o, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        PlotGroupStyleCollectionBase s = null != o ? (PlotGroupStyleCollectionBase)o : new PlotGroupStyleCollectionBase();
        SDeserialize(s, info, parent);
        return s;
      }

      public virtual void SDeserialize(PlotGroupStyleCollectionBase s, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        Type parentStyleType = null;
        int count = info.OpenArray();
        for (int i = 0; i < count; i++)
        {
          IPlotGroupStyle style = (IPlotGroupStyle)info.GetValue("Style", s);
          bool hasChild = info.GetBoolean("HasChild");
          s.Add(style, parentStyleType);
          parentStyleType = hasChild ? style.GetType() : null;
        }
        info.CloseArray(count);

        s._plotGroupStrictness = (PlotGroupStrictness)info.GetEnum("Strictness", typeof(PlotGroupStrictness));
      }
    }

    [Altaxo.Serialization.Xml.XmlSerializationSurrogateFor(typeof(PlotGroupStyleCollectionBase), 1)]
    class XmlSerializationSurrogate1 : XmlSerializationSurrogate0
    {
      public override void Serialize(object obj, Altaxo.Serialization.Xml.IXmlSerializationInfo info)
      {
        base.Serialize(obj, info);
        PlotGroupStyleCollectionBase s = (PlotGroupStyleCollectionBase)obj;
        info.AddValue("InheritFromParent", s._inheritFromParentGroups);
        info.AddValue("DistributeToChilds", s._distributeToChildGroups);
      }
      public override void SDeserialize(PlotGroupStyleCollectionBase s, Altaxo.Serialization.Xml.IXmlDeserializationInfo info, object parent)
      {
        base.SDeserialize(s, info, parent);
        s._inheritFromParentGroups = info.GetBoolean("InheritFromParent");
        s._distributeToChildGroups = info.GetBoolean("DistributeToChilds");
      }


    }


    #endregion

    #region Constructors
    public PlotGroupStyleCollectionBase()
    {
      _typeToInstance = new Dictionary<Type, IPlotGroupStyle>();
      _typeToInfo = new Dictionary<Type, GroupInfo>();
      _inheritFromParentGroups = true;
    }

    public PlotGroupStyleCollectionBase(PlotGroupStyleCollectionBase from)
    {
      CopyFrom(from);
    }

    public virtual void CopyFrom(PlotGroupStyleCollectionBase from)
    {
			if (object.ReferenceEquals(this, from))
				return;

      _typeToInstance = new Dictionary<Type, IPlotGroupStyle>();
      _typeToInfo = new Dictionary<Type, GroupInfo>();

      foreach (KeyValuePair<System.Type, IPlotGroupStyle> entry in from._typeToInstance)
        this._typeToInstance.Add(entry.Key, (IPlotGroupStyle)entry.Value.Clone());

      foreach (KeyValuePair<System.Type, GroupInfo> entry in from._typeToInfo)
        this._typeToInfo.Add(entry.Key, entry.Value.Clone());

      _plotGroupStrictness = from._plotGroupStrictness;
      this._inheritFromParentGroups = from._inheritFromParentGroups;
      this._distributeToChildGroups = from._distributeToChildGroups;
    }

    #endregion

    #region ICloneable Members

    public PlotGroupStyleCollectionBase Clone()
    {
      return new PlotGroupStyleCollectionBase(this);
    }

    object ICloneable.Clone()
    {
      return new PlotGroupStyleCollectionBase(this);
    }

    #endregion


 

    /// <summary>
    /// Returns the plot group strictness of this plot group, i.e. how the plot group is updated.
    /// </summary>
    public PlotGroupStrictness PlotGroupStrictness
    {
      get { return _plotGroupStrictness; }
      set { _plotGroupStrictness = value; }
    }

    public bool InheritFromParentGroups
    {
      get { return _inheritFromParentGroups; }
      set { _inheritFromParentGroups = value; }
    }

    public bool DistributeToChildGroups
    {
      get { return _distributeToChildGroups; }
      set { _distributeToChildGroups = value; }
    }

		/// <summary>
		/// Tests, whether or not a group style of a given type is existent in the collection.
		/// </summary>
		/// <param name="groupStyleType">Type of plot group style to test.</param>
		/// <returns>True if the type is existent in this collection.</returns>
    public bool ContainsType(System.Type groupStyleType)
    {
      return _typeToInstance.ContainsKey(groupStyleType);
    }

		/// <summary>
		/// Gets the instance of the plot group style of a given type.
		/// </summary>
		/// <param name="groupStyleType">The type of plot group style.</param>
		/// <returns>The instance of the plot group style of the type given in the parameter.</returns>
    public IPlotGroupStyle GetPlotGroupStyle(System.Type groupStyleType)
    {
      return _typeToInstance[groupStyleType];
    }

		/// <summary>
		/// Retries the type of a child plot group style, if one exists.
		/// </summary>
		/// <param name="groupStyleType">The type of group style for which the type of child group style should be retrieved.</param>
		/// <returns>Type of the child plot group style, or null if no child exists.</returns>
    public System.Type GetTypeOfChild(System.Type groupStyleType)
    {
      return _typeToInfo[groupStyleType].ChildGroupType;
    }

		/// <summary>
		/// Retrieves the type of the parent plot group style, if one exists.
		/// </summary>
		/// <param name="groupStyleType">The type of group style for which the type of parent group style should be retrieved.</param>
		/// <returns>Type of the parent plot group style, or null if no parent exists.</returns>
    public System.Type GetParentTypeOf(System.Type groupStyleType)
    {
      return _typeToInfo[groupStyleType].ParentGroupType;
    }

		/// <summary>
		/// Determines the tree level of a group style, i.e. how many parent items it has.
		/// </summary>
		/// <param name="groupStyleType">Type of group style.</param>
		/// <returns>Number of parent items in the tree above the item.</returns>
		public int GetTreeLevelOf(System.Type groupStyleType)
		{
			int result = 0;
			System.Type t = groupStyleType;
			while (null != (t = _typeToInfo[t].ParentGroupType))
				++result;

			return result;
		}

		/// <summary>
		/// Number of styles in this collection.
		/// </summary>
    public int Count
    {
      get
      {
        return _typeToInstance.Count;
      }
    }

		/// <summary>
		/// Adds a group style to this collection. An exception will be thrown if an item of the same type already exists in the collection.
		/// </summary>
		/// <param name="groupStyle">Group style to add.</param>
    public void Add(IPlotGroupStyle groupStyle)
    {
      Add(groupStyle, null);
    }

		/// <summary>
		/// Adds a group style to this collection, as a child of another group style. If the parent group style doesn't allow childs, then the group style is added normally.
		/// </summary>
		/// <param name="groupStyle">Group style to add to this collection.</param>
		/// <param name="parentGroupStyleType">Type of the parent group style.</param>
    public void Add(IPlotGroupStyle groupStyle, System.Type parentGroupStyleType)
    {
			if (parentGroupStyleType != null)
			{
				if (!_typeToInstance.ContainsKey(parentGroupStyleType))
					throw new ArgumentException(string.Format("The parent group style (of type: {0}) can not be found in this collection", parentGroupStyleType));
				var parentInstance = _typeToInstance[parentGroupStyleType];
				if (!parentInstance.CanCarryOver)
				{
					Add(groupStyle);
					return;
				}
			}
			
			
			if (groupStyle == null)
        throw new ArgumentNullException("Try to add a null value to this group style collection");

      if (_typeToInstance.ContainsKey(groupStyle.GetType()))
        throw new ArgumentException(string.Format("The group style type <<{0}>> is already present in this group style collection", groupStyle.GetType()));

      _typeToInstance.Add(groupStyle.GetType(), groupStyle);
      GroupInfo groupInfo = new GroupInfo();
      groupInfo.ParentGroupType = parentGroupStyleType;
      _typeToInfo.Add(groupStyle.GetType(), groupInfo);


      if (parentGroupStyleType != null)
      {
        System.Type oldChildType = _typeToInfo[parentGroupStyleType].ChildGroupType;
        _typeToInfo[parentGroupStyleType].ChildGroupType = groupStyle.GetType();
        groupInfo.ChildGroupType = oldChildType;
        if (oldChildType != null)
          _typeToInfo[oldChildType].ParentGroupType = groupStyle.GetType();
      }
    }


    /// <summary>
    /// Inserts a group style by adding it. The child group style type is appended as child to this group type.
    /// In case the child type has already a parent, then this parent will be the parent of the inserted group style.
    /// </summary>
    /// <param name="groupStyle">Group style to add.</param>
    /// <param name="childGroupStyleType">Type of group style, which should be the child of the added group style.</param>
    public void Insert(IPlotGroupStyle groupStyle, System.Type childGroupStyleType)
    {
      if (groupStyle == null)
        throw new ArgumentNullException("Try to add a null value to this group style collection");

      if (_typeToInstance.ContainsKey(groupStyle.GetType()))
        throw new ArgumentException(string.Format("The group style type <<{0}>> is already present in this group style collection", groupStyle.GetType()));

      if (childGroupStyleType != null && !_typeToInstance.ContainsKey(childGroupStyleType))
        throw new ArgumentException(string.Format("The child group style (of type: {0}) can not be found in this collection", childGroupStyleType));

      _typeToInstance.Add(groupStyle.GetType(), groupStyle);
      GroupInfo groupInfo = new GroupInfo();
      groupInfo.ChildGroupType = childGroupStyleType;
      _typeToInfo.Add(groupStyle.GetType(), groupInfo);


      if (childGroupStyleType != null)
      {
        System.Type oldParentType = _typeToInfo[childGroupStyleType].ParentGroupType;
        _typeToInfo[childGroupStyleType].ParentGroupType = groupStyle.GetType();
        groupInfo.ParentGroupType = oldParentType;
        if (oldParentType != null)
          _typeToInfo[oldParentType].ChildGroupType = groupStyle.GetType();
      }
    }

		/// <summary>
		/// Removes a certain group style from the collection.
		/// </summary>
		/// <param name="groupType">Type of group style to remove from the collection.</param>
    public void RemoveType(System.Type groupType)
    {
      if (!_typeToInstance.ContainsKey(groupType))
        return;

      GroupInfo groupInfo = _typeToInfo[groupType];
      System.Type parentGroupType = groupInfo.ParentGroupType;
      System.Type childGroupType = groupInfo.ChildGroupType;

      if (parentGroupType != null) // then append my current child directly to the parent
      {
        GroupInfo parentGroupInfo = _typeToInfo[parentGroupType];
        parentGroupInfo.ChildGroupType = groupInfo.ChildGroupType;
        if (parentGroupInfo.ChildGroupType != null)
          _typeToInfo[parentGroupInfo.ChildGroupType].ParentGroupType = groupInfo.ParentGroupType;
      }
      else if (childGroupType != null) // then set the parent of the child to null
      {
        _typeToInfo[childGroupType].ParentGroupType = null;
      }

      _typeToInstance.Remove(groupType);
      _typeToInfo.Remove(groupType);


    }

		/// <summary>
		/// Removes all group styles, thus clearing the collection.
		/// </summary>
    public virtual void Clear()
    {
      _typeToInfo.Clear();
      _typeToInstance.Clear();
    }

    public void BeginPrepare()
    {
      foreach (IPlotGroupStyle pgs in this)
        pgs.BeginPrepare();
    }
    public void EndPrepare()
    {
      foreach (IPlotGroupStyle pgs in this)
        pgs.EndPrepare();
    }
    public void PrepareStep()
    {
      foreach (IPlotGroupStyle pgs in this)
        pgs.PrepareStep();
    }

    public void BeginApply()
    {
      foreach (GroupInfo info in _typeToInfo.Values)
        info.WasApplied = false;
    }

    public void EndApply()
    {
      foreach (GroupInfo info in _typeToInfo.Values)
        info.WasApplied = false;
    }

    public void OnBeforeApplication(System.Type groupStyleType)
    {
      _typeToInfo[groupStyleType].WasApplied = true;
    }

    public void Step(int step)
    {
      foreach (KeyValuePair<Type, IPlotGroupStyle> entry in _typeToInstance)
      {
        Type groupType = entry.Key;
        IPlotGroupStyle groupStyle = entry.Value;
        GroupInfo groupInfo = _typeToInfo[groupType];

        if (groupInfo.ParentGroupType == null
          && groupInfo.WasApplied
          && groupStyle.IsStepEnabled
          )
        {
          int subStep = groupStyle.Step(step);
          GroupInfo subGroupInfo = groupInfo;
          for (Type subGroupType = subGroupInfo.ChildGroupType; subGroupType != null && subStep != 0; subGroupType = subGroupInfo.ChildGroupType)
          {
            subGroupInfo = _typeToInfo[subGroupType];
            IPlotGroupStyle subGroupStyle = _typeToInstance[subGroupType];
            subStep = subGroupStyle.IsStepEnabled ? subGroupStyle.Step(subStep) : 0;
          }
        }
        groupInfo.WasApplied = false;
      }
    }

    /// <summary>
    /// Executes a prepare step only on those items, where in the own collection the stepping is enabled, but in the foreign collection it is present, but is not enabled.
    /// </summary>
    /// <param name="foreignStyles"></param>
    public void PrepareStepIfForeignSteppingFalse(PlotGroupStyleCollectionBase foreignStyles)
    {
      foreach (KeyValuePair<Type, IPlotGroupStyle> entry in _typeToInstance)
      {
        Type groupType = entry.Key;
        IPlotGroupStyle groupStyle = entry.Value;
        GroupInfo groupInfo = _typeToInfo[groupType];

        if (groupInfo.ParentGroupType == null
          && groupInfo.WasApplied
          && groupStyle.IsStepEnabled
          && foreignStyles.ContainsType(groupType)
          && !(foreignStyles.GetPlotGroupStyle(groupType).IsStepEnabled)
          )
        {
          groupStyle.PrepareStep();
        }
      }
    }


    /// <summary>
    /// Executes a step only on those items, where in the own collection the stepping is enabled, but in the foreign collection it is present, but is not enabled.
    /// </summary>
    /// <param name="step"></param>
    /// <param name="foreignStyles"></param>
    public void StepIfForeignSteppingFalse(int step, PlotGroupStyleCollectionBase foreignStyles)
    {
      foreach (KeyValuePair<Type, IPlotGroupStyle> entry in _typeToInstance)
      {
        Type groupType = entry.Key;
        IPlotGroupStyle groupStyle = entry.Value;
        GroupInfo groupInfo = _typeToInfo[groupType];

        if (groupInfo.ParentGroupType == null
          && groupInfo.WasApplied
          && groupStyle.IsStepEnabled 
          && foreignStyles.ContainsType(groupType)
          && !(foreignStyles.GetPlotGroupStyle(groupType).IsStepEnabled)
          )
        {
          int subStep = groupStyle.Step(step);
          GroupInfo subGroupInfo = groupInfo;
          for (Type subGroupType = subGroupInfo.ChildGroupType; subGroupType != null && subStep != 0; subGroupType = subGroupInfo.ChildGroupType)
          {
            subGroupInfo = _typeToInfo[subGroupType];
            IPlotGroupStyle subGroupStyle = _typeToInstance[subGroupType];
            subStep = subGroupStyle.IsStepEnabled ? subGroupStyle.Step(subStep) : 0;
          }
        }
        groupInfo.WasApplied = false;
      }
    }

    public static void TransferFromTo(PlotGroupStyleCollectionBase from, PlotGroupStyleCollectionBase tothis)
    {
      foreach (KeyValuePair<Type, IPlotGroupStyle> entry in from._typeToInstance)
      {
        Type groupType = entry.Key;
        if (!tothis.ContainsType(groupType))
          continue;

        IPlotGroupStyle fromGroupStyle = entry.Value;
        IPlotGroupStyle tothisGroupStyle = tothis.GetPlotGroupStyle(groupType);
        tothisGroupStyle.TransferFrom(fromGroupStyle);
      }
    }
    public static void TransferFromToIfBothSteppingEnabled(PlotGroupStyleCollectionBase from, PlotGroupStyleCollectionBase tothis)
    {
      foreach (KeyValuePair<Type, IPlotGroupStyle> entry in from._typeToInstance)
      {
        Type groupType = entry.Key;
        if (!tothis.ContainsType(groupType))
          continue;

        IPlotGroupStyle fromGroupStyle = entry.Value;
        if (false == fromGroupStyle.IsStepEnabled)
          continue;

        IPlotGroupStyle tothisGroupStyle = tothis.GetPlotGroupStyle(groupType);
        if (false == tothisGroupStyle.IsStepEnabled)
          continue;

        tothisGroupStyle.TransferFrom(fromGroupStyle);
      }
    }
    public void SetAllToApplied()
    {
      foreach (KeyValuePair<Type, GroupInfo> entry in _typeToInfo)
        entry.Value.WasApplied = true;
    }

    #region IEnumerable<IPlotGroup> Members

    public IEnumerator<IPlotGroupStyle> GetEnumerator()
    {
      return _typeToInstance.Values.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return _typeToInstance.Values.GetEnumerator();
    }

    #endregion


		/// <summary>
		/// Returns a list, in which the items are ordered by (i) their name, if the items are on the same level, (ii) by their parent-child relationship.
		/// </summary>
		/// <param name="comparison">Comparism method for items on the first level.</param>
		/// <returns>Ordered list of items.</returns>
		public List<IPlotGroupStyle> GetOrderedListOfItems(Comparison<IPlotGroupStyle> comparison)
		{
			var result = new List<IPlotGroupStyle>();
			// add the items, which have no parent, and sort them
			foreach (var entry in _typeToInfo)
			{
				if (entry.Value.ParentGroupType == null)
					result.Add(_typeToInstance[entry.Key]);
			}
			// now order them by whatever ordering
			result.Sort(comparison);

			// now find the other items and insert them
			// we have to use a for loop because we modify the result list
			for(int i=result.Count-1;i>=0;--i)
			{
				var info = _typeToInfo[result[i].GetType()];
				int insertPoint = i + 1;
				while(info.ChildGroupType != null)
				{
					result.Insert(insertPoint,_typeToInstance[info.ChildGroupType]);
					++insertPoint;
					info = _typeToInfo[info.ChildGroupType];
				}
			}

			System.Diagnostics.Debug.Assert(result.Count == this.Count); // hope that all items are now in the collection

			return result;
		}


  }
}
