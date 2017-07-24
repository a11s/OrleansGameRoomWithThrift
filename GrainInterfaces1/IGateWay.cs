using System.Threading.Tasks;
using Orleans;

namespace ServerInterface
{
    /// <summary>
    /// Grain interface IGateWay
    /// </summary>
    public interface IGateway : IGrainWithStringKey
    {
        Task<bool> Start();

        Task Stop();
    }
}
