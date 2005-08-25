#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2004 Dr. Dirk Lellinger
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Altaxo.Main.GUI;

namespace Altaxo.Gui.Graph
{
  /// <summary>
  /// Summary description for LineScatterPlotStyleControl.
  /// </summary>
        	[UserControlForController(typeof(IXYPlotGroupViewEventSink))]
  public class XYPlotGroupControl : System.Windows.Forms.UserControl, IXYPlotGroupView
  {
    private IXYPlotGroupViewEventSink m_Controller;
    private int m_SuppressEvents=0;

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox m_chkPlotGroupSymbol;
    private System.Windows.Forms.CheckBox m_chkPlotGroupLineType;
    private System.Windows.Forms.CheckBox m_chkPlotGroupColor;
    private System.Windows.Forms.Button m_btRemove;
    private System.Windows.Forms.Button m_btWorksheet;
            private System.Windows.Forms.RadioButton _rbtSequential;
            private System.Windows.Forms.RadioButton _rbtConcurrently;
            private System.Windows.Forms.CheckBox _chkStrict;
            private System.Windows.Forms.CheckBox _chkUpdate;
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public XYPlotGroupControl()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      // TODO: Add any initialization after the InitializeComponent call

    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if(components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

    #region Component Designer generated code
    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.m_chkPlotGroupSymbol = new System.Windows.Forms.CheckBox();
      this.m_chkPlotGroupLineType = new System.Windows.Forms.CheckBox();
      this.m_chkPlotGroupColor = new System.Windows.Forms.CheckBox();
      this._rbtSequential = new System.Windows.Forms.RadioButton();
      this._rbtConcurrently = new System.Windows.Forms.RadioButton();
      this.m_btRemove = new System.Windows.Forms.Button();
      this.m_btWorksheet = new System.Windows.Forms.Button();
      this._chkStrict = new System.Windows.Forms.CheckBox();
      this._chkUpdate = new System.Windows.Forms.CheckBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this._chkUpdate);
      this.groupBox1.Controls.Add(this._chkStrict);
      this.groupBox1.Controls.Add(this.m_chkPlotGroupSymbol);
      this.groupBox1.Controls.Add(this.m_chkPlotGroupLineType);
      this.groupBox1.Controls.Add(this.m_chkPlotGroupColor);
      this.groupBox1.Controls.Add(this._rbtSequential);
      this.groupBox1.Controls.Add(this._rbtConcurrently);
      this.groupBox1.Location = new System.Drawing.Point(8, 72);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(96, 232);
      this.groupBox1.TabIndex = 29;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Plot Group";
      // 
      // m_chkPlotGroupSymbol
      // 
      this.m_chkPlotGroupSymbol.Location = new System.Drawing.Point(8, 72);
      this.m_chkPlotGroupSymbol.Name = "m_chkPlotGroupSymbol";
      this.m_chkPlotGroupSymbol.Size = new System.Drawing.Size(72, 16);
      this.m_chkPlotGroupSymbol.TabIndex = 4;
      this.m_chkPlotGroupSymbol.Text = "Symbol";
      // 
      // m_chkPlotGroupLineType
      // 
      this.m_chkPlotGroupLineType.Location = new System.Drawing.Point(8, 48);
      this.m_chkPlotGroupLineType.Name = "m_chkPlotGroupLineType";
      this.m_chkPlotGroupLineType.Size = new System.Drawing.Size(80, 16);
      this.m_chkPlotGroupLineType.TabIndex = 3;
      this.m_chkPlotGroupLineType.Text = "Line Type";
      // 
      // m_chkPlotGroupColor
      // 
      this.m_chkPlotGroupColor.Location = new System.Drawing.Point(8, 24);
      this.m_chkPlotGroupColor.Name = "m_chkPlotGroupColor";
      this.m_chkPlotGroupColor.Size = new System.Drawing.Size(72, 16);
      this.m_chkPlotGroupColor.TabIndex = 2;
      this.m_chkPlotGroupColor.Text = "Color";
      // 
      // _rbtSequential
      // 
      this._rbtSequential.Location = new System.Drawing.Point(8, 96);
      this._rbtSequential.Name = "_rbtSequential";
      this._rbtSequential.Size = new System.Drawing.Size(88, 24);
      this._rbtSequential.TabIndex = 1;
      this._rbtSequential.Text = "Sequential";
      // 
      // _rbtConcurrently
      // 
      this._rbtConcurrently.Location = new System.Drawing.Point(8, 120);
      this._rbtConcurrently.Name = "_rbtConcurrently";
      this._rbtConcurrently.Size = new System.Drawing.Size(88, 24);
      this._rbtConcurrently.TabIndex = 0;
      this._rbtConcurrently.Text = "Concurrently";
      // 
      // m_btRemove
      // 
      this.m_btRemove.Location = new System.Drawing.Point(8, 40);
      this.m_btRemove.Name = "m_btRemove";
      this.m_btRemove.Size = new System.Drawing.Size(96, 24);
      this.m_btRemove.TabIndex = 28;
      this.m_btRemove.Text = "Remove";
      // 
      // m_btWorksheet
      // 
      this.m_btWorksheet.Location = new System.Drawing.Point(8, 8);
      this.m_btWorksheet.Name = "m_btWorksheet";
      this.m_btWorksheet.Size = new System.Drawing.Size(96, 24);
      this.m_btWorksheet.TabIndex = 27;
      this.m_btWorksheet.Text = "Worksheet";
      // 
      // _chkStrict
      // 
      this._chkStrict.Location = new System.Drawing.Point(8, 200);
      this._chkStrict.Name = "_chkStrict";
      this._chkStrict.Size = new System.Drawing.Size(72, 24);
      this._chkStrict.TabIndex = 5;
      this._chkStrict.Text = "Strict";
      // 
      // _chkUpdate
      // 
      this._chkUpdate.Location = new System.Drawing.Point(8, 176);
      this._chkUpdate.Name = "_chkUpdate";
      this._chkUpdate.Size = new System.Drawing.Size(64, 24);
      this._chkUpdate.TabIndex = 6;
      this._chkUpdate.Text = "Update";
      // 
      // XYPlotGroupControl
      // 
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.m_btRemove);
      this.Controls.Add(this.m_btWorksheet);
      this.Name = "XYPlotGroupControl";
      this.Size = new System.Drawing.Size(112, 312);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    #endregion


