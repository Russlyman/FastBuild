using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;

namespace FastBuild
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            if (Directory.Exists(Helper.Paths["temp"]))
            {
                Directory.Delete(Helper.Paths["temp"], true);
            }

            Directory.CreateDirectory(Helper.Paths["temp"]);

            await Parser.Default.ParseArguments
            <
                Setup,
                UpdateServer,
                UpdateData
            >
                (args).WithParsedAsync<IOption>(o => o.Run());
        }
    }
}