using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using AzureBatchSender;
using Microsoft.Extensions.Configuration;

namespace EventHubBenchmark
{

    public class HubSender : MessagingBase
    {
        private string eventHubName;

        public void SendInBatch(IConfigurationRoot configuration)
        {
            ConnectionString = configuration.GetSection("Messaging:EventHub:ConnectionString").Value;
            eventHubName = configuration.GetSection("Messaging:EventHub:HubName").Value;
            NumThreads = Convert.ToInt32(configuration.GetSection("Messaging:NumberOfThreads").Value);
            NumMessages = Convert.ToInt32(configuration.GetSection("Messaging:NumberOfMessages").Value);

            string msgInput = "Any Content that you want";

            Parallel.For(0, NumThreads, async i =>
            {
                await using (var producerClient = new EventHubProducerClient(ConnectionString, eventHubName))
                {
                    
                    // Create a batch of events 
                    using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                    // Add events to the batch. An event is a represented by a collection of bytes and metadata. 
                    for (int m = 0; m < NumMessages; m++)
                    {
                        eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(msgInput)));
                    }

                    // Use the producer client to send the batch of events to the event hub
                    await producerClient.SendAsync(eventBatch);
                    Console.WriteLine($"A batch of {NumMessages} events has been published.");
                }
            }); 

            Console.WriteLine("Events Created - Wait for posted message");            
        }


    }

}
