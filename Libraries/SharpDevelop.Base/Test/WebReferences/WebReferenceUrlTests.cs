// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision: 1385 $</version>
// </file>

using ICSharpCode.SharpDevelop.Project;
using NUnit.Framework;
using System;
using System.IO;

namespace ICSharpCode.SharpDevelop.Tests.WebReferences
{
	[TestFixture]
	public class WebReferenceUrlTests
	{
		[Test]
		public void FileName()
		{
			MSBuildProject project = new MSBuildProject();
			project.FileName = "c:\\projects\\test\\foo.csproj";
			WebReferenceUrl url = new WebReferenceUrl(project);
			url.RelPath = "Web References\\localhost";
			url.Include = "http://localhost/test.asmx";
			
			Assert.AreEqual(Path.Combine(project.Directory, url.RelPath), url.FileName);
		}
		
		[Test]
		public void RelPathEndsWithSlash()
		{
			MSBuildProject project = new MSBuildProject();
			project.FileName = "c:\\projects\\test\\foo.csproj";
			WebReferenceUrl url = new WebReferenceUrl(project);
			url.RelPath = "Web References\\localhost\\";
			url.Include = "http://localhost/test.asmx";
			
			Assert.AreEqual(Path.Combine(project.Directory, url.RelPath.Trim('\\')), url.FileName);
		}
		
		[Test]
		public void ChangeFileName()
		{
			MSBuildProject project = new MSBuildProject();
			project.FileName = "c:\\projects\\test\\foo.csproj";
			WebReferenceUrl url = new WebReferenceUrl(project);
			url.RelPath = "Web References\\localhost";
			url.Include = "http://localhost/test.asmx";
			
			// Change filename - simulate a folder rename.
			url.FileName = "c:\\projects\\test\\Web References\\mywebservice";
			
			Assert.AreEqual("http://localhost/test.asmx", url.Include);
			Assert.AreEqual("Web References\\mywebservice", url.RelPath);
		}
		
		[Test]
		public void NoNamespaceSpecified()
		{
			MSBuildProject project = new MSBuildProject();
			project.FileName = "c:\\projects\\test\\foo.csproj";
			project.RootNamespace = "TestRootNamespace";
			WebReferenceUrl url = new WebReferenceUrl(project);
			
			Assert.AreEqual("TestRootNamespace", url.Namespace);
		}
		
		[Test]
		public void NamespaceSpecified()
		{
			MSBuildProject project = new MSBuildProject();
			project.FileName = "c:\\projects\\test\\foo.csproj";
			project.RootNamespace = "TestRootNamespace";
			WebReferenceUrl url = new WebReferenceUrl(project);
			url.Namespace = "WebReferenceNamespace";
			
			Assert.AreEqual("WebReferenceNamespace", url.Namespace);
		}
	}
}
