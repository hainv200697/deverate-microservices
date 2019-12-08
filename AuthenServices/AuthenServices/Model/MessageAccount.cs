using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Models
{
    public class MessageAccount
    {
        public MessageAccount(int companyId, string fullname, string email, int role, string address, bool gender, string phone)
        {
            CompanyId = companyId;
            Fullname = fullname;
            Email = email;
            Role = role;
            Address = address;
            Gender = gender;
            Phone = phone;
        }
        public int CompanyId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Role { get; set; }
        public bool Gender { get; set; }
    }
}