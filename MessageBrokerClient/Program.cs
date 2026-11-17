using MessageBrokerClient.Entities;
using System.Net.Sockets;
using System.Text.Json;

namespace MessageBrokerClient
{
    internal class Program
    {
        private static MsgBrokerConnection connection = new MsgBrokerConnection("127.0.0.1:3476");
        static async Task Main(string[] args)
        {





            await connection.Connect();

            _ = connection.RecieveMessage(async (Message) =>
            {
                Console.WriteLine(Message);

                Console.WriteLine(connection.GetClientId());

                //await Task.Delay(3000);

            });

            while (true)
            {
                await ShowMenu();
            }



            //connection.Enqueue()

            //connection.Dequeue()


        }


        public static async Task ShowMenu() {

            Console.WriteLine("""
                Please chose a funcion
                    
                    1 - enqueue
                    2 - dequeue
                    3 - unsubscribe

                """);

            int function = int.Parse(Console.ReadLine() ?? "0");

            //if (!(function == 1 || function == 2|| function == 3))
            //{
            //    return;
            //}

            switch (function) {

                case 1:

                    await SendEnqueue();


                    break;

                case 2:

                    await SendDequeue();
                    break;

                case 3:

                    await SendUnsubscripe();
                    break;
            
            
            }











        }

        private static async Task SendUnsubscripe()
        {
            Console.WriteLine("""
                 
                write queue Name
                """);

            string? Queue = Console.ReadLine();
            if (Queue is null)
            {
                return;
            }

            await connection.Unsubscribe(Queue);

        }

        private static async Task SendEnqueue()
        {
            Console.WriteLine("""
                 
                write queue Name
                """);

            string? Queue = Console.ReadLine();
            if (Queue is null)
            {
                return;
            }

            Console.WriteLine("""
                 
                write Messages
                """);

            string? Messages = Console.ReadLine();

            if (Messages is null)
            {
                return;
            }

            var ListOfMessages = Messages.Split(",").ToList();


            await connection.Enqueue(Queue, ListOfMessages);

        }

        private static async Task SendDequeue()
        {
            Console.WriteLine("""
                 
                write queue Name
                """);

            string? Queue = Console.ReadLine();
            if (Queue is null)
            {
                return;
            }

            await connection.Dequeue(Queue);


        }
    }
}
