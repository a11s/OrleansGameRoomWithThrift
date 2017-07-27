using System;
using GameInterface;
using GrainInterfaces1;

namespace AppGateway
{
    public class TokenInfo
    {
        public const int TIMEOUT_HEARTBEAT_SECOND = 3;
        public string SessionId;
        public DateTime LastHeartBeat;

        public string PlayerName { get; internal set; }
        public string AccountName { get; internal set; }
        public MapObserver CurrMapObserver { get; internal set; }
        public IMap CurrentMapGrain { get; internal set; }
        public IMapObserver SubscribedMapObserver { get; internal set; }
    }
}