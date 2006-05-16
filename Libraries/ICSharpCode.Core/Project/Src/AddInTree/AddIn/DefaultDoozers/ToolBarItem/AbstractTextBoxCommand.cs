// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="John Simons" email="johnsimons007@yahoo.com.au"/>
//     <version>$Revision: 1 $</version>
// </file>

using System;
using System.Collections;
using System.CodeDom.Compiler;
using System.Windows.Forms;

namespace ICSharpCode.Core
{
	public abstract class AbstractTextBoxCommand : AbstractCommand, ITextBoxCommand
	{
		bool isEnabled = true;
		
		public virtual bool IsEnabled {
			get {
				return isEnabled;
			}
			set {
				isEnabled = value;
			}
		}
		
		public override void Run()
		{
			
		}
	}
}
