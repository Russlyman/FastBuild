using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace FastBuild
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"), false, false)
                .Build();

            config["resourceName"] = new DirectoryInfo(config["resourcePath"]).Name;

            if (Directory.Exists(Helper.Paths["temp"]))
            {
                Directory.Delete(Helper.Paths["temp"], true);
            }

            Directory.CreateDirectory(Helper.Paths["temp"]);

            await Parser.Default.ParseArguments
            <
                Setup,
                StartServer,
                Build,
                UpdateServer,
                UpdateData,
                Link
            >
                (args).WithParsedAsync<IOption>(o => o.Execute(config));
        }
    }
}