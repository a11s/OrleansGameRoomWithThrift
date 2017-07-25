using System;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using ServerInterface;
using System.Net;



using Thrift;
using Thrift.Protocol;
using Thrift.Transport;
using Thrift.Server;
using System.Threading.Tasks;

namespace SiloHost1
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {

        Task thriftTask;

        TThreadPoolServer ps;


        static void Main(string[] args)
        {
            // First, configure and start a local silo
            var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
            var silo = new SiloHost("TestSilo", siloConfig);
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();

            Console.WriteLine("Silo started.");

            // Then configure and connect a client.
            var clientConfig = ClientConfiguration.LocalhostSilo();
            var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
            client.Connect().Wait();

            Console.WriteLine("Client connected.");

            //
            // This is the place for your test code.
            //
            var xxxx = new ClientConfiguration();
            xxxx.GatewayProvider = ClientConfiguration.GatewayProviderType.None;
            xxxx.Gateways.Add(new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 40000));
            GrainClient.Initialize(xxxx);

            var gw = GrainClient.GrainFactory.GetGrain<IGateway>("gw1");
            var succ = gw.Start().Result;

            initThrift(gw);


            Console.WriteLine("\nPress Enter to terminate...");
            Console.ReadLine();

            // Shut down
            client.Close();
            silo.ShutdownOrleansSilo();
        }

        private static void initThrift(IGateway gw)
        {
            var cp = new ClientProcessor(gw);
            var t_processor = new Client.GW.Processor(cp);
            var transport = new Thrift.Transport.TServerSocket(6325);

            //TThreadedServer ps = new TThreadedServer(t_processor, transport);
            System.Threading.ThreadPool.SetMinThreads(1, 0);
            //var tps = new Thrift.Server.TThreadPoolServer(t_processor, transport);
            var ss = new Thrift.Server.TSimpleServer(t_processor, transport);

            Task.Run(() =>
            {
                Console.WriteLine($"{gw.GetPrimaryKeyString()}:Task: thrift started");
                System.Threading.ThreadPool.GetMaxThreads(out var wt, out var cpt);
                Console.WriteLine($"ThreadPool maxwt:{wt} maxcpt:{cpt}");
                ss.Serve();
            });
        }
    }
}
