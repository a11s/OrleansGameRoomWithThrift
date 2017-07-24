using System;
using System.Threading.Tasks;
using Orleans;
using ServerInterface;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;
using Thrift.Server;

namespace Grains1
{
    /// <summary>
    /// Grain implementation class BaseGateway.
    /// </summary>
    public class BaseGateway : Grain, IGateway
    {
        Task thriftTask;

        TThreadPoolServer ps;
        // TODO: replace placeholder grain interface with actual grain
        // communication interface(s).
        public Task<bool> Start()
        {
            var cp = new ClientProcessor(this.GetPrimaryKeyString());
            var t_processor = new Client.GW.Processor(cp);
            var transport = new Thrift.Transport.TServerSocket(6325);

            TThreadedServer ps = new TThreadedServer(t_processor, transport);
            thriftTask = Task.Run(() =>
            {
                Console.WriteLine($"{this.GetPrimaryKeyString()}:Task: thrift started");
                ps.Serve();
            });

            return Task.FromResult<bool>(true);
        }

        public Task Stop()
        {
            if (thriftTask != null)
            {
                // await Task.Run(() => ps.Stop());
                ps.Stop();
                Console.WriteLine($"{this.GetPrimaryKeyString()}:Task: thrift stopped");
            }
            return Task.CompletedTask;
        }
    }
}
