using GameInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces1
{
    [Serializable]
    public class PlayerBaseInfo : IPlayerBaseInfo
    {
        public string PlayerName { get; set; }
        public string LastMapId { get; set; }
        public IVector3 LastMapPos { get; set; }
        public string AccountId { get; set; }
    }
    [Serializable]
    public class Vector3 : IVector3
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }
}
