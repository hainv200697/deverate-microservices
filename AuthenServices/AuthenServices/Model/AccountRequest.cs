using AuthenServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Model
{
    public class AccountRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
