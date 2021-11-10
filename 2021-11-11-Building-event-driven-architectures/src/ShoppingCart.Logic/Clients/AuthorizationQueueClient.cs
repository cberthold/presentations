using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using ShoppingCart.Logic.Payment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Clients
{
    public class AuthorizationQueueClient
    {
        private readonly QueueClient client;

        public AuthorizationQueueClient(IConfiguration configuration)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            client = CreateClient(configuration);
        }

        private QueueClient CreateClient(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("PaymentConnection");
            var queueClient = new QueueClient(connectionString, "paymentqueue-items", new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });
            queueClient.CreateIfNotExists();
            return queueClient;
        }

        public async Task SendAuthorization(AuthorizationRequest request)
        {
            string messageAsJson = JsonSerializer.Serialize(request);
            BinaryData cloudQueueMessage = new BinaryData(messageAsJson);
            await client.SendMessageAsync(cloudQueueMessage);
        }
    }
}
