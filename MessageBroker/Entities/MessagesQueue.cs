using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Threading.Channels;

namespace MessageBroker.Entities
{
    public class MessagesQueue
    {

        public List<TcpClient> Clients { get; set; } = new List<TcpClient>();

        private readonly string _Name;

        public Channel <string> Messages { get;private set; }



        public string Name {

            get {


                return _Name;
            }
        }

        public MessagesQueue(string name)
        {
           


            _Name = name;

            Messages = Channel.CreateUnbounded<string>();
            
        }

    }
}
