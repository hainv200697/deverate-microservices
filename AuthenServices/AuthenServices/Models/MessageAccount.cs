using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Models
{
    public class MessageAccount
    {
        public int CompanyId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Role { get; set; }
        public bool Gender { get; set; }
    }
}