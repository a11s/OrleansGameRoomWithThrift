using AppGateway;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public partial class ClientProcessor
{

    public ConcurrentDictionary<string, TokenInfo> AllTokens = new ConcurrentDictionary<string, TokenInfo>();

    public void CheckHeartbeat()
    {
        List<string> timeoutlist = null;
        foreach (var item in AllTokens)
        {
            if (DateTime.Now.Subtract(item.Value.LastHeartBeat).TotalSeconds > TokenInfo.TIMEOUT_HEARTBEAT_SECOND)
            {
                //说明超时了
                if (timeoutlist == null)
                {
                    timeoutlist = new List<string>();
                }
                timeoutlist.Add(item.Key);

            }
        }
        if (timeoutlist != null)
        {
            for (int i = 0; i < timeoutlist.Count; i++)
            {
                if (AllTokens.TryRemove(timeoutlist[i], out var token))
                {
                    Console.WriteLine("Found Player lost:" + token.SessionId);
                    if (allPlayersBySid.TryRemove(token.SessionId, out var p))
                    {
                        //需要通知 自己下线了
                        token.CurrentMapGrain?.UnSubscribe(token?.SubscribedMapObserver);
                        p.LeftGame(token.PlayerName);
                    }
                    allPlayerAccidBySid.TryRemove(token.SessionId, out string xxx);
                    allPlayerEventsBySid.TryRemove(token.SessionId, out var qqq);

                }
            }
            timeoutlist.Clear();

        }
    }
}

