using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using AzureBatchSender;
using Microsoft.Extensions.Configuration;

namespace EventHubBenchmark
{

    public class BusSender : MessagingBase
    {
        private string QueueName { get;set; }

        public void SendInBatch(IConfigurationRoot configuration)
        {
            string msgInput = "Any Content that you want";


            ConnectionString = configuration.GetSection("Messaging:ServiceBus:ConnectionString").Value;
            QueueName = configuration.GetSection("Messaging:ServiceBus:QueueName").Value;
            NumThreads = Convert.ToInt32(configuration.GetSection("Messaging:NumberOfThreads").Value);
            NumMessages = Convert.ToInt32(configuration.GetSection("Messaging:NumberOfMessages").Value);

            var client = new ServiceBusClient(ConnectionString);
            var sendbatch = client.CreateSender(QueueName);            

            Parallel.For(0, NumThreads, async i =>
            {
                List<ServiceBusMessage> msgs = new List<ServiceBusMessage>();

                for (int a = 0; a < NumMessages; a++)
                {
                    var msg = new ServiceBusMessage(msgInput);
                    msgs.Add(msg);
                }

                await sendbatch.SendMessagesAsync(msgs.AsEnumerable());

            });

            Console.WriteLine($"Messages published on ServiceBus");

        }

    }

}
