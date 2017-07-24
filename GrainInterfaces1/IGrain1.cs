using System.Threading.Tasks;
using Orleans;

namespace GameInterface
{
    /// <summary>
    /// Grain interface IGrain1
    /// </summary>
    public interface ISample : IGrainWithGuidKey
    {

    }

    public interface IPlayer : IGrainWithStringKey
    {
        

        Task<bool> TryMove(float fx, float fy, float fz);

        Task MoveTo(string objKey, float x, float y, float z);



    }

    public interface IMap : IGrainWithStringKey
    {
        Task<bool> TryMove(string objectKey, float x, float y, float z);

        Task<bool> TryJoin(string objkey, string keyData);
        Task<bool> Leave(string objKey);

        Task Tick(int frame);
    }


}
