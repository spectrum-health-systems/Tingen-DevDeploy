// ================================================================ v1.0.0 =====
// TingenDeploy: A command-line deployment utility for Tingen.
// https://github.com/spectrum-health-systems/AbatabLieutenant
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240525 =====

// b240525.0837

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.Clear();

        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        VerifyFramework(timestamp);

    }

    private static void VerifyFramework(string timestamp)
    {
        VerifyRequiredDataDirectories(timestamp);
    }

    private static void VerifyRequiredDataDirectories(string timestamp)
    {
        var dataDirectories = GetListOfRequiredDataDirectories();

        foreach (var dataDirectory in dataDirectories)
        {
            if (!Directory.Exists(dataDirectory))
            {
                StatusUpdate($"Creating directory: {dataDirectory}...{Environment.NewLine}", timestamp);
                Directory.CreateDirectory(dataDirectory);
            }
        }
    }

    private static List<string> GetListOfRequiredDataDirectories()
    {
        return new List<string>
        {
            @"C:\TingenData",
            @"C:\TingenData\Archive",
            @"C:\TingenData\Commander",
            @"C:\TingenData\Commander\Logs",
            @"C:\TingenData\Commander\Staging",
            @"C:\TingenData\Commander\Temporary",
            @"C:\TingenData\Deploy",
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
        };
    }

    private static void StatusUpdate(string message, string timestamp)
    {
        Console.WriteLine(message);
        File.AppendAllText($@"C:\TingenData\Lieutenant\Logs\{timestamp}.deploy", message);
    }
}