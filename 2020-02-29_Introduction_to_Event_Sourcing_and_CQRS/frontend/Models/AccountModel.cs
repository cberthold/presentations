using System;
using System.Collections.Generic;
using System.Linq;

namespace frontend.Models
{
    public class AccountModel
    {
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}