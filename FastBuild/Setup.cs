using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("setup", HelpText = "Setup FastBuild.")]
public class Setup : IOption
{
    public async Task Execute(IConfigurationRoot config)
    {
        await new UpdateData().Execute(config);
        await new UpdateServer().Execute(config);
    }
}