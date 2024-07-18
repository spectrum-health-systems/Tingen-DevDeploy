// ================================================================ v1.3.1 =====
// Tingen-DevDeploy: A command-line deployment utility for Tingen-Development.
// https://github.com/spectrum-health-systems/AbatabLieutenant
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240718 =====

// u240718.1052_code
// u240613.1209_documentation

/* Please see the Tingen-DevDeploy README for more information.
 */

using System.IO.Compression;
using System.Net;

namespace TingenDevDeploy
{
    /// <summary>The main entry point for the application.</summary>
    internal static class Program
    {
        private const string LogRoot                    = @"C:\TingenData\DevDeploy\Logs";
        private const string StagingRoot                = @"C:\TingenData\DevDeploy\Staging";
        private const string ZipUrl                     = "https://github.com/spectrum-health-systems/Tingen-Development/archive/refs/heads/development.zip";
        private const string ZipDownloadPath            = @"C:\TingenData\DevDeploy\Staging\Tingen-Development.zip";
        private const string TingenUatServiceRoot       = @"C:\Tingen\UAT";
        private const string TingenUatServiceRoslynPath = @"C:\Tingen\UAT\bin\roslyn";
        private const string TingenStagingBinPath       = @"C:\TingenData\DevDeploy\Staging\Tingen-Development-development\src\bin";
        private const string TingenUatServiceBinPath    = @"C:\Tingen\UAT\bin";
        private const string TingenStagingServiceRoot   = @"C:\TingenData\DevDeploy\Staging\Tingen-Development-development\src";

        /// <summary>Starting point.</summary>
        /// <param name="args">The passed arguments.</param>
        static void Main(string[] args)
        {
            Console.Clear();

            var dateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            VerifyLogPath();
            VerifyDataPaths(dateTimeStamp);
            RefreshStaging(dateTimeStamp);
            RefreshServiceDirectory(dateTimeStamp);
            DownloadRepoZip(dateTimeStamp);
            ExtractRepoZip(dateTimeStamp);
            CopyBinFiles(dateTimeStamp);
            CopyServiceFiles(dateTimeStamp);
        }

        /// <summary>Verify the log directory exists.</summary>
        /// <remarks>
        ///  <para>
        ///   - Verify the log directory exists, since logs are written before anything else is setup.
        ///  </para>
        /// </remarks>
        private static void VerifyLogPath()
        {
            if (!Directory.Exists(LogRoot))
            {
                Directory.CreateDirectory(LogRoot);
            }
        }

        /// <summary>Verify the required Tingen-DevDeploy data directories exist.</summary>
        /// <param name="dateTimeStamp">The date/time when Tingen-DevDeploy was executed.</param>
        private static void VerifyDataPaths(string dateTimeStamp)
        {
            foreach (var dataPath in from dataDirectory in GetListOfDataDirectories()
                                     where !Directory.Exists(dataDirectory)
                                     select dataDirectory)
            {
                StatusUpdate($"Creating directory: {dataPath}...", dateTimeStamp);
                Directory.CreateDirectory(dataPath);
            }
        }

        /// <summary>Refresh the Tingen-DevDeploy staging environment.</summary>
        /// <param name="dateTimeStamp">The date/time when Tingen-DevDeploy was executed.</param>
        private static void RefreshStaging(string dateTimeStamp)
        {
            if (Directory.Exists(StagingRoot))
            {
                StatusUpdate("Refreshing staging environment...", dateTimeStamp);
                Directory.Delete(StagingRoot, true);
                Directory.CreateDirectory(StagingRoot);
            }
        }

        /// <summary>Download the development branch of the Tingen-Development repository.</summary>
        /// <param name="dateTimeStamp">The date/time when Tingen-DevDeploy was executed.</param>
        private static void DownloadRepoZip(string dateTimeStamp)
        {
            StatusUpdate("Downloading the Tingen-Development repository...", dateTimeStamp);
            var client = new WebClient();
            client.DownloadFile(ZipUrl, ZipDownloadPath);
        }

        /// <summary> Extract the Tingen-Development repository zip file. </summary>
        /// <param name="dateTimeStamp">The date/time when Tingen-DevDeploy was executed.</param>
        private static void ExtractRepoZip(string dateTimeStamp)
        {
            StatusUpdate("Extracting the Tingen-Development zip file...", dateTimeStamp);
            ZipFile.ExtractToDirectory(ZipDownloadPath, StagingRoot);
        }

