using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Altaxo.Graph
{
	/// <summary>
	/// Summary description for AltaxoGraph.
	/// </summary>
	public class GraphForm : System.Windows.Forms.Form
	{
		private AltaxoDocument parentDocument =null;

		private Altaxo.Graph.GraphControl m_GraphControl;
		private System.Windows.Forms.ImageList m_LayerButtonImages;
		private System.ComponentModel.IContainer components;
		private ToolBar m_LayerToolbar=null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuDataPopup;
		private System.Windows.Forms.MenuItem menuDataSeparator;
		private ToolBarButton m_PushedLayerBotton=null;

		public GraphForm(System.Windows.Forms.Form parent, AltaxoDocument doc)
		: this(parent,doc,null)
		{
		}		

		public GraphForm(System.Windows.Forms.Form parent, AltaxoDocument doc, PlotAssociation[] pas)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


			parentDocument = doc;
			this.MdiParent=parent;

			InitLayerToolbar();
			this.Show();
		}


		public Altaxo.Graph.GraphControl GraphControl
		{
			get { return m_GraphControl; }
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GraphForm));
			this.m_GraphControl = new Altaxo.Graph.GraphControl();
			this.m_LayerButtonImages = new System.Windows.Forms.ImageList(this.components);
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuDataPopup = new System.Windows.Forms.MenuItem();
			this.menuDataSeparator = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// m_GraphControl
			// 
			this.m_GraphControl.ActualLayer = 0;
			this.m_GraphControl.AutoZoom = true;
			this.m_GraphControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_GraphControl.Name = "m_GraphControl";
			this.m_GraphControl.Size = new System.Drawing.Size(304, 266);
			this.m_GraphControl.SurfaceBounds = ((System.Drawing.RectangleF)(resources.GetObject("m_GraphControl.SurfaceBounds")));
			this.m_GraphControl.SurfaceMargins = ((System.Drawing.RectangleF)(resources.GetObject("m_GraphControl.SurfaceMargins")));
			this.m_GraphControl.TabIndex = 0;
			this.m_GraphControl.Zoom = 0.270784F;
			// 
			// m_LayerButtonImages
			// 
			this.m_LayerButtonImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.m_LayerButtonImages.ImageSize = new System.Drawing.Size(1, 1);
			this.m_LayerButtonImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																							this.menuDataPopup});
			// 
			// menuDataPopup
			// 
			this.menuDataPopup.Index = 0;
			this.menuDataPopup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																									this.menuDataSeparator});
			this.menuDataPopup.Text = "Data";
			this.menuDataPopup.Popup += new System.EventHandler(this.menuDataPopup_Popup);
			this.menuDataPopup.Click += new System.EventHandler(this.menuDataPopup_Click);
			this.menuDataPopup.Select += new System.EventHandler(this.menuDataPopup_Select);
			// 
			// menuDataSeparator
			// 
			this.menuDataSeparator.Index = 0;
			this.menuDataSeparator.Text = "-";
			// 
			// GraphForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 266);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																																	this.m_GraphControl});
			this.Menu = this.mainMenu1;
			this.Name = "GraphForm";
			this.Text = "AltaxoGraph";
			this.ResumeLayout(false);

		}
		#endregion

		private void m_LayerToolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			
			if(null!=this.m_PushedLayerBotton)
			{
				// if we have clicked the button already down then open the layer dialog
				if(this.m_PushedLayerBotton==e.Button)
				{
					int nLayer = System.Convert.ToInt32(e.Button.Text);
					LayerDialog dlg = new LayerDialog(m_GraphControl.Layer[nLayer],LayerDialog.Tab.Scale,EdgeType.Bottom);
					dlg.ShowDialog(this);
				}
					// if the clicked button is not already pushed, then unpush the old button
				else
				{
					this.m_PushedLayerBotton.Pushed=false;
				}
			}
				
			e.Button.Pushed = true;
			this.m_PushedLayerBotton = e.Button;
			m_GraphControl.ActualLayer = System.Convert.ToInt32(e.Button.Text);
		}

		private void InitLayerToolbar()
		{
			m_GraphControl.Dock = DockStyle.Fill;

		// 
		// m_LayerToolbar
		// 
		this.m_LayerToolbar = new ToolBar();
		this.m_LayerToolbar.Parent = this;
		this.m_LayerToolbar.ImageList = this.m_LayerButtonImages;
		this.m_LayerToolbar.AutoSize = true;
		this.m_LayerToolbar.ButtonSize = new System.Drawing.Size(24, 22);
		this.m_LayerToolbar.DropDownArrows = true;
		this.m_LayerToolbar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
		this.m_LayerToolbar.Name = "m_LayerToolbar";
		this.m_LayerToolbar.ShowToolTips = true;
		this.m_LayerToolbar.Size = new System.Drawing.Size(304, 24);
		this.m_LayerToolbar.TabIndex = 1;
		this.m_LayerToolbar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
		this.m_LayerToolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.m_LayerToolbar_ButtonClick);

		ToolBarButton tbb = new ToolBarButton("0");
		tbb.Pushed=true;
		this.m_PushedLayerBotton = tbb;

		//tbb.Style = ToolBarButtonStyle.ToggleButton;
		m_LayerToolbar.Buttons.Add(tbb);

			this.m_LayerToolbar.Dock = DockStyle.Left;

		}

		private void menuData_Data(object sender, System.EventArgs e)
		{
			DataMenuItem dmi = (DataMenuItem)sender;

			if(!dmi.Checked)
			{
				// if the menu item was not checked before, check it now
				// by making the plot association shown by the menu item
				// the actual plot association
				int actLayerNum = this.m_GraphControl.ActualLayer;
				Layer actLayer = this.m_GraphControl.Layer[actLayerNum];
				if(null!=actLayer && dmi.tagValue<actLayer.PlotAssociations.Count)
				{
					dmi.Checked=true;
					actLayer.ActualPlotAssociation = dmi.tagValue;
				}
			}
			else
			{
				// if it was checked before, then bring up the plot style dialog
				// of the plot association represented by this menu item
				int actLayerNum = this.m_GraphControl.ActualLayer;
				Layer actLayer = this.m_GraphControl.Layer[actLayerNum];
				PlotAssociation pa = actLayer.PlotAssociations[actLayer.ActualPlotAssociation];


				// get plot group
				PlotGroup plotGroup = actLayer.PlotGroups.GetPlotGroupOf(pa);
				PlotStyleDialog dlg = new PlotStyleDialog(pa.PlotStyle,plotGroup);
				DialogResult dr = dlg.ShowDialog(this);
				if(dr==DialogResult.OK)
				{
					if(null!=plotGroup)
					{
						plotGroup.Style = dlg.PlotGroupStyle;
						if(plotGroup.IsIndependent)
							pa.PlotStyle = dlg.PlotStyle;
						else
						{
							plotGroup.MasterItem.PlotStyle = dlg.PlotStyle;
							plotGroup.UpdateMembers();
						}
					}
					else // pa was not member of a plot group
					{
						pa.PlotStyle = dlg.PlotStyle;
					}

						this.m_GraphControl.Invalidate(); // renew the picture
				}
			}

		}


		private void menuDataPopup_Popup(object sender, System.EventArgs e)
		{
			int actLayerNum = this.m_GraphControl.ActualLayer;
			Layer actLayer = this.m_GraphControl.Layer[actLayerNum];
			if(null==actLayer)
				return;


			// first delete old menuitems
			// then append the plot associations of the actual layer
			menuDataPopup.MenuItems.Clear();


			menuDataPopup.MenuItems.Add(this.menuDataSeparator);
			int actPA = actLayer.ActualPlotAssociation;
			int len = actLayer.PlotAssociations.Count;
			for(int i = 0; i<len; i++)
			{
				PlotAssociation pa = actLayer.PlotAssociations[i];
				DataMenuItem mi = new DataMenuItem(pa.ToString(), new EventHandler(menuData_Data));
				mi.Checked = (i==actPA);
				mi.tagValue = i;
				menuDataPopup.MenuItems.Add(mi);
			}


		}

		private void menuDataPopup_Select(object sender, System.EventArgs e)
		{
		}

		private void menuDataPopup_Click(object sender, System.EventArgs e)
		{
		
		}

		public class DataMenuItem : MenuItem
		{
			public int tagValue=0;

			public DataMenuItem() {}
			public DataMenuItem(string t, EventHandler e) : base(t,e) {}
		}
	}
}
