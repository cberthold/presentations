using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CreditCardProcessor
{
    public class ProcessFunction
    {
        private readonly IRequestProcessor processor;

        public ProcessFunction(IRequestProcessor processor)
        {
            this.processor = processor;
        }

        [FunctionName("Process")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<AuthorizationRequest>
                (requestBody);

            var response = await processor.Process(data);

            if (response.IsSucess)
                return new OkObjectResult(response);
            else
                return new StatusCodeResult(500);
        }
    }
}
