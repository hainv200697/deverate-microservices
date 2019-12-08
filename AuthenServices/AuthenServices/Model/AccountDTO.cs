using AuthenServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Model
{
    public class AccountDTO
    {
        public int accountId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public bool gender { get; set; }
        public string avatar { get; set; }
        public DateTime joinDate { get; set; }
        public int roleId { get; set; }
        public bool isActive { get; set; }
        public int companyId { get; set; }
    }
}
