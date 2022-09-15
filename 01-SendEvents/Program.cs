using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _01_SendEvents
{
    class Program
    {
        private const string connectionString = "Endpoint=sb://stdemo.servicebus.windows.net/;SharedAccessKeyName=my-sender;SharedAccessKey=KsZT7IfHfYj6bMWBoijb6MYW480CsJ9SgUKIM5kzA/o=;EntityPath=my-demo";
        private const string eventHubName = "my-demo";

        //static async Task Main()
        //{
        //    await using (var producerClient = new EventHubProducerClient(connectionString, eventHubName))
        //    {
        //        using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

        //        eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("First Event")));
        //        eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Second Event")));
        //        eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Third Event")));

        //        await producerClient.SendAsync(eventBatch);

        //        Console.WriteLine("A batch of 3 events have been published.");

        //    }
        //}

        static void Main(string[] args)
        {

            EventHubProducerClient _client = new EventHubProducerClient(connectionString, eventHubName);

            EventDataBatch _batch = _client.CreateBatchAsync().GetAwaiter().GetResult();

            for (int i = 0; i < 20; i++)
            {
                List<Order> _orders = new List<Order>()
                {
                    new Order() {OrderID="O1",Quantity=10,UnitPrice=9.99m,DiscountCategory="Tier 1" },
                    new Order() {OrderID="O2",Quantity=15,UnitPrice=10.99m,DiscountCategory="Tier 2" },
                    new Order() {OrderID="O3",Quantity=20,UnitPrice=11.99m,DiscountCategory="Tier 3" },
                    new Order() {OrderID="O4",Quantity=25,UnitPrice=12.99m,DiscountCategory="Tier 1" },
                    new Order() {OrderID="O5",Quantity=30,UnitPrice=13.99m,DiscountCategory="Tier 2" }
                };

                foreach (Order _order in _orders)
                    _batch.TryAdd(new Azure.Messaging.EventHubs.EventData(Encoding.UTF8.GetBytes(_order.ToString())));

                _client.SendAsync(_batch).GetAwaiter().GetResult();
            }

            Console.WriteLine("Batch of events sent");
        }

    }
}
