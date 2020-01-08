using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountController : ControllerBase
    {
        private static int current_count = 0;

        [HttpGet]
        public int Get()
        {
            Interlocked.Increment(ref current_count);
            return current_count;
        }
    }
}
