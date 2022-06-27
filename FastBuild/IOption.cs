using System.Threading.Tasks;

namespace FastBuild;

public interface IOption
{
    Task Run();
}