        /// <summary>Refresh the UAT Tingen web service.</summary>
        /// <remarks>
        ///  <para>
        ///   - When the TingenUatRoslynPath is created, it will also create the:
        ///    - TingenUatServicePath
        ///    - TingenUatServicePath\bin
        ///  </para>
        /// </remarks>
        /// <param name="dateTimeStamp">The date/time when Tingen-DevDeploy was executed.</param>
        private static void RefreshServiceDirectory(string dateTimeStamp)
        {
            if (Directory.Exists(TingenUatServiceRoot))
            {
                StatusUpdate("Refreshing the UAT Tingen web service directory...", dateTimeStamp);
                Directory.Delete(TingenUatServiceRoot, true);
                Directory.CreateDirectory(TingenUatServiceRoslynPath);
            }
            else
            {
                Directory.CreateDirectory(TingenUatServiceRoslynPath);
            }
        }

        /// <summary>Copy the Tingen-Development bin files to the UAT Tingen web service directory.</summary>
        /// <param name="dateTimeStamp">The date/time when Tingen-DevDeploy was executed.</param>
        private static void CopyBinFiles(string dateTimeStamp)
        {
            StatusUpdate("Copying Tingen-Development files to the UAT Tingen web service directory...", dateTimeStamp);

            CopyDirectory(TingenStagingBinPath, TingenUatServiceBinPath, dateTimeStamp);
        }

        /// <summary>Copy the Tingen-Development service files to the UAT Tingen web service directory.</summary>
        /// <param name="dateTimeStamp">The date/time when Tingen-DevDeploy was executed.</param>
        private static void CopyServiceFiles(string dateTimeStamp)
        {
            foreach (string serviceFile in GetServiceFiles())
            {
                StatusUpdate($"Copying {serviceFile}...", dateTimeStamp);
                File.Copy($@"{TingenStagingServiceRoot}\{serviceFile}", $@"{TingenUatServiceRoot}\{serviceFile}");
            }
        }

        /// <summary>Copy the source directory to the target directory.</summary>
        /// <param name="sourcePath">The source path to copy from.</param>
        /// <param name="targetPath">The target path to copy to.</param>
        /// <param name="dateTimeStamp">The date/time when Tingen-DevDeploy was executed.</param>
        private static void CopyDirectory(string sourcePath, string targetPath, string dateTimeStamp)
        {
            DirectoryInfo dirToCopy = new DirectoryInfo(sourcePath);
            DirectoryInfo[] subDirsToCopy = GetSubDirs(sourcePath); // redundant

            foreach (FileInfo file in dirToCopy.GetFiles())
            {
                StatusUpdate($"Copying {file.FullName}...", dateTimeStamp);
                _=file.CopyTo(Path.Combine(targetPath, file.Name));
            }

            foreach (var (subDir, newTargetDir) in from DirectoryInfo subDir in subDirsToCopy
                                                   let newTargetDir = Path.Combine(targetPath, subDir.Name)
                                                   select (subDir, newTargetDir))
            {
                StatusUpdate($"Copying {subDir}...", dateTimeStamp);
                CopyDirectory(subDir.FullName, newTargetDir, dateTimeStamp);
            }
        }

        /// <summary>Get the subdirectories of the source directory.</summary>
        /// <param name="sourcePath">The path to get the subdirectories of.</param>
        /// <returns>The subdirectories of the sourcePath.</returns>
        private static DirectoryInfo[] GetSubDirs(string sourcePath)
        {
            DirectoryInfo dirToCopy = new DirectoryInfo(sourcePath);

            return dirToCopy.GetDirectories();
        }

        /// <summary>Write a status update to the console and log file.</summary>
        /// <param name="message">The status update to display/write.</param>
        /// <param name="dateTimeStamp">The date/time when Tingen-DevDeploy was executed.</param>
        private static void StatusUpdate(string message, string dateTimeStamp)
        {
            Console.WriteLine(message);
            File.AppendAllText($@"{LogRoot}\{dateTimeStamp}.devdeploy", $"{message}{Environment.NewLine}");
        }

        /// <summary>Creates a list of the required Tingen-DevDeploy directories.</summary>
        /// <remarks>
        ///  <para>
        ///   - There are actually four directories that are required for Tingen-DevDeploy, but the other two are
        ///   created when these two are.
        ///  </para>
        /// </remarks>
        /// <returns></returns>
        private static List<string> GetListOfDataDirectories() =>
        [
            LogRoot,
            StagingRoot
        ];

        /// <summary>Get the list of service files to copy to the UAT Tingen web service directory.</summary>
        /// <returns>A list of the required Tingen service files.</returns>
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