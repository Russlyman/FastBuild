﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
#pragma warning disable CS8509

namespace FastBuild;

internal static class Helper
{
    private static readonly string TempPath = AppDomain.CurrentDomain.BaseDirectory + "temp" + Path.DirectorySeparatorChar;
    private static readonly string ServerPath = AppDomain.CurrentDomain.BaseDirectory + "server" + Path.DirectorySeparatorChar;

    public static readonly Dictionary<string, string> Paths = new()
    {
        { "temp", TempPath },
        { "server", ServerPath },
        { "fxserverConfig", ServerPath + "server.cfg" },
        { "fxserverData", ServerPath + "server-data" + Path.DirectorySeparatorChar },
        { "resourcesLocal", ServerPath + "server-data" + Path.DirectorySeparatorChar + "resources" + Path.DirectorySeparatorChar + "[local]" + Path.DirectorySeparatorChar },
        { "dataArchive", TempPath + "cfx-server-data-master.zip" },
        { "tempFxserverData", TempPath + "cfx-server-data-master" + Path.DirectorySeparatorChar },
        { "fxserver", ServerPath + "fxserver" + Path.DirectorySeparatorChar },
        { "fxserverBinary", ServerPath + "fxserver" + Path.DirectorySeparatorChar + "FXServer.exe" },
        { "artefactArchive", TempPath + "server.7z" }
    };

    internal static async Task<string> GetLatestArtefactUrl()
    {
        using var httpClient = new HttpClient();
        var userAgent = new ProductInfoHeaderValue("FastBuild", "Russlyman");
        httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);

        var request = await httpClient.GetAsync("https://api.github.com/repos/citizenfx/fivem/tags");

        var tagsString = await request.Content.ReadAsStringAsync();
        var tagsJson = JsonConvert.DeserializeObject<dynamic>(tagsString);

        var regex = new Regex(@"v\d.\d.\d.\d\d\d\d");
        string identifier = "";

        foreach (var tag in tagsJson)
        {
            string tagName = tag.name;
            if (!regex.IsMatch(tagName)) continue;

            identifier = tagName[^4..] + "-" + tag.commit.sha;

            break;
        }

        return "https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/" + identifier + "/server.7z";
    }

    internal static async Task DownloadFile(string url, string target)
    {
        using var httpClient = new HttpClient();
        var fileBytes = await httpClient.GetByteArrayAsync(url);

        await File.WriteAllBytesAsync(target, fileBytes);
    }

    // I think this error happens because the FiveM artefact file is fat, idk though.
    // It ain't a problem if you can't see the warning =)
    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Byte[]")]
    internal static void ExtractFile(string source, string target)
    {
        if (!Directory.Exists(target))
        {
            Directory.CreateDirectory(target);
        }

        // https://stackoverflow.com/questions/41635084/performance-issues-at-extracting-7z-file-to-a-directory-c-sharp
        using var archive = ArchiveFactory.Open(source);
        using var reader = archive.ExtractAllEntries();

        while (reader.MoveToNextEntry())
        {
            if (!reader.Entry.IsDirectory)
            {
                reader.WriteEntryToDirectory(target, new ExtractionOptions()
                {
                    ExtractFullPath = true,
                    Overwrite = true
                });
            }
        }
    }

    internal static bool Choice(string message)
    {
        Console.WriteLine(message);

        string choice;

        do
        {
            Console.Write("Proceed? y/n ");
            choice = Console.ReadLine();
        }
        while (string.IsNullOrWhiteSpace(choice) || choice.ToLower() != "y" && choice.ToLower() != "n");

        return choice.ToLower() switch
        {
            "y" => true,
            "n" => false
        };
    }

    // A bit of a hack but Directory.CreateSymbolicLink doesn't work unless the program is ran as admin.
    internal static async Task CreateSymLink(string location, string target)
    {
        var symlink = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/C MKLINK /J \"{ location }\" \"{ target }\"",
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(symlink);
        await process.WaitForExitAsync();
    }
}