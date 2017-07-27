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

using ServerInterface;
using GrainInterfaces1;
using AppGateway;

public partial class ClientProcessor : Client.GW.Iface
{

    ConcurrentDictionary<string, IPlayer> allPlayersBySid = new ConcurrentDictionary<string, IPlayer>();

    ConcurrentDictionary<string, string> allPlayerAccidBySid = new ConcurrentDictionary<string, string>();


    ConcurrentDictionary<string, Queue<game_event>> allPlayerEventsBySid = new ConcurrentDictionary<string, Queue<game_event>>();

    ConcurrentDictionary<string, EventBuffer> allPlayerEventBuffer = new ConcurrentDictionary<string, EventBuffer>();



    private IGateway gGateway;



    private IMap gMap;


    public ClientProcessor(IGateway baseGateway)
    {
        this.gGateway = baseGateway;



    }

    public string auth(string name)
    {
        Console.WriteLine($"{ gGateway.GetPrimaryKeyString()}:player auth:{name}");
        //todo 这里直接用name 作为sid
        var sid = "sid_" + name;
        var bp = GrainClient.GrainFactory.GetGrain<IPlayer>(name);

        Debug.Assert(bp != null);
        allPlayersBySid[sid] = bp;
        allPlayerAccidBySid[sid] = name;
        allPlayerEventBuffer[sid] = new EventBuffer(sid);
        AllTokens[sid] = new TokenInfo() { LastHeartBeat = DateTime.Now, AccountName = name, SessionId = sid, PlayerName = string.Empty };

        return sid;
    }

    public List<game_event> get_events(string sid)
    {
        if (string.IsNullOrWhiteSpace(sid)) return new List<game_event>();
        if (AllTokens.TryGetValue(sid, out var token))
        {
            token.LastHeartBeat = DateTime.Now;
        }
        if (allPlayerEventBuffer.TryGetValue(sid, out var eb))
        {
            var xx = eb.CurrentEventsBuff.ToList();//有必要拷贝一次?
            eb.SwapEventBuffList();
            return xx;
        }
        else
        {
            return new List<game_event>();
        }

    }

    public List<player_base_info> get_player_list(string sid)
    {
        var list = new List<player_base_info>();
        if (AllTokens.TryGetValue(sid, out var token))
        {
            token.LastHeartBeat = DateTime.Now;
        }
        if (allPlayersBySid.TryGetValue(sid, out var bp))
        {
            var r = bp.GetPlayerBaseInfo().Result;
            if (r.Length > 0)
            {
                gMap = GrainClient.GrainFactory.GetGrain<IMap>(r[0].LastMapId);
                token.CurrMapObserver = new MapObserver();
                token.CurrMapObserver.OnMessage = (m) =>
                {
                    Console.WriteLine("mo got msg:" + m);
                    if (allPlayerEventBuffer.TryGetValue(sid, out var eb))
                    {
                        eb.CurrentEventsBuff.Add(new game_event() { Messages = new List<string>() { m } });
                    }

                };
                var gmo = GrainClient.GrainFactory.CreateObjectReference<IMapObserver>(token.CurrMapObserver);

                token.CurrentMapGrain = gMap;
                token.SubscribedMapObserver = gmo.Result;
                gMap.Subscribe(gmo.Result);

                for (int i = 0; i < r.Length; i++)
                {
                    var pbi = r[i];
                    list.Add(new player_base_info() { Map_id = pbi.LastMapId, Player_name = pbi.PlayerName, Last_map_pos = new vector3() { X = pbi.LastMapPos.x, Y = pbi.LastMapPos.y, Z = pbi.LastMapPos.z } });
                }
            }
        }
        return list;
    }





    public bool join_game(string sid, string player_name)
    {
        // 
        if (allPlayersBySid.TryGetValue(sid, out var bp))
        {
            var res = bp.JoinGame(player_name);
            if (res.Result)
            {
                if (AllTokens.TryGetValue(sid, out var token))
                {
                    token.PlayerName = player_name;
                }
            }
            else
            {
                if (AllTokens.TryGetValue(sid, out var token))
                {
                    token.PlayerName = string.Empty;
                }
            }
            return res.Result;
        }
        return false;
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

