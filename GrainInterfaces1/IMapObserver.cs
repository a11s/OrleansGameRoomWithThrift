using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
namespace GrainInterfaces1
{
    public interface IMapObserver : IGrainObserver
    {
        void ReceiveMessage(string message);
        void ObjectMoved(string key, double x, double y, double z);
    }



    public class MapObserver : IMapObserver
    {
        public void ObjectMoved(string key, double x, double y, double z)
        {
            OnObjectMoved?.Invoke(key, x, y, z);
        }

        public void ReceiveMessage(string message)
        {
            OnMessage?.Invoke(message);
            //Console.WriteLine(message);
        }

        public Action<string> OnMessage;

        public Action<string, double, double, double> OnObjectMoved;

    }
}
