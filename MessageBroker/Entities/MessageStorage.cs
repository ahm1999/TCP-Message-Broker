using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MessageBroker.Entities
{
    public class MessageStorage
    {

        public static ConcurrentDictionary<string, MessagesQueue> Queues = new();

       

        public string? AddQueue(string title) {

            var QueueExists = Queues.Any(kv => kv.Key == title);

            if (QueueExists) {


                return null ;
            }


            //long rangedLong = Random.Shared.NextInt64(100000000000, 900000000000);

            var messageQueue = new MessagesQueue(title);

            bool Success =  Queues.TryAdd(title, messageQueue);


            if (!Success)
            {
                return null;
            }

            return title;


            //return rangedLong;


        }

        public async Task Enqueue(string QueueTitle, List<string> Messages) {

            MessagesQueue? retQueue;


            var Exists = Queues.TryGetValue(QueueTitle, out retQueue);

            if (!Exists|| retQueue is null)
            {
                retQueue =  new MessagesQueue(QueueTitle);

                Queues.TryAdd(QueueTitle,retQueue);
            }

            foreach (var item in Messages)
            {
               await retQueue.Messages.Writer.WriteAsync(item);
            }

            



        }

        public async Task Dequeue(string QueueTitle) {


            Queues.TryGetValue(QueueTitle, out MessagesQueue? retQueue);

            if (retQueue is null) {

                return ;
            }

            

            while (await retQueue.Messages.Reader.WaitToReadAsync())
            {

                retQueue.Messages.Reader.TryRead(out string? result);

                if (result is null)
                {

                    return ;
                }

               


            }



            


        
        
        }



    }
}
