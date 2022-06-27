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
                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory +"appsettings.json", false, false)
                .Build();

            if (Directory.Exists(Helper.Paths["temp"]))
            {
                Directory.Delete(Helper.Paths["temp"], true);
            }

            Directory.CreateDirectory(Helper.Paths["temp"]);

            await Parser.Default.ParseArguments
            <
                Setup,
                StartServer,
                UpdateServer,
                UpdateData
            >
                (args).WithParsedAsync<IOption>(o => o.Run(config));
        }
    }
}