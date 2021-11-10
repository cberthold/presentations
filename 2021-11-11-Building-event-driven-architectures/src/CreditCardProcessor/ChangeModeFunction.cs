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
    public class ChangeModeFunction
    {
        private readonly IProcessModeService modeService;

        public ChangeModeFunction(IProcessModeService modeService)
        {
            this.modeService = modeService;
        }

        [FunctionName("ChangeMode")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!int.TryParse(req.Query["mode"], out var iMode))
            {
                var badRequest = new BadRequestObjectResult("query parameter \"mode\" must be a valid integer");
                return badRequest;
            }

            var currentMode = modeService.Mode;
            modeService.SetProcessMode(iMode);

            var responseMessage = new
            {
                Message = "Mode changed",
                PreviousMode = currentMode.ToString(),
                NewMode = modeService.Mode.ToString(),
            };

            return new OkObjectResult(responseMessage);
        }
    }
}
