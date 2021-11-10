using System;
using System.Threading.Tasks;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ShoppingCart.Logic.Clients;
using ShoppingCart.Logic.DTO;
using ShoppingCart.Logic.Payment;
using ShoppingCart.Logic.Services;

namespace ShoppingCart.Payments
{
    public class PaymentFunction
    {
        private readonly IPaymentService service;

        public PaymentFunction(IPaymentService service)
        {
            this.service = service;
        }

        [FunctionName("PaymentFunction")]
        public async Task Run([QueueTrigger("paymentqueue-items", Connection = "PaymentConnection")] QueueMessage myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var order = myQueueItem.Body.ToObjectFromJson<Order>();
            await service.AuthorizePayment(order);
            if (order.Status != "Declined" && order.Status != "Paid")
            {
                throw new Exception("Error processing message");
            }   
        }
    }
}
