using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Shared.Demo3;

namespace FunctionApp.Demo3.Activity
{
    public class GetOrderNumberActivity
    {
        [Function(nameof(GetOrderNumber))]
        public static Output GetOrderNumber([ActivityTrigger] Input input, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("GetOrderNumber");
            
            input.Order.OrderNumber = OrderCounter.GetNext();
            logger.LogInformation($"Getting request for pizza order {input.Order.OrderNumber}.");

            return new Output
            {
                Order = input.Order,
            };
        }

        public class Input
        {
            public PizzaOrder Order { get; set; }

            public static Input New(PizzaOrder order)
            {
                return new Input { Order = order };
            }
        }

        public class Output
        {
            public PizzaOrder Order { get; set; }
        }
    }
}
