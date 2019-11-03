using AuthenServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Model
{
    [JsonObject("AccountDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AccountDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
