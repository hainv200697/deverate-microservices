using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class MessageAccount
    {
        public int? CompanyId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public int Role { get; set; }
        public bool Gender { get; set; }


        public MessageAccount(int? companyId, string fullname, string email, int role,string address, bool gender, int phone)
        {
            CompanyId = companyId;
            Fullname = fullname;
            Email = email;
            Role = role;
            Address = address;
            Gender = gender;
            Phone = phone;
        }
    }
}
