using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;

namespace _02_RecieveEvents
{
    class Program
    {
        private const string connectionString = "Endpoint=sb://stdemo.servicebus.windows.net/;SharedAccessKeyName=my-reciever;SharedAccessKey=ob00/mRMsyZugp3VJrYF/EYSY6RaWjep8ls7CC8wALI=;EntityPath=my-demo";
        private const string eventHubName = "my-demo";
        private const string blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=azeventhubdemost;AccountKey=kt7mk95Oy8hj7XIRtpiwCRrK9sMteImD42QuVH33NR7QkqFb9vLLSaI1lH4xP1cwgWgEM+IA6+KN+ASt14D3pg==;EndpointSuffix=core.windows.net";
        private const string blobContainerName = "checkpoints";

        static async Task Main()
        {
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            BlobContainerClient storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);
            EventProcessorClient processor = new EventProcessorClient(storageClient, consumerGroup, connectionString, eventHubName);

            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            await processor.StartProcessingAsync();
            await Task.Delay(TimeSpan.FromSeconds(10));

            await processor.StopProcessingAsync();
        }

        static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            // Write the body of the event to the console window
            Console.WriteLine("\tRecevied event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));

            // Update checkpoint in the blob storage so that the app receives only new events the next time it's run
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            // Write details about the error to the console window
            Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }

    }
}
