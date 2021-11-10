using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ShoppingCart.Payments
{
    public class PaymentFunction
    {
        [FunctionName("PaymentFunction")]
        public static void Run([QueueTrigger("paymentqueue-items", Connection = "PaymentConnection")]BinaryData myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
