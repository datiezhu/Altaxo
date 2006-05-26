// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision: 955 $</version>
// </file>

using SD = ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Project;
using NUnit.Framework;
using System;
using System.Web.Services.Description;
using System.Web.Services.Discovery;

namespace ICSharpCode.SharpDevelop.Tests.WebReferences
{
	/// <summary>
	/// Tests the WebReferenceChanges class.
	/// </summary>
	[TestFixture]
	public class WebReferenceChangesTest
	{
		SD.WebReferenceChanges changes;
		
		[TestFixtureSetUp]
		public void SetUpFixture()
		{
			// Set up the project.
			MSBuildProject project = new MSBuildProject();
			project.Language = "C#";
			project.FileName = "c:\\projects\\test\\foo.csproj";
			
			// Web references item.
			WebReferencesProjectItem webReferencesItem = new WebReferencesProjectItem(project);
			webReferencesItem.Include = "Web References\\";
			project.Items.Add(webReferencesItem);
			
			// Web reference url.
			WebReferenceUrl webReferenceUrl = new WebReferenceUrl(project);
			webReferenceUrl.Include = "http://localhost/test.asmx";
			webReferenceUrl.UpdateFromURL = "http://localhost/test.asmx";
			webReferenceUrl.RelPath = "Web References\\localhost";
			project.Items.Add(webReferenceUrl);
			
			FileProjectItem discoFileItem = new FileProjectItem(project, ItemType.None);
			discoFileItem.Include = "Web References\\localhost\\test.disco";
			project.Items.Add(discoFileItem);

			FileProjectItem wsdlFileItem = new FileProjectItem(project, ItemType.None);
			wsdlFileItem.Include = "Web References\\localhost\\test.wsdl";
			project.Items.Add(wsdlFileItem);
			
			// Proxy
			FileProjectItem proxyItem = new FileProjectItem(project, ItemType.Compile);
			proxyItem.Include = "Web References\\localhost\\Reference.cs";
			proxyItem.DependentUpon = "Reference.map";
			project.Items.Add(proxyItem);
			
			// Reference map.
			FileProjectItem mapItem = new FileProjectItem(project, ItemType.None);
			mapItem.Include = "Web References\\localhost\\Reference.map";
			project.Items.Add(mapItem);
			
			// System.Web.Services reference.
			ReferenceProjectItem webServicesReferenceItem = new ReferenceProjectItem(project, "System.Web.Services");
			project.Items.Add(webServicesReferenceItem);
			
			// Set up the web reference.
			DiscoveryClientProtocol	protocol = new DiscoveryClientProtocol();
			DiscoveryDocumentReference discoveryRef = new DiscoveryDocumentReference();
			discoveryRef.Url = "http://localhost/new.asmx";
			protocol.References.Add(discoveryRef);
			
			ContractReference contractRef = new ContractReference();
			contractRef.Url = "http://localhost/new.asmx?wsdl";
			contractRef.ClientProtocol = new DiscoveryClientProtocol();
			ServiceDescription desc = new ServiceDescription();
			contractRef.ClientProtocol.Documents.Add(contractRef.Url, desc);
			protocol.References.Add(contractRef);
			
			SD.WebReference webReference = new SD.WebReference(project, "http://localhost/new.asmx", "localhost", "ProxyNamespace", protocol);
			changes = webReference.GetChanges(project);
		}
				
		[Test]
		public void HasChanged()
		{
			Assert.IsTrue(changes.Changed);
		}
		
		[Test]
		public void HasNewWsdlFile()
		{
			Assert.IsNotNull(WebReferenceTestHelper.GetFileProjectItem(changes.NewItems, "Web References\\localhost\\new.wsdl", ItemType.None));
		}
		
		[Test]
		public void HasNewDiscoFile()
		{
			Assert.IsNotNull(WebReferenceTestHelper.GetFileProjectItem(changes.NewItems, "Web References\\localhost\\new.disco", ItemType.None));
		}
		
		[Test]
		public void OldWsdlFileRemoved()
		{
			Assert.IsNotNull(WebReferenceTestHelper.GetFileProjectItem(changes.ItemsRemoved, "Web References\\localhost\\test.wsdl", ItemType.None));
		}
		
		[Test]
		public void OldDiscoFileRemoved()
		{
			Assert.IsNotNull(WebReferenceTestHelper.GetFileProjectItem(changes.ItemsRemoved, "Web References\\localhost\\test.disco", ItemType.None));
		}
		
		[Test]
		public void WebReferenceUrlNotConsideredNewItem()
		{
			Assert.IsNull(WebReferenceTestHelper.GetProjectItem(changes.NewItems, ItemType.WebReferenceUrl));
		}
		
		[Test]
		public void WebReferenceUrlNotConsideredRemoved()
		{
			Assert.IsNull(WebReferenceTestHelper.GetProjectItem(changes.ItemsRemoved, ItemType.WebReferenceUrl));
		}
	}
}