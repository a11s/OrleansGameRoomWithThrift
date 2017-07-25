using GameInterface;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains1
{
    public class BaseMap : Grain, IMap
    {
        Dictionary<string, baseMoveObject> AllObj = new Dictionary<string, baseMoveObject>();
        public Task<bool> Leave(string objKey)
        {
            var r = AllObj.Remove(objKey);
            Console.WriteLine($"{this.GetPrimaryKeyString()}: {objKey} left");
            return Task.FromResult(r);
        }

        public Task Tick(int frame)
        {
            //给每个客户端广播所有人的信息,明显,这里需要优化
            foreach (var item in AllObj)
            {
                var p = GrainClient.GrainFactory.GetGrain<IPlayer>(item.Key);
                SyncAllObjPosToPlayer(p);
            }
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
