using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("setupserver", HelpText = "Setup FXServer.")]
public class SetupServer : IOption
{
    public async Task Execute(IConfigurationRoot config)
    {
        await new UpdateData().Execute(config);
        await new UpdateServer().Execute(config);
    }
}