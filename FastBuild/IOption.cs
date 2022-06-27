using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

public interface IOption
{
    Task Run(IConfigurationRoot config);
}