using BlazorApp.Demo3.Store.Data;
using Microsoft.AspNetCore.Mvc;
using Shared.Demo3;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorApp.Demo3.Store.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly PizzaOrderService service;

        public OrderController(PizzaOrderService service)
        {
            this.service = service;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<PizzaOrderRequest> Get()
        {
            return service.GetOrders();
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task AcceptOrder([FromBody] PizzaOrderRequest value)
        {
            await service.AddOrder(value);
        }

    }
}
