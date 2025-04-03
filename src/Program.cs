/*
* ████████╗██╗███╗   ██╗ ██████╗ ███████╗███╗   ██╗
* ╚══██╔══╝██║████╗  ██║██╔════╝ ██╔════╝████╗  ██║
*    ██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██╔██╗ ██║
*    ██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██║╚██╗██║
*    ██║   ██║██║ ╚████║╚██████╔╝███████╗██║ ╚████║
*    ╚═╝   ╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚══════╝╚═╝  ╚═══╝
*
*
* ██████╗ ███████╗██╗   ██╗██████╗ ███████╗██████╗ ██╗      ██████╗ ██╗   ██╗
* ██╔══██╗██╔════╝██║   ██║██╔══██╗██╔════╝██╔══██╗██║     ██╔═══██╗╚██╗ ██╔╝
* ██║  ██║█████╗  ██║   ██║██║  ██║█████╗  ██████╔╝██║     ██║   ██║ ╚████╔╝ 
* ██║  ██║██╔══╝  ╚██╗ ██╔╝██║  ██║██╔══╝  ██╔═══╝ ██║     ██║   ██║  ╚██╔╝  
* ██████╔╝███████╗ ╚████╔╝ ██████╔╝███████╗██║     ███████╗╚██████╔╝   ██║   
* ╚═════╝ ╚══════╝  ╚═══╝  ╚═════╝ ╚══════╝╚═╝     ╚══════╝ ╚═════╝    ╚═╝   
                                                                           

* https://github.com/APrettyCoolProgram/Tingen-DevDeploy
* Copyright (c) A Pretty Cool Program. All rights reserved.
* Licensed under the Apache 2.0 license.
*
* Release 1.4
*/

// u250403_code
// u250403_documentation

/* Please see the Tingen-DevDeploy README.md for more information.
 * https://github.com/spectrum-health-systems/Tingen-DevDeploy
 */

using System.IO.Compression;
using System.Net;

namespace TingenDevDeploy
{
    /// <summary>The main entry point for the application.</summary>
    internal static class Program
    {
        private const string LogRoot                            = @"C:\Tingen_Data\DevDeploy\Logs";
        private const string StagingRoot                        = @"C:\Tingen_Data\DevDeploy\Staging";
        private const string ZipUrl                             = "https://github.com/spectrum-health-systems/Tingen-WebService/archive/refs/heads/development.zip";
        private const string ZipDownloadPath                    = @"C:\Tingen_Data\DevDeploy\Staging\Tingen-WebService.zip";
        private const string TingenUatServiceRoot               = @"C:\Tingen\UAT";
        private const string TingenUatServiceRoslynPath         = @"C:\Tingen\UAT\bin\roslyn";
        private const string TingenUatServiceAppDataPath        = @"C:\Tingen\UAT\bin\AppData";
        private const string TingenUatServiceAppDataRuntimePath = @"C:\Tingen\UAT\bin\AppData\Runtime";
        private const string TingenStagingBinPath               = @"C:\Tingen_Data\DevDeploy\Staging\Tingen-WebService-development\src\bin";
        private const string TingenUatServiceBinPath            = @"C:\Tingen\UAT\bin";
        private const string TingenStagingServiceRoot           = @"C:\Tingen_Data\DevDeploy\Staging\Tingen-WebService-development\src";

        /// <summary>Starting point.</summary>
        /// <param name="args">The passed arguments.</param>
        static void Main(string[] args)
        {
            Console.Clear();

            var dateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            VerifyLogPath();
            Start(dateTimeStamp);
            VerifyDataPaths(dateTimeStamp);
            RefreshStaging(dateTimeStamp);
            RefreshServiceDirectory(dateTimeStamp);
            DownloadRepoZip(dateTimeStamp);
            ExtractRepoZip(dateTimeStamp);
            CopyBinFiles(dateTimeStamp);
            CopyServiceFiles(dateTimeStamp);
        }


        private static void Start(string dateTimeStamp)
        {
            var logHeader = $"Tingen DevDeploy v 1.4{Environment.NewLine}" +
                            $"======================{Environment.NewLine}" +
                            Environment.NewLine;

            StatusUpdate(logHeader, dateTimeStamp);
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
            StatusUpdate("Downloading the Tingen-WebService development branch...", dateTimeStamp);
            var client = new WebClient();
            client.DownloadFile(ZipUrl, ZipDownloadPath);
        }

        /// <summary> Extract the Tingen-Development repository zip file. </summary>
        /// <param name="dateTimeStamp">The date/time when Tingen-DevDeploy was executed.</param>
        private static void ExtractRepoZip(string dateTimeStamp)
        {
            StatusUpdate("Extracting the Tingen-WebService development branch...", dateTimeStamp);
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
                StatusUpdate("Refreshing the UAT Tingen Web Service directory...", dateTimeStamp);
                Directory.Delete(TingenUatServiceRoot, true);
                Directory.CreateDirectory(TingenUatServiceRoslynPath);
                Directory.CreateDirectory(TingenUatServiceAppDataPath);
                Directory.CreateDirectory(TingenUatServiceAppDataRuntimePath);
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
            StatusUpdate("Copying Tingen Web Service files to the UAT Tingen Web Service directory...", dateTimeStamp);

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
                "TingenWebService.asmx",
                "TingenWebService.asmx.cs",
                "packages.config",
                "Web.config",
                "Web.Debug.config",
                "Web.Release.config"
            ];
        }
    }
}