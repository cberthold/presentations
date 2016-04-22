using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web1.Code
{
    public class RandomSale
    {
        public User User { get; set; }
        public decimal Amount { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}