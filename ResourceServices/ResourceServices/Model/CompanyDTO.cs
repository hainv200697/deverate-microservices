using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class CompanyDTO
    {
        public CompanyDTO()
        {

        }

        public CompanyDTO(Company company, Account account)
        {
            this.companyId = company.CompanyId;
            this.name = company.Name;
            this.address = company.Address;
            this.create_At = company.CreateDate;
            this.phone = company.Phone;
            this.isActive = company.IsActive;
            if (account != null)
            {
                this.managerUserName = account.Username;
                this.managerMail = account.Email;
                this.managerName = account.Fullname;
            }
        }

        public CompanyDTO(Company company)
        {
            this.companyId = company.CompanyId;
            this.name = company.Name;
            this.address = company.Address;
            this.create_At = company.CreateDate;
            this.phone = company.Phone;
            this.isActive = company.IsActive;
        }

        [JsonProperty("companyId")]
        public int? companyId { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public DateTime? create_At { get; set; }
        public string phone { get; set; }
        public bool? isActive { get; set; }
        public string managerUserName { get; set; }
        public string managerMail { get; set; }
        public string managerName { get; set; }
    }
}
