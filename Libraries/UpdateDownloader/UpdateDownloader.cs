﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2011 Dr. Dirk Lellinger
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
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace Altaxo.Serialization.AutoUpdates
{
	/// <summary>
	/// Responsible for the download of the auto update files.
	/// </summary>
	public class Downloader
	{
		private string _storagePath;
		private string _downloadURL;
		private Version _currentVersion;
		private bool _isDownloadOfPackageCompleted;

		/// <summary>Initializes a new instance of the <see cref="Downloader"/> class.</summary>
		/// <param name="loadUnstable">If set to <c>true</c>, the <see cref="Downloader"/> take a look for the latest unstable version. If set to <c>false</c>, it
		/// looks for the latest stable version.</param>
		/// <param name="currentProgramVersion">The version of the currently installed Altaxo program.</param>
		public Downloader(bool loadUnstable, Version currentProgramVersion)
		{
			_currentVersion = currentProgramVersion;

			_storagePath = PackageInfo.GetDownloadDirectory(loadUnstable);

			if (loadUnstable)
				_downloadURL = "http://downloads.sourceforge.net/project/altaxo/Altaxo/Altaxo-Latest-Unstable/";
			else
				_downloadURL = "http://downloads.sourceforge.net/project/altaxo/Altaxo/Altaxo-Latest-Stable/";
		}

		/// <summary>Runs the <see cref="Downloader"/>.</summary>
		/// <remarks>
		/// The download is done in steps:
		/// <para>Firstly, the appropriate version file in the application data directory is locked,
		/// so that no other program can use it, until this program ends.</para>
		/// <para>Then, the version file is downloaded from the remote location.</para>
		/// <para>If there is already a valid version file in the download directory,
		/// and the version obtained from the remote version file is equal to the version obtained from the version file in the download directory,
		/// then the package was already downloaded before. Then we only check that the package file is also present and that it has the appropriate hash sum.</para>
		/// <para>Else, if the version obtained from the remote version file is higher than the program's current version,
		/// we download the package file from the remote location.</para>
		/// </remarks>
		public void Run()
		{
			if (!Directory.Exists(_storagePath))
			{
				Directory.CreateDirectory(_storagePath);
				SetDownloadDirectoryAccessRights(_storagePath);
			}

			var versionFileFullName = Path.Combine(_storagePath, PackageInfo.VersionFileName);
			using (FileStream fs = new FileStream(versionFileFullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
			{
				fs.Seek(0, SeekOrigin.Begin);
				var alreadyDownloadedVersion = PackageInfo.GetPresentDownloadedPackage(fs, _storagePath);
				fs.Seek(0, SeekOrigin.Begin);

				using (var webClient = new System.Net.WebClient())
				{
					Console.Write("Starting to download version file ...");
					var versionData = webClient.DownloadData(_downloadURL + PackageInfo.VersionFileName);
					Console.WriteLine(" ok! ({0} bytes downloaded)", versionData.Length);
					// we leave the file open, thus no other process can access it
					var parsedVersions = PackageInfo.FromStream(new MemoryStream(versionData));

					fs.Write(versionData, 0, versionData.Length);
					fs.Flush(); // write the new version to disc in order to change the write date

					// from all parsed versions, choose that one that matches the requirements
					PackageInfo parsedVersion = PackageInfo.GetHighestVersion(parsedVersions);

					if (null != parsedVersion)
					{
						Console.WriteLine("The remote package version is: {0}", parsedVersion.Version);
					}
					else
					{
						Console.WriteLine("This computer does not match the requirements of any package. The version file contains {0} packages.", parsedVersions.Length);
						return;
					}

					if (Comparer<Version>.Default.Compare(parsedVersion.Version, _currentVersion) > 0) // if the remote version is higher than the currently installed Altaxo version
					{
						Console.Write("Cleaning download directory ...");
						CleanDirectory(versionFileFullName); // Clean old downloaded files from the directory
						Console.WriteLine(" ok!");

						var packageUrl = _downloadURL + PackageInfo.GetPackageFileName(parsedVersion.Version);
						var packageFileName = Path.Combine(_storagePath, PackageInfo.GetPackageFileName(parsedVersion.Version));
						Console.WriteLine("Starting download of package file ...");
						webClient.DownloadProgressChanged += EhDownloadOfPackageFileProgressChanged;
						webClient.DownloadFileCompleted += EhDownloadOfPackageFileCompleted;
						_isDownloadOfPackageCompleted = false;
						webClient.DownloadFileAsync(new Uri(packageUrl), packageFileName);// download the package asynchronously to get progress messages
						for (; !_isDownloadOfPackageCompleted;)
						{
							System.Threading.Thread.Sleep(250);
						}
						webClient.DownloadProgressChanged -= EhDownloadOfPackageFileProgressChanged;
						webClient.DownloadFileCompleted -= EhDownloadOfPackageFileCompleted;

						Console.WriteLine("Download finished!");

						// make at least the test for the right length
						var fileInfo = new FileInfo(packageFileName);
						if (fileInfo.Length != parsedVersion.FileLength)
						{
							Console.WriteLine("Downloaded file length ({0}) differs from length in VersionInfo.txt {1}, thus the downloaded file will be deleted!", fileInfo.Length, parsedVersion.FileLength);
							fileInfo.Delete();
						}
						else
						{
							Console.WriteLine("Test file length of downloaded package file ... ok!");
						}
					}
				}
			}
		}

		/// <summary>It is neccessary to modify the download directory access rights, because as default only creator/owner has the right to change the newly created directory.
		/// But of course we want to allow all authenticated users to download auto updates. Thus we modify the access to the download directory, so that authenticated users have the right to modify files/folders. </summary>
		/// <param name="downloadDirectory">The download directory.</param>
		private void SetDownloadDirectoryAccessRights(string downloadDirectory)
		{
			try
			{
				var authenticatedUser = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
				var inheritance = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
				var propagation = PropagationFlags.None;
				var security = new DirectorySecurity();
				security.AddAccessRule(new FileSystemAccessRule(authenticatedUser, FileSystemRights.Modify, inheritance, propagation, AccessControlType.Allow));
				security.SetAccessRuleProtection(false, true);
				Directory.SetAccessControl(downloadDirectory, security);
			}
			catch (Exception)
			{
			}
		}

		/// <summary>Called when the download of the package file is completed.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.ComponentModel.AsyncCompletedEventArgs"/> instance containing the event data.</param>
		private void EhDownloadOfPackageFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			_isDownloadOfPackageCompleted = true;
		}

		/// <summary>Outputs the download progress to the console.</summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Net.DownloadProgressChangedEventArgs"/> instance containing the event data.</param>
		private void EhDownloadOfPackageFileProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
		{
			Console.Write("{0}%\r", e.ProgressPercentage);
		}

		/// <summary>Cleans the download directory from all package files.</summary>
		/// <param name="withExceptionOfThisFile">A file name, which should not be removed.</param>
		private void CleanDirectory(string withExceptionOfThisFile)
		{
			try
			{
				var files = Directory.GetFiles(_storagePath, "Altaxo*.zip");
				foreach (var fileName in files)
				{
					if (0 == string.Compare(fileName, withExceptionOfThisFile, true))
						continue;
					File.Delete(fileName);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
