// ================================================================ v1.3.0 =====
// Tingen-DevDeploy: A command-line deployment utility for Tingen-Development.
// https://github.com/spectrum-health-systems/AbatabLieutenant
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240613 =====

// b240613.1036

/* This is a simple command-line application that deploys Tingen-Development.
 * Most of the settings/variables are hardcoded, and are specific to the
 * Tingen-Development repository.
 *
 * Eventually this will be superceded by Tingen-Lieutenant and Tingen-Commander.
 */

using System.IO.Compression;
using System.Net;

namespace TingenDevDeploy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            VerifyLogDirectory();
            VerifyFramework(timestamp);
            RefreshStagingEnvironment(timestamp);
            RefreshServiceDirectory(timestamp);
            DownloadRepoZip(timestamp);
            ExtractRepoZip(timestamp);
            CopyBinFiles(timestamp);
            CopyServiceFiles(timestamp);
        }

        private static void VerifyFramework(string timestamp) => VerifyDataDirectories(timestamp);

        private static void VerifyDataDirectories(string timestamp)
        {
            foreach (var dataDirectory in from dataDirectory in GetListOfDataDirectories()
                                          where !Directory.Exists(dataDirectory)
                                          select dataDirectory)
            {
                StatusUpdate($"Creating directory: {dataDirectory}...", timestamp);
                Directory.CreateDirectory(dataDirectory);
            }
        }

        private static void VerifyLogDirectory()
        {
            const string logDirectory = @"C:\TingenData\DevDeploy\Logs";

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        private static void RefreshStagingEnvironment(string timestamp)
        {
            const string stagingDirectory = @"C:\TingenData\DevDeploy\Staging";

            if (Directory.Exists(stagingDirectory))
            {
                StatusUpdate("Refreshing staging environment...", timestamp);
                Directory.Delete(stagingDirectory, true);
                Directory.CreateDirectory(stagingDirectory);
            }
        }

        private static void DownloadRepoZip(string timestamp)
        {
            const string url = "https://github.com/spectrum-health-systems/Tingen-Development/archive/refs/heads/development.zip";
            const string target = @"C:\TingenData\DevDeploy\Staging\Tingen-Development.zip";

            StatusUpdate("Downloading repository zip...", timestamp);
            var client = new WebClient();
            client.DownloadFile(url, target);
        }

        private static void ExtractRepoZip(string timestamp)
        {
            const string source = @"C:\TingenData\DevDeploy\Staging\Tingen-Development.zip";
            const string target = @"C:\TingenData\DevDeploy\Staging";

            StatusUpdate("Extracting repository zip...", timestamp);
            ZipFile.ExtractToDirectory(source, target);
        }

        private static void RefreshServiceDirectory(string timestamp)
        {
            const string serviceDirectory = @"C:\Tingen\UAT";
            const string roslynDirectory = @"C:\Tingen\UAT\bin\roslyn";

            if (Directory.Exists(@"C:\Tingen\UAT"))
            {
                StatusUpdate("Refreshing web service directory...", timestamp);
                Directory.Delete(serviceDirectory, true);
                Directory.CreateDirectory(roslynDirectory);
            }
            else
            {
                Directory.CreateDirectory(roslynDirectory);
            }
        }

        private static void CopyBinFiles(string timestamp)
        {
            const string source = @"C:\TingenData\DevDeploy\Staging\Tingen-Development-development\src\bin";
            const string target = @"C:\Tingen\UAT\bin";

            StatusUpdate("Copying repository files...", timestamp);

            CopyDirectory(source, target, timestamp);
        }

        private static void CopyServiceFiles(string timestamp)
        {
            const string source = @"C:\TingenData\DevDeploy\Staging\Tingen-Development-development\src";
            const string target = @"C:\Tingen\UAT";

            foreach (string file in GetServiceFiles())
            {
                StatusUpdate($"Copying {file}...", timestamp);
                File.Copy($@"{source}\{file}", $@"{target}\{file}");
            }
        }

        private static void CopyDirectory(string source, string target, string timestamp)
        {
            DirectoryInfo dirToCopy = new DirectoryInfo(source);
            DirectoryInfo[] subDirsToCopy = GetSubDirs(source, target);

            foreach (FileInfo file in dirToCopy.GetFiles())
            {
                StatusUpdate($"Copying {file.FullName}...", timestamp);
                _=file.CopyTo(Path.Combine(target, file.Name));
            }

            foreach (var (subDir, newTargetDir) in from DirectoryInfo subDir in subDirsToCopy
                                                   let newTargetDir = Path.Combine(target, subDir.Name)
                                                   select (subDir, newTargetDir))
            {
                StatusUpdate($"Copying {subDir}...", timestamp);
                CopyDirectory(subDir.FullName, newTargetDir, timestamp);
            }

        }

        private static DirectoryInfo[] GetSubDirs(string source, string target)
        {
            DirectoryInfo dirToCopy = new DirectoryInfo(source);

            return dirToCopy.GetDirectories();
        }

        private static void StatusUpdate(string message, string timestamp)
        {
            Console.WriteLine(message);
            File.AppendAllText($@"C:\TingenData\DevDeploy\Logs\{timestamp}.devdeploy", message);
        }

        private static List<string> GetListOfDataDirectories() =>
        [
            @"C:\TingenData",
            @"C:\TingenData\DevDeploy",
            @"C:\TingenData\DevDeploy\Logs",
            @"C:\TingenData\DevDeploy\Staging",
            @"C:\TingenData\DevDeploy\Temporary",
        ];

        private static List<string> GetServiceFiles()
        {
            return
            [
                "Tingen_development.asmx",
                "Tingen_development.asmx.cs",
                "packages.config",
                "Web.config",
                "Web.Debug.config",
                "Web.Release.config"
            ];
        }
    }
}