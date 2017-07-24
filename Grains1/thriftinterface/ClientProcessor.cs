using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;

namespace Grains1
{
    public class ClientProcessor : Client.GW.Iface
    {
        string GatewayName;
        public ClientProcessor(string gw)
        {
            GatewayName = gw;
        }
        public bool auth(string name)
        {
            Console.WriteLine($"{GatewayName}:player auth:{name}");
            return true;
        }

        public List<game_event> get_events()
        {
            return new List<game_event>();
        }

        public bool join_map(string map_name)
        {
            
            return true;
        }

        public bool try_move(string key, double x, double y, double z)
        {
            return true;
        }
    }
}
