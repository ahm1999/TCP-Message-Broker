using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBrokerClient.Entities
{
    internal class Request
    {

        public Request(string Method,List<string> Messages,string Queue,string ClientId)
        {
            this.Method = Method;
            this.Messages = Messages;
            this.Queue = Queue;
            this.ClientId = ClientId;
            
        }

        public string Method { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public ICollection<string>? Messages { get; set; }

        public string Queue { get; set; } = string.Empty;


    }
}
