using System;
using System.Threading.Tasks;
using Orleans;
using ServerInterface;

using GameInterface;

namespace Grains1
{
    /// <summary>
    /// Grain implementation class BaseGateway.
    /// </summary>
    public class BaseGateway : Grain, IGateway
    {

        // TODO: replace placeholder grain interface with actual grain
        // communication interface(s).
        public Task<bool> Start()
        {
            

            return Task.FromResult<bool>(true);
        }

        public Task Stop()
        {
          
            return Task.CompletedTask;
        }

        public Task<IPlayer> GetGBasePlayerByAccId(string accid)
        {
            var g = this.GrainFactory.GetGrain<IPlayer>(accid);
            
            return Task.FromResult(g);
        }
    }
}
