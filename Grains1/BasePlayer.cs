using GameInterface;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains1
{
    public class BasePlayer : IPlayer
    {

        #region local variable
        public string CurrentMapName { get; set; }
        #endregion

        public string NickName { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public bool LockPos { get; set; }
        public string Data { get; set; }

        public Task MoveTo(string objKey, float x, float y, float z)
        {
            //tell client somthing moved
            if (objKey == NickName)
            {
                //myself?
                this.x = x;
                this.y = y;
                this.z = z;
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

        public Task<bool> TryMove(float fx, float fy, float fz)
        {
            var map = GrainClient.GrainFactory.GetGrain<IMap>(CurrentMapName);

            var succ = map.TryMove(NickName, x + fx, y + fy, z + fz);
            //if (succ)
            //{
            //    //map accept this move request
            //}
            //else
            //{
            //    //map dont want you go to that place
            //}
            return succ;
        }
        #endregion
    }
}
