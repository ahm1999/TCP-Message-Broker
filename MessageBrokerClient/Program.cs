using MessageBrokerClient.Entities;
using System.Net.Sockets;
using System.Text.Json;

namespace MessageBrokerClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
           
            var connection = new MsgBrokerConnection("127.0.0.1:3476");


            connection.Connect();

            //connection.Enqueue()

            //connection.Dequeue()
           

        }

    }
}