    public void InitComboBox(System.Windows.Forms.ComboBox box, string[] names, string name)
    {
      ++m_SuppressEvents;
      box.Items.Clear();
      box.Items.AddRange(names);
      box.SelectedItem = name;
      --m_SuppressEvents;
    }

    #region IXYPlotGroupView Members

    public IXYPlotGroupViewEventSink Controller
    {
      get { return m_Controller; }
      set { m_Controller = value; }
    }

 
    

    public void InitializePlotGroupConditions(bool bColor, bool bLineType, bool bSymbol, bool bConcurrently, bool bStrict)
    {
      this._rbtConcurrently.Checked=  bConcurrently;
      this._rbtSequential.Checked = !bConcurrently;

      this.m_chkPlotGroupColor.Checked = bColor;
      this.m_chkPlotGroupLineType.Checked = bLineType;
      this.m_chkPlotGroupSymbol.Checked = bSymbol;

      this._chkStrict.Checked = bStrict;
      this._chkUpdate.Checked = true;
    }

   

    public bool PlotGroupConcurrently
    {
      get { return this._rbtConcurrently.Checked; }
    }

    public bool PlotGroupColor
    {
      get { return m_chkPlotGroupColor.Checked; }
    }
    public bool PlotGroupLineType
    {
      get { return m_chkPlotGroupLineType.Checked; }
    }
    public bool PlotGroupSymbol
    {
      get { return m_chkPlotGroupSymbol.Checked; }
    }
            public bool PlotGroupStrict
            {
              get { return this._chkStrict.Checked; }
            }

            public bool PlotGroupUpdate
            {
              get { return this._chkUpdate.Checked; }
            }




    #endregion
  }
}
