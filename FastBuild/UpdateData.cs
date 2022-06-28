using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("updatedata", HelpText = "Update cfx-server-data.")]
public class UpdateData : IOption
{
    public async Task Execute(IConfigurationRoot config)
    {
        if (Directory.Exists(Helper.Paths["fxserverData"]))
        {
            Directory.Delete(Helper.Paths["fxserverData"], true);
        }

        if (!Directory.Exists(Helper.Paths["server"]))
        {
            Directory.CreateDirectory(Helper.Paths["server"]);
        }

        await Helper.DownloadFile("https://github.com/citizenfx/cfx-server-data/archive/refs/heads/master.zip", Helper.Paths["dataArchive"]);
        Helper.ExtractFile(Helper.Paths["dataArchive"], Helper.Paths["temp"]);
        File.Delete(Helper.Paths["dataArchive"]);

        Directory.Move(Helper.Paths["tempFxserverData"], Helper.Paths["fxserverData"]);
    }
}