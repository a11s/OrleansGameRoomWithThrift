using GameInterface;
using GrainInterfaces1;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains1
{
    

    public class BasePlayer : Grain, IPlayer
    {

        #region local variable
        public string CurrentMapName { get; set; }


        #endregion
        public string AccountId { get; set; }
        public string CurrentPlayerName { get; set; }
        public PlayerBaseInfo CurrentPlayerBaseInfo { get; set; }

        public List<IPlayerBaseInfo> AllPlayersBaseInfo = new List<IPlayerBaseInfo>();




        public Vector3 MapPos { get; set; }
        public bool LockPos { get; set; }
        public string Data { get; set; }

        public override Task OnActivateAsync()
        {
            //load from db

            CurrentPlayerName = this.GetPrimaryKeyString();
            AccountId = this.GetPrimaryKeyString();
            return base.OnActivateAsync();
        }

        public Task<IPlayerBaseInfo[]> GetPlayerBaseInfo()
        {
            List<IPlayerBaseInfo> list = new List<IPlayerBaseInfo>();
            list.Add(new PlayerBaseInfo() { LastMapId = "map1", LastMapPos = new Vector3(), PlayerName = this.CurrentPlayerName, AccountId = this.AccountId });
            AllPlayersBaseInfo = list;

            Console.WriteLine($"{this.GetPrimaryKeyString()}: player {nameof(GetPlayerBaseInfo)}  ");
            return Task.FromResult(list.ToArray());
        }

        public Task<bool> JoinGame(string playerName)
        {
            //其他需要缓存的数据
            //自动加入到地图上
            var pbi = AllPlayersBaseInfo.Find(a => a.PlayerName == playerName);
            if (pbi == null)
            {
                return Task.FromResult(false);
            }
            CurrentPlayerBaseInfo = pbi as PlayerBaseInfo;
            CurrentPlayerName = pbi.PlayerName;
            Console.WriteLine("player joingame");
            var gMap = this.GrainFactory.GetGrain<IMap>(pbi.LastMapId);
            //var joinres = gMap.TryJoin(CurrentPlayerName, "").Result;

            //return Task.FromResult(true);
            Console.WriteLine($"{gMap.GetPrimaryKeyString()}: player {CurrentMapName} join map ");
            return gMap.TryJoin(CurrentPlayerName, "");
        }

        public Task MoveTo(string objKey, double x, double y, double z)
        {
            //tell client somthing moved
            if (objKey == CurrentPlayerName)
            {
                //myself?
                this.MapPos.x = x;
                this.MapPos.y = y;
                this.MapPos.z = z;
            }
            return Task.CompletedTask;
        }

        #region Client request
        /// <summary>
        /// someone changed player pos, like player or skill
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="fz"></param>
        /// <returns></returns>

        public Task<bool> TryMove(double fx, double fy, double fz)
        {
            var map = this.GrainFactory.GetGrain<IMap>(CurrentMapName);

            var succ = map.TryMove(CurrentPlayerName, MapPos.x + fx, MapPos.y + fy, MapPos.z + fz);
            //if (succ)
            //{
            //    //map accept this move request
            //}
            //else
            //{
            //    //map dont want you go to that place
            //}
            Console.WriteLine($"{map.GetPrimaryKeyString()}: player {nameof(TryMove)}  ");
            return succ;
        }
        #endregion
    }
}
