using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("updatedata", HelpText = "Updates FXServer server data folder.")]
public class UpdateData : IOption
{
    public async Task Execute(IConfigurationRoot config)
    {
        if (Directory.Exists(Helper.Paths["fxserverData"]))
        {
            Helper.DeleteAllSymLinks(Helper.Paths["fxserverData"]);
            Directory.Delete(Helper.Paths["fxserverData"], true);
        }

        if (!Directory.Exists(Helper.Paths["server"]))
        {
            Directory.CreateDirectory(Helper.Paths["server"]);
        }

        Console.WriteLine("STATUS: Downloading server data.");
        await Helper.DownloadFile("https://github.com/citizenfx/cfx-server-data/archive/refs/heads/master.zip", Helper.Paths["dataArchive"]);
        
        Console.WriteLine("STATUS: Extracting server data.");
        Helper.ExtractFile(Helper.Paths["dataArchive"], Helper.Paths["temp"]);
        
        File.Delete(Helper.Paths["dataArchive"]);

        Directory.Move(Helper.Paths["tempFxserverData"], Helper.Paths["fxserverData"]);

        await new Link().Execute(config);
    }
}