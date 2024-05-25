// ================================================================ v1.0.0 =====
// TingenDeploy: A command-line deployment utility for Tingen.
// https://github.com/spectrum-health-systems/AbatabLieutenant
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240525 =====

// b240525.0837

using System.IO.Compression;
using System.Net;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.Clear();

        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        VerifyFramework(timestamp);

        RefreshStagingEnvironment(timestamp);

        RefreshServiceDirectory(timestamp);

        DownloadRepoZip(timestamp);

        ExtractRepoZip(timestamp);

        CopyBinFiles(timestamp);


    }

    /// <summary>Verify Tingen framework</summary>
    /// <param name="timestamp">Timesamp for logging purposes.</param>
    private static void VerifyFramework(string timestamp)
    {
        VerifyDataDirectories(timestamp);
    }

    /// <summary>Verify framework directories</summary>
    /// <param name="timestamp">Timesamp for logging purposes.</param>
    private static void VerifyDataDirectories(string timestamp)
    {
        VerifyLogDirectory();

        foreach (var dataDirectory in GetListOfDataDirectories())
        {
            if (!Directory.Exists(dataDirectory))
            {
                StatusUpdate($"Creating directory: {dataDirectory}...", timestamp);
                Directory.CreateDirectory(dataDirectory);
            }
        }
    }

    private static void VerifyLogDirectory()
    {
        if (!Directory.Exists(@"C:\TingenData\Lieutenant\Logs"))
        {
            Directory.CreateDirectory(@"C:\TingenData\Lieutenant\Logs");
        }
    }

    /// <summary>Get the list of required data directories.</summary>
    /// <returns></returns>
    private static List<string> GetListOfDataDirectories()
    {
        return
        [
            @"C:\TingenData",
            @"C:\TingenData\Archive",
            @"C:\TingenData\Commander",
            @"C:\TingenData\Commander\Logs",
            @"C:\TingenData\Commander\Staging",
            @"C:\TingenData\Commander\Temporary",
            @"C:\TingenData\Lieutenant",
            @"C:\TingenData\Lieutenant\Logs",
            @"C:\TingenData\Lieutenant\Staging",
            @"C:\TingenData\Lieutenant\Temporary",
            @"C:\TingenData\LIVE",
            @"C:\TingenData\LIVE\Archive",
            @"C:\TingenData\LIVE\Configs",
            @"C:\TingenData\LIVE\Data",
            @"C:\TingenData\LIVE\Data\Export",
            @"C:\TingenData\LIVE\Data\Import",
            @"C:\TingenData\LIVE\Debug",
            @"C:\TingenData\LIVE\Logs",
            @"C:\TingenData\LIVE\Messages",
            @"C:\TingenData\LIVE\Messages\Alerts",
            @"C:\TingenData\LIVE\Messages\Errors",
            @"C:\TingenData\LIVE\Messages\Warnings",
            @"C:\TingenData\LIVE\Reports",
            @"C:\TingenData\LIVE\Templates",
            @"C:\TingenData\LIVE\Temporary",
            @"C:\TingenData\Primeval",
            @"C:\TingenData\Public",
            @"C:\TingenData\Public\Messages",
            @"C:\TingenData\Public\Messages\Alerts",
            @"C:\TingenData\Public\Messages\Warnings",
            @"C:\TingenData\Public\Reports",
            @"C:\TingenData\Remote",
            @"C:\TingenData\Remote\Alerts",
            @"C:\TingenData\Remote\Errors",
            @"C:\TingenData\Remote\Reports",
            @"C:\TingenData\Remote\Warnings",
            @"C:\TingenData\UAT",
            @"C:\TingenData\UAT\Archive",
            @"C:\TingenData\UAT\Configs",
            @"C:\TingenData\UAT\Data",
            @"C:\TingenData\UAT\Data\Export",
            @"C:\TingenData\UAT\Data\Import",
            @"C:\TingenData\UAT\Debug",
            @"C:\TingenData\UAT\Logs",
            @"C:\TingenData\UAT\Messages",
            @"C:\TingenData\UAT\Messages\Alerts",
            @"C:\TingenData\UAT\Messages\Errors",
            @"C:\TingenData\UAT\Messages\Warnings",
            @"C:\TingenData\UAT\Reports",
            @"C:\TingenData\UAT\Templates",
            @"C:\TingenData\UAT\Temporary",
        ];
    }

    private static void RefreshStagingEnvironment(string timestamp)
    {
        if (Directory.Exists(@"C:\TingenData\Lieutenant\Staging"))
        {
            StatusUpdate("Refreshing staging environment...", timestamp);
            Directory.Delete(@"C:\TingenData\Lieutenant\Staging", true);
            Directory.CreateDirectory(@"C:\TingenData\Lieutenant\Staging");
        }
    }

    private static void DownloadRepoZip(string timestamp)
    {
        StatusUpdate("Downloading repository zip...", timestamp);
        var client = new WebClient();
        client.DownloadFile("https://github.com/spectrum-health-systems/Tingen_development/archive/refs/heads/staging.zip", @"C:\TingenData\Lieutenant\Staging");
    }

    private static void ExtractRepoZip(string timestamp)
    {
        StatusUpdate("Extracting repository zip...", timestamp);
        ZipFile.ExtractToDirectory(@"C:\TingenData\Lieutenant\Staging\Tingen_development.zip", @"C:\TingenData\Lieutenant\Staging\Tingen_development");
    }

    private static void RefreshServiceDirectory(string timestamp)
    {
        if (Directory.Exists(@"C:\Tingen\UAT"))
        {
            StatusUpdate("Refreshing web service directory...", timestamp);
            Directory.Delete(@"C:\Tingen\UAT", true);
            Directory.CreateDirectory(@"C:\Tingen\UAT");
        }
    }

    private static void CopyBinFiles(string timestamp)
    {
        StatusUpdate("Copying repository files...", timestamp);
        var sourceDirectory = @"C:\TingenData\Lieutenant\Staging\Tingen_development\src\bin";
        var destinationDirectory = @"C:\Tingen\UAT\bin";
        CopyDirectory(sourceDirectory, destinationDirectory, timestamp);
    }

    private static void CopyServiceFiles(string source, string target, string timestamp)
    {
        foreach (string file in GetServiceFiles())
        {
            StatusUpdate($"Copying {file}...", timestamp);
            File.Copy($@"{source}\{file}", $@"{target}\{file}");
        }
    }

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


    private static void CopyDirectory(string source, string target, string timestamp)
    {
        DirectoryInfo dirToCopy       = new DirectoryInfo(source);
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
        File.AppendAllText($@"C:\TingenData\Lieutenant\Logs\{timestamp}.deploy", message);
    }
}