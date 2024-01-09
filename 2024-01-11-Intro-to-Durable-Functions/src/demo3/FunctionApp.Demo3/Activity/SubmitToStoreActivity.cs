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
    public class SubmitToStoreActivity
    {
        private readonly IHttpClientFactory httpClientFactory;

        public SubmitToStoreActivity(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [Function(nameof(SubmitToStore))]
        public async Task SubmitToStore([ActivityTrigger] Input input, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("SubmitToStore");

            logger.LogInformation($"Submitting request for pizza order {input.Request.Order.OrderNumber} to store.");

            var client = httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7073/api/order", input.Request);
        }

        public class Input
        {
            public PizzaOrderRequest Request { get; set; }

            public static Input New(PizzaOrderRequest request) { return new Input { Request = request }; }
        }
    }
}
