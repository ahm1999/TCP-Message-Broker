

using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CustomTCPServer
{
    public class TCPServer
    {

        private readonly IPAddress _IPAddress;
        private readonly int _port;

        public Func<TcpClient, Task>? ClientDelegate { get; set; }




        public TCPServer(IPAddress iPAddress, int port)
        {
            _IPAddress = iPAddress;
            _port = port;
            
        }


        public async Task StartServer()
        {



            TcpListener listener = new TcpListener(_IPAddress, _port);

            listener.Start();

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                
                _ = HandleClientAsync(client);

               

            }

    

        }


        private async Task HandleClientAsync(TcpClient client) {



            try
            {
                if (ClientDelegate is null)
                {
                    throw new Exception("no client delegate specified");
                }

                await ClientDelegate(client);



            }
            catch (IOException ex) when (ex.InnerException is SocketException)
            {
                Console.WriteLine("Client connection aborted.");
            }
            catch (SocketException)
            {
                Console.WriteLine("Client connection failed.");
            }
            finally
            {
                client.Close();
            }





        }

        }
    }




    public static class UtilFunctions {

        public static async Task ReadAsync(this TcpClient client, Func<string,Task> action) {


            var stream = client.GetStream();

            byte[] buffer = new byte[10];

            int bytesRead;


           // byte[] result = new byte[0];


            StringBuilder message = new StringBuilder();


            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string chunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                message.Append(chunk);

                if (message.ToString().EndsWith("end"))
                {
                    // Remove the terminator from the message
                    message.Length -= "end".Length;

                    break;
                }


            }
             
             
            await action(message.ToString());


        }


        public static async Task WriteAsync(this TcpClient client, string message) {

            byte[] buffer = Encoding.UTF8.GetBytes(message);

            await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
        
        }
    
    
    
    }



