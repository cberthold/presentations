using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Redis;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountController : ControllerBase
    {
        private readonly IRedisClientsManager clientsManager;

        public CountController(IRedisClientsManager clientsManager)
        {
            this.clientsManager = clientsManager;
        }

        [HttpGet]
        public long Get()
        {
            using (IRedisClient redis = clientsManager.GetClient())
            {
                return redis.IncrementValue("counter");
            }
        }
    }
}
