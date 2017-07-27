using System.Threading.Tasks;
using Orleans;
using System.Collections.Generic;
using GrainInterfaces1;

namespace GameInterface
{
    public interface IVector3
    {
        double x { get; set; }
        double y { get; set; }
        double z { get; set; }
    }
    public interface IPlayerBaseInfo
    {
        string AccountId { get; set; }
        string PlayerName { get; set; }
        string LastMapId { get; set; }
        IVector3 LastMapPos { get; set; }

    }


    public interface IPlayer : IGrainWithStringKey
    {


        Task<bool> TryMove(double fx, double fy, double fz);

        Task MoveTo(string objKey, double x, double y, double z);

        Task<IPlayerBaseInfo[]> GetPlayerBaseInfo();

        Task<bool> JoinGame(string playerName);


        Task<bool> LeftGame(string playerName);

    }

    public interface IMap : IGrainWithStringKey
    {
        Task<bool> TryMove(string objectKey, double x, double y, double z);

        Task<bool> TryJoin(string objkey, string keyData);
        Task<bool> Leave(string objKey);

        Task Tick(int frame);


        Task UnSubscribe(IMapObserver observer);
        Task Subscribe(IMapObserver observer);

        Task SendUpdateMessage(string message);//真的需要吗?
    }


}
