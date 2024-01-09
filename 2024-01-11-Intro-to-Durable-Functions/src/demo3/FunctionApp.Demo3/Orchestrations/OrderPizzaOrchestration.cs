using FunctionApp.Demo3.Activity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Demo3;

namespace FunctionApp.Demo3.Orchestrations
{
    public static class OrderPizzaOrchestration
    {
        [Function(nameof(OrderPizzaOrchestration))]
        public static async Task<PizzaOrderRequest> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(OrderPizzaOrchestration));
            logger.LogInformation("Starting order");
            var input = context.GetInput<PizzaOrder>();

            // get the next order number for this order
            var orderOutput = await context.CallActivityAsync<GetOrderNumberActivity.Output>(nameof(GetOrderNumberActivity.GetOrderNumber), GetOrderNumberActivity.Input.New(input));

            var request = new PizzaOrderRequest
            {
                Order = orderOutput.Order,
                InstanceId = context.InstanceId,
            };
            // submit to store
            await context.CallActivityAsync(nameof(SubmitToStoreActivity.SubmitToStore), SubmitToStoreActivity.Input.New(request));

            // wait for store to make pizza
            await context.WaitForExternalEvent<bool>("PizzaMade");

            // send email
            await context.CallActivityAsync(nameof(SendNotificationActivity.SendNotification), SendNotificationActivity.Input.New(request));

            return request;
        }


        [Function("OrderPizzaOrchestration_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("OrderPizzaOrchestration_HttpStart");
            //var order = await req.ReadFromJsonAsync<PizzaOrder>();
            string s = req.ReadAsString();
            var order = JsonConvert.DeserializeObject<PizzaOrder>(s);
            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(OrderPizzaOrchestration), order);

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
