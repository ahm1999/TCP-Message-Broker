using MessageBroker.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MessageBroker.Handlers
{
    public static class QueueHandler
    {
        private static ConcurrentDictionary<string,Task> QueuesHandled = new ConcurrentDictionary<string,Task>();


        internal static async Task Handle(MessagesQueue returnedQueue)
        {
            //QueuesHandled.TryGetValue(returnedQueue.Name, out Task? task);

            //if (task is not null)
            //{
            //    return;
            //}



            var HandledQueueTask = Task.Run(async () =>
            {
                var Queue = returnedQueue.Messages;

                var Clients = returnedQueue.Clients;

                while (await Queue.Reader.WaitToReadAsync())
                {

                    //List<string> Messages  = await Queue.Reader.ReadAllAsync().ToListAsync();

                    if (Clients.Count == 0) continue;


                    foreach (var client in Clients)
                    {

                        if (!client.Connected)
                        {
                            Clients.Remove(client);
                            continue;
                        }

                        if (Queue.Reader.TryRead(out var message))
                        {
                            await client.WriteAsync(message + "\n");
                        }
                        else
                        {
                            break;
                        }



                    }

                }
            });

            QueuesHandled.TryAdd(returnedQueue.Name, HandledQueueTask);


            }

          


        }



    }

