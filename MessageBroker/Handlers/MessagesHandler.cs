using MessageBroker.Entities;
using MessageBroker.Enums;
using MessageBroker.Utils;
using System.Collections;
using System.Net.Sockets;


namespace MessageBroker.Handlers
{
    public record MessageHandlerDTO(bool Success ,string? payload  = "",string? Queue = "",string Message = "operation completed succesfully");

    public class MessagesHandler
    {

        public static Func<string, Task>? DequeueDelagete;

      


        public static async Task<MessageHandlerDTO > Handle(RequesParserDTO ParsedMessage,TcpClient client) {


            var _MsgContext = new MessageStorage();

            RequestMethods Method = ParsedMessage.Method;

            if (Method == RequestMethods.Enqueue)
            {
                
                await _MsgContext.Enqueue(ParsedMessage.Queue,ParsedMessage.Messages);

                return new MessageHandlerDTO(true,null);

            }

            if (Method == RequestMethods.Dequeue)
            {


                await HandleDequeue(ParsedMessage,client);
                



                return new MessageHandlerDTO(true,ParsedMessage.Queue);
            }


            if (Method == RequestMethods.CreateQueue)
            {

                string? ret_Queue = _MsgContext.AddQueue(ParsedMessage.Queue);

                if (ret_Queue is null)
                {
                    return new MessageHandlerDTO(false, Message: "failed to create Queue");
                }


                return new MessageHandlerDTO(true,Queue:ret_Queue);

            }

            return new MessageHandlerDTO(false,"no operation specified",string.Empty);


        }

        private static async Task HandleDequeue(RequesParserDTO parsedMessage,TcpClient client)
        {
            var Queues = MessageStorage.Queues;

            string QueueName = parsedMessage.Queue;

            Queues.TryGetValue(QueueName, out MessagesQueue? returnedQueue);

            if (returnedQueue is null)
            {

                returnedQueue = new MessagesQueue(parsedMessage.Queue);

                Queues.TryAdd(parsedMessage.Queue, returnedQueue);
            }

            returnedQueue.Clients.Add(client);


            await QueueHandler.Handle(returnedQueue);


            //while (await returnedQueue.Messages.Reader.WaitToReadAsync()) {



            //    returnedQueue.Messages.Reader.TryRead(out string? Message);

            //    if (Message is null)
            //    {
            //        continue;
            //    }

            //    if (DequeueDelagete is null)
            //    {
            //        throw new Exception("Dequeue Delegate can't be null");
            //    }

            //    await DequeueDelagete(Message);




            //}




            
        }
    }
}
