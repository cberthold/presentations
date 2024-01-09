using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Shared.Demo3;

namespace FunctionApp.Demo3.Activity
{
    public static class SendNotificationActivity
    {

        [Function(nameof(SendNotification))]
        public static void SendNotification([ActivityTrigger] Input input, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("SubmitToStore");

            logger.LogInformation($"Store completed order for pizza order {input.Request.Order.OrderNumber}.");

        }

        public class Input
        {
            public PizzaOrderRequest Request { get; set; }

            public static Input New(PizzaOrderRequest request) { return new Input { Request = request }; }
        }
    }
}
