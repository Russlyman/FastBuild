using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("link", HelpText = "Creates a Symbolic Link between the resource and server.")]
internal class Link : IOption
{
    public async Task Execute(IConfigurationRoot config)
    {
        var symLinkPath = Path.Combine(Helper.Paths["resources"], config["resourceName"]);

        if (Directory.Exists(Helper.Paths["resources"]))
        {
            Directory.Delete(Helper.Paths["resources"], true);
        }

        Directory.CreateDirectory(Helper.Paths["resources"]);

        await Helper.CreateSymLink(symLinkPath, config["resourcePath"]);
    }
}