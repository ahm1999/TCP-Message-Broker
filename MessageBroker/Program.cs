using CustomTCPServer;
using MessageBroker.Handlers;
using MessageBroker.Utils;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace MessageBroker
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
           

            var _IPAddress = IPAddress.Parse("127.0.0.1");
            var port = 3476;

            TCPServer server = new TCPServer(_IPAddress,port);




            server.ClientDelegate = async (TcpClient client) =>
            {

                //Handshake



                while (client.Connected)
                {

                    await client.ReadAsync(async (message) =>
                    {

                        Console.WriteLine($"Recieved {message}");

                        try
                        {
                            var Result = RequestParser.Handle(message);

                            await client.WriteAsync("\nparsed Succesfully\n");

                            Console.WriteLine($"""
                            method : {Result.Method}
                            Messages : {Result.Messages}
                            queue : {Result.Queue}

                            """);

                            var MsgHandlerResponse =  await MessagesHandler.Handle(Result,client);
                            

                            await client.WriteAsync(MsgHandlerResponse.ToString()+"\n");
                           // await client.WriteAsync("end");

                            //await client.WriteAsync("end");



                        }
                        catch (JsonException ex) {

                            await client.WriteAsync($"error Parsing the message {ex.Message}\n");
                            //await client.WriteAsync("end");
                        }

                        catch (Exception ex)
                        {

                            //Console.WriteLine(ex.ToString());

                            Console.WriteLine( ex.Message,ex.StackTrace);

                        }



                        



                        //Console.WriteLine($"Received: {message}");

                        //await client.WriteAsync($"the message {message}");

                        if (message == "exit")
                        {
                            Console.WriteLine("Client disconnected");
                            client.Close();
                            return;
                        }


                    });
                }



            };

            await server.StartServer();

            



        }
    }
}
