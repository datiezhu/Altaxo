// *****************************************************************************
// 
//  Copyright 2004, Weifen Luo
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Weifen Luo
//  and are supplied subject to licence terms.
// 
//  WinFormsUI Library Version 1.0
// *****************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI
{
	/// <summary>
	/// Summary description for HiddenMdiChild.
	/// </summary>
	internal class HiddenMdiChild : Form
	{
		private DockContent m_content = null;

		internal HiddenMdiChild(DockContent content) : base()
		{
			if (content == null)
				throw(new ArgumentNullException());

			Form mdiParent = (content.DockPanel == null) ? null : content.DockPanel.FindForm();
			if (mdiParent != null)
				if (!mdiParent.IsMdiContainer)
					mdiParent = null;

			if (mdiParent == null)
				throw(new InvalidOperationException());

			m_content = content;
			Menu = content.Menu;
			FormBorderStyle = FormBorderStyle.None;
			Text = m_content.Text;
			Size = new Size(0, 0);
			SetMdiParent(mdiParent);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((Form)m_content).Menu = Menu;
				m_content = null;
				Menu = null;
			}
			base.Dispose(disposing);
		}

		public DockContent Content
		{
			get	{	return m_content;	}
		}

		internal void SetMdiParent(Form mdiParent)
		{
			if (mdiParent == null)
				throw(new ArgumentNullException());

			MdiParent = mdiParent;
			Show();
			Size = new Size(0, 0);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == (int)Win32.Msgs.WM_MDIACTIVATE && m.HWnd == m.LParam)
				Content.Show();

			base.WndProc(ref m);
		}
	}
}
