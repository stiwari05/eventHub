using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Text;
using System.Threading.Tasks;

namespace _01_SendEvents
{
    class Program
    {
        private const string connectionString = "Endpoint=sb://stdemo.servicebus.windows.net/;SharedAccessKeyName=my-sender;SharedAccessKey=KsZT7IfHfYj6bMWBoijb6MYW480CsJ9SgUKIM5kzA/o=;EntityPath=my-demo";
        private const string eventHubName = "my-demo";

        static async Task Main()
        {
            await using (var producerClient = new EventHubProducerClient(connectionString, eventHubName))
            {
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("First Event")));
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Second Event")));
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Third Event")));

                await producerClient.SendAsync(eventBatch);

                Console.WriteLine("A batch of 3 events have been published.");

            }
        }
    }
}
