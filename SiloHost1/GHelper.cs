using GameInterface;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiloHost1
{
    public class GHelper
    {
        internal static IPlayer GetGBasePlayerByAccId(string name)
        {
            return GrainClient.GrainFactory.GetGrain<IPlayer>(name);
        }
    }
}
