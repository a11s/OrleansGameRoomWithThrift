using Orleans;
using Orleans.Runtime.Configuration;
using ServerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            //先搞一个GrainClient
            //然后thrift监听
            var cfg = new ClientConfiguration();
            cfg.GatewayProvider = ClientConfiguration.GatewayProviderType.None;
            cfg.Gateways.Add(new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 40000));
            cfg.SerializationProviders.Add(typeof(Orleans.Serialization.BondSerializer).GetTypeInfo());
            GrainClient.Initialize(cfg);
            var gw = GrainClient.GrainFactory.GetGrain<IGateway>("gw1");
            var succ = gw.Start().Result;

            initThrift(gw);

            Console.WriteLine($"Gateway {gw.GetPrimaryKeyString()} READY");
            Console.WriteLine("press anykey to exit");
            Console.ReadKey();



        }


        private static void initThrift(IGateway gw)
        {
            G.DefaultGateway = gw;
            var cp = new ClientProcessor(gw);
            var t_processor = new Client.GW.Processor(cp);
            var transport = new Thrift.Transport.TServerSocket(6325, 100000, true);

            //TThreadedServer ps = new TThreadedServer(t_processor, transport);
            System.Threading.ThreadPool.SetMinThreads(1, 0);
            var tps = new Thrift.Server.TThreadPoolServer(t_processor, transport);
            //var ss = new Thrift.Server.TSimpleServer(t_processor, transport);

            Task.Run(() =>
            {
                Console.WriteLine($"{gw.GetPrimaryKeyString()}:Task: thrift started");
                System.Threading.ThreadPool.GetMaxThreads(out var wt, out var cpt);
                Console.WriteLine($"ThreadPool maxwt:{wt} maxcpt:{cpt}");
                tps.Serve();
            });

            Task.Run(() =>
            {

                while (true)
                {
                    cp.CheckHeartbeat();
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
