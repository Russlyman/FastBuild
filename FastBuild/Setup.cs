using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("setup", HelpText = "Setup FastBuild.")]
public class Setup : IOption
{
    public async Task Execute(IConfigurationRoot config)
    {
        if (Directory.Exists(Helper.Paths["server"]))
        {
            var choice = Helper.Choice("FastBuild has already been setup.\nContinuing will remove your existing development server.\n");

            if (!choice) return;

            Directory.Delete(Helper.Paths["server"], true);
        }

        await new UpdateData().Execute(config);
        await new UpdateServer().Execute(config);
        await new Link().Execute(config);
    }
}