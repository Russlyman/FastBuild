using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FastBuild;

public interface IOption
{
    public Task Execute(IConfigurationRoot config);
}