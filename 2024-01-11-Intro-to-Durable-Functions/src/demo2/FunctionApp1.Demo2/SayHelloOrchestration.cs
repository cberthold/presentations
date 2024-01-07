using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace FunctionApp1.Demo2
{
    public static class SayHelloOrchestration
    {
        [Function(nameof(SayHelloOrchestration))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(SayHelloOrchestration));
            logger.LogInformation("Saying hello.");
            //var outputs = new List<string>();
            var tasks = new List<Task<string>>();
            // Replace name and input with values relevant for your Durable Functions Activity
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));
            tasks.Add(context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
            tasks.Add(context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            tasks.Add(context.CallActivityAsync<string>(nameof(SayHello), "London"));

            string[] taskOutputs = await Task.WhenAll(tasks);
            var outputs = taskOutputs.ToList();

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [Function(nameof(SayHello))]
        public static async Task<string> SayHello([ActivityTrigger] string name, FunctionContext executionContext)
        {
            var random = new Random();
            int delayMs = random.Next(700, 5000);
            await Task.Delay(delayMs);
            ILogger logger = executionContext.GetLogger("SayHello");
            logger.LogInformation("Saying hello to {name}.", name);
            return $"Hello {name}, after {delayMs} Milliseconds!";
        }

        [Function("SayHelloOrchestration_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("SayHelloOrchestration_HttpStart");

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(SayHelloOrchestration));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
