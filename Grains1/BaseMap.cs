using GameInterface;
using GrainInterfaces1;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Grains1
{
    public class BaseMap : Grain, IMap, IRemindable
    {
        Dictionary<string, baseMoveObject> AllObj = new Dictionary<string, baseMoveObject>();

        Dictionary<string, IPlayer> CachedPlayer = new Dictionary<string, IPlayer>();

        private ObserverSubscriptionManager<IMapObserver> _subsManager;

        IDisposable timer;

        int TickCount = 0;

        public override Task OnActivateAsync()
        {
            _subsManager = new ObserverSubscriptionManager<IMapObserver>();
            RegisterOrUpdateReminder("reminder_" + this.GetPrimaryKeyString(), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            timer = RegisterTimer((a) =>
             {
                 Tick(TickCount++);
                 return Task.CompletedTask;
             }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

            return base.OnActivateAsync();
        }

        public Task Subscribe(IMapObserver observer)
        {
            _subsManager.Subscribe(observer);
            return Task.CompletedTask;
        }

        //Also clients use this to unsubscribe themselves to no longer receive the messages.
        public Task UnSubscribe(IMapObserver observer)
        {
            _subsManager.Unsubscribe(observer);
            return Task.CompletedTask;
        }
        public Task SendUpdateMessage(string message)
        {
            Console.WriteLine("BroadCast:" + message);
            _subsManager.Notify(s => s.ReceiveMessage(message));
            return Task.CompletedTask;
        }

        public Task<bool> Leave(string objKey)
        {
            var r = AllObj.Remove(objKey);
            var msg = $"{this.GetPrimaryKeyString()}: {objKey} left";
            SendUpdateMessage($"{this.GetPrimaryKeyString()}: {objKey} left");
            //移除
            CachedPlayer.Remove(objKey);
            return Task.FromResult(r);
        }

        public Task Tick(int frame)
        {
            //给每个客户端广播所有人的信息,明显,这里需要减少发送量
            //foreach (var item in AllObj)
            //{

            //    IPlayer p = null;
            //    if (CachedPlayer.TryGetValue(item.Key, out p))
            //    {

            //    }
            //    else
            //    {
            //        p = this.GrainFactory.GetGrain<IPlayer>(item.Key);
            //        CachedPlayer[item.Key] = p;
            //    }
            //    SyncAllObjPosToPlayer(p);
            //}
            try
            {
                foreach (var item in AllObj)
                {

                    IPlayer p = null;
                    if (CachedPlayer.TryGetValue(item.Key, out p))
                    {

                    }
                    else
                    {
                        p = this.GrainFactory.GetGrain<IPlayer>(item.Key);
                        CachedPlayer[item.Key] = p;
                    }
                    SyncAllObjPosToPlayer(p);
                }
            }
            catch (Exception xxxxx)
            {
                Console.WriteLine("HOHOHOHO");
            }


            SendUpdateMessage($"tick {frame} Obj:{AllObj.Count}");
            return Task.CompletedTask;
        }

        private void SyncAllObjPosToPlayer(IPlayer p)
        {
            foreach (var item in AllObj)
            {
                p.MoveTo(item.Key, item.Value.x, item.Value.y, item.Value.z);
            }
        }

        public Task<bool> TryJoin(string objKey, string keyData)
        {
            try
            {
                var bmo = new baseMoveObject() { Data = keyData };
                //todo reset bornpos
                AllObj.Add(objKey, bmo);
                Console.WriteLine($"{this.GetPrimaryKeyString()}: {objKey} joined");

                SendUpdateMessage($"{objKey} joined map {this.GetPrimaryKeyString()}");
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }



            return Task.FromResult(true);
        }

        public Task<bool> TryMove(string objectKey, double x, double y, double z)
        {
            if (AllObj.TryGetValue(objectKey, out var obj))
            {
                obj.x += x;
                obj.y += y;
                obj.z += z;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task ReceiveReminder(string reminderName, TickStatus status)
        {
            Tick(TickCount++);
            return Task.CompletedTask;
        }
    }

    public class baseMoveObject
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public bool LockPos { get; set; }
        public string Data { get; set; }
    }
}
