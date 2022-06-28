using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("setup", HelpText = "Setup FastBuild.")]
public class Setup : IOption
{
    public async Task Run(IConfigurationRoot config)
    {
        if (Directory.Exists(Helper.Paths["server"]))
        {
            var choice = Helper.Choice("FastBuild has already been setup.\nContinuing will remove your existing development server.\n");

            if (!choice) return;

            Directory.Delete(Helper.Paths["server"], true);
        }

        await new UpdateData().Run(config);
        await new UpdateServer().Run(config);
        await new Link().Run(config);
    }
}