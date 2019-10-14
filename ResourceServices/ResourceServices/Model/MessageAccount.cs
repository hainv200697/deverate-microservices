using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class MessageAccount
    {
        public int CompanyId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }

        public MessageAccount(int companyId, string fullname, string email, int role)
        {
            CompanyId = companyId;
            Fullname = fullname;
            Email = email;
            Role = role;
        }
    }
}
