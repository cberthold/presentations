using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web1.Code
{
    public class ResponsePayload<T>
    {
        public T Payload { get; set; }
    }
}