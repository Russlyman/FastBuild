using System.Diagnostics;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

[Verb("start", HelpText = "Start FXServer.")]
internal class StartServer : IOption
{
    public async Task Execute(IConfigurationRoot config)
    {
        var server = new ProcessStartInfo
        {
            WorkingDirectory = Helper.Paths["fxserverData"],
            FileName = Helper.Paths["fxserverBinary"],
            Arguments = $"+set sv_licenseKey {config["fxserverLicenseKey"]} +exec \"{ Helper.Paths["fxserverConfig"] }\" +ensure { config["resourceName"] }"
        };

        using var process = Process.Start(server);
        await process.WaitForExitAsync();
    }
}