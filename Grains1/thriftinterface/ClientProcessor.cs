using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
using System.Collections.Concurrent;
using Orleans;
using System.Diagnostics;
using GameInterface;

namespace Grains1
{
    public class ClientProcessor : Client.GW.Iface
    {

        ConcurrentDictionary<string, IPlayer> allPlayersBySid = new ConcurrentDictionary<string, IPlayer>();

        ConcurrentDictionary<string, string> allPlayerAccidBySid = new ConcurrentDictionary<string, string>();


        ConcurrentDictionary<string, Queue<game_event>> allPlayerEventsBySid = new ConcurrentDictionary<string, Queue<game_event>>();


        private BaseGateway gGateway;

        public ClientProcessor(BaseGateway baseGateway)
        {
            this.gGateway = baseGateway;
        }

        public string auth(string name)
        {
            Console.WriteLine($"{ gGateway.GetPrimaryKeyString()}:player auth:{name}");
            //todo 这里直接用name 作为sid
            var sid = "sid_" + name;
            var bp = gGateway.GetGBasePlayerByAccId(name);
            
            Debug.Assert(bp != null);
            allPlayersBySid[sid] = bp.Result;
            allPlayerAccidBySid[sid] = name;
            return sid;
        }

        public List<game_event> get_events(string sid)
        {
            return new List<game_event>();
        }

        public List<player_base_info> get_player_list(string sid)
        {
            var list = new List<player_base_info>();
            //if (allPlayersBySid.TryGetValue(sid, out var bp))
            //{
            //    var r = bp.GetPlayerBaseInfo().Result;
            //    for (int i = 0; i < r.Length; i++)
            //    {
            //        var pbi = r[i];
            //        list.Add(new player_base_info() { Map_id = pbi.LastMapId, Player_name = pbi.PlayerName, Last_map_pos = new vector3() { X = pbi.LastMapPos.x, Y = pbi.LastMapPos.y, Z = pbi.LastMapPos.z } });
            //    }
            //}
            if (allPlayerAccidBySid.TryGetValue(sid, out var accid))
            {
                var g = this.gGateway.GetGBasePlayerByAccId(accid);
                //ConfigureAwait(false);
                
                
                //TaskScheduler.
                g.ContinueWith((rr) => {
                    var r = g.Result.GetPlayerBaseInfo();
                    for (int i = 0; i < r.Result.Length; i++)
                    {
                        var pbi = r.Result[i];
                        list.Add(new player_base_info() { Map_id = pbi.LastMapId, Player_name = pbi.PlayerName, Last_map_pos = new vector3() { X = pbi.LastMapPos.x, Y = pbi.LastMapPos.y, Z = pbi.LastMapPos.z } });
                    }
                });
                
                
            }
            return list;
        }





        public bool join_game(string sid, string player_name)
        {
            // 
            return true;
        }

        public bool join_map(string sid, string map_name)
        {

            return true;
        }

        public bool try_move(string sid, string key, double x, double y, double z)
        {
            return true;
        }
    }
}
