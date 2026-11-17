
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MessageBrokerClient.Entities
{
    public class MsgBrokerConnection
    {
        private readonly string connectionString;

        private readonly int Port;
        private readonly IPAddress HostName;

        private string ClientId = string.Empty;

        private TcpClient? client;
        public MsgBrokerConnection(string connectionString)
        {
            this.connectionString = connectionString;

            string[] connectiostringAndArray = this.connectionString.Split(':');

            var parsedIP = IPAddress.TryParse(connectiostringAndArray[0], out var ip);

            var parsedPort = int.TryParse(connectiostringAndArray[1], out int result);


            if (!parsedPort||!parsedIP|| ip is null)
            {
                throw new Exception("Invalid connection string and port");
            }

            this.HostName = ip;
            this.Port = result;

        }


        public async Task Connect() {

            if (client is not null)
            {
                return;
            }

            this.client = new TcpClient(this.HostName.ToString(), Port);

           // await this.client.ConnectAsync();

            
        }

        public async Task RecieveMessage(Func<string,Task> action) {

            if (this.client is null) throw new Exception("client not connected");


            byte[] buffer = new byte[1024];

            int BytesRead;

            StringBuilder builder = new StringBuilder();

            while ((BytesRead = await client.GetStream().ReadAsync(buffer,0,buffer.Length)) > 0) {





                string chunk = Encoding.UTF8.GetString(buffer,0,BytesRead);

                if (chunk.Contains("ClientId")) {

                    this.ClientId =  chunk.Split("=")[1].Trim();
                        
                }

                await action(chunk);

            
            }
        
        
        }

        public async Task Unsubscribe(string QueueName) {

            if (this.client is null) throw new Exception("client not connected");

            var request = new
            {
                ClientId = this.ClientId,
                Queue = QueueName,
                Method = "unsubscribe"

            };

            await client.WriteAsync(JsonSerializer.Serialize(request));
            await client.WriteAsync("end");



        }




        public async Task Enqueue(string queueName ,List<string> Messages)
        {

            var request = new Request("enqueue", Messages, queueName,this.ClientId);

            string serializedRequest = JsonSerializer.Serialize(request);

            if (this.client is null) throw new Exception("client not connected");


            await this.client.WriteAsync(serializedRequest);
            await this.client.WriteAsync("end");



        }


        public async Task Dequeue(string queueName)
        {
            if (this.client is null) throw new Exception("client not connected");

            var request = new
            {
                ClientId = this.ClientId,
                Queue = queueName,
                Method = "dequeue"

            };

            await client.WriteAsync(JsonSerializer.Serialize(request));
            await client.WriteAsync("end");



        }

        public string GetClientId() {

            return this.ClientId;
        }







    }
}
