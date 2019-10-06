using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenServices.Models
{
    public class CompanyManagerDTO
    {
        public int? companyId { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public CompanyManagerDTO() { }
        public CompanyManagerDTO(int? companyId, string fullName, string email)
        {
            this.companyId = companyId;
            this.fullName = fullName;
            this.email = email;
        }
    }
}
