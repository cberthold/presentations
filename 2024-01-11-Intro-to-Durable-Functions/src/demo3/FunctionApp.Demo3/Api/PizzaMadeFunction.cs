using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace FunctionApp.Demo3.Api
{
    public class PizzaMadeFunction
    {
        private readonly ILogger _logger;

        public PizzaMadeFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PizzaMadeFunction>();
        }

        [Function(nameof(PizzaMadeFunction))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "pizza/made/{instanceId}")] HttpRequestData req, 
            string instanceId,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            _logger.LogInformation($"C# HTTP trigger function processed a PizzaMade event request for instance Id {instanceId}.");

            await client.RaiseEventAsync(instanceId, "PizzaMade", true);

            var response = req.CreateResponse(HttpStatusCode.OK);

            return response;
        }
    }
}
