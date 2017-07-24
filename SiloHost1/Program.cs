using System;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using ServerInterface;
using System.Net;

namespace SiloHost1
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
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
            Console.WriteLine("\nPress Enter to terminate...");
            Console.ReadLine();

            // Shut down
            client.Close();
            silo.ShutdownOrleansSilo();
        }
    }
}
