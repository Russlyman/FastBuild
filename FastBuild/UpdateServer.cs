using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("updateserver", HelpText = "Updates FiveM development server.")]
public class UpdateServer : IOption
{
    public async Task Run(IConfigurationRoot config)
    {

        if (Directory.Exists(Helper.Paths["fxserver"]))
        {
            Directory.Delete(Helper.Paths["fxserver"], true);
            Directory.CreateDirectory(Helper.Paths["fxserver"]);
        }

        var latestArtefactUrl = await Helper.GetLatestArtefactUrl();

        await Helper.DownloadFile(latestArtefactUrl, Helper.Paths["artefectArchive"]);
        Helper.ExtractFile(Helper.Paths["artefectArchive"], Helper.Paths["fxserver"]);
        File.Delete(Helper.Paths["artefectArchive"]);
    }
}