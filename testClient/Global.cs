using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
using Thrift;
using Thrift.Protocol;

using Thrift.Transport;

using System.Threading;
using System.Windows.Threading;

namespace testClient
{
    public class SingleThreadTimer
    {
        object syncObject = new object();
        private Dispatcher dispatcher;
        private readonly Action callback;

        /// <summary>
        /// 一个脉冲信号
        /// </summary>
        public void Pulse()
        {
            var succ = Monitor.TryEnter(syncObject, 1);
            if (succ)
            {
                try
                {
                    dispatcher.Invoke(callback);
                }
                catch(System.Net.Sockets.SocketException se)
                {
                    //todo: socket
                }
                catch (System.IO.IOException ioe)
                {
                    //todo: network 
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    Monitor.Exit(syncObject);
                }
            }
        }

        public SingleThreadTimer(Dispatcher dispatcher, Action a)
        {
            this.dispatcher = dispatcher;
            this.callback = a;
        }
    }
    public class Global
    {
        private static Timer innerTimer = null;

        private static SingleThreadTimer STT = null;

        public static void InitTimer(Dispatcher dispatcher, Action a)
        {
            STT = new SingleThreadTimer(dispatcher, a);
            innerTimer = new Timer(
            (aaa) =>
            {
                STT?.Pulse();
            },
            null, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));

        }
        public static GW.Client Network { get; private set; }
        public static bool IsAuthSuccess { get; internal set; }
        public static string SessionId { get; internal set; }
        public static player_base_info CurrentPlayerBaseInfo { get; internal set; }

        public static GW.Client GetNetwork(string text)
        {
            var arr = text.Split(":"[0]);
            var ip = arr[0];
            int port = int.Parse(arr[1]);

            TTransport transport = new TSocket(ip, port, 100000);
            TProtocol protocol = new TBinaryProtocol(transport);
            try
            {
                transport.Open();
            }
            catch
            {
                return null;

            }
            var c = new Client.GW.Client(protocol);
            Global.Network = c;
            return c;
        }


    }
}
