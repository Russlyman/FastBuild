﻿using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("updateserver", HelpText = "Updates FXServer development server.")]
public class UpdateServer : IOption
{
    public async Task Execute(IConfigurationRoot config)
    {
        if (Directory.Exists(Helper.Paths["fxserver"]))
        {
            Directory.Delete(Helper.Paths["fxserver"], true);
            Directory.CreateDirectory(Helper.Paths["fxserver"]);
        }

        var latestArtefactUrl = await Helper.GetLatestArtefactUrl();

        Console.WriteLine("STATUS: Downloading FXServer.");
        await Helper.DownloadFile(latestArtefactUrl, Helper.Paths["artefactArchive"]);

        Console.WriteLine("STATUS: Extracting FXServer.");
        Helper.ExtractFile(Helper.Paths["artefactArchive"], Helper.Paths["fxserver"]);

        File.Delete(Helper.Paths["artefactArchive"]);
    }
}