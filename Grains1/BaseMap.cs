using GameInterface;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains1
{
    public class BaseMap : IMap
    {
        Dictionary<string, baseMoveObject> AllObj = new Dictionary<string, baseMoveObject>();
        public Task<bool> Leave(string objKey)
        {
            var r= AllObj.Remove(objKey);
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

        public Task<bool> TryJoin(string objkey, string keyData)
        {
            try
            {
                var bmo = new baseMoveObject() { Data = keyData };
                //todo reset bornpos
                AllObj.Add(objkey, bmo);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public Task<bool> TryMove(string objectKey, float x, float y, float z)
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
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public bool LockPos { get; set; }
        public string Data { get; set; }
    }
}
