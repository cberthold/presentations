using System;
using System.Collections.Generic;
using System.Linq;

namespace frontend.Models
{
    public class CreateAccountModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AccountType { get; set; }
    }
}