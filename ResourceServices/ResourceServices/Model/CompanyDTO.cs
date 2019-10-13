using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("CompanyDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CompanyDTO
    {
        public CompanyDTO()
        {

        }

        public CompanyDTO(Company company, string name)
        {
            this.companyId = company.CompanyId;
            this.name = company.Name;
            this.address = company.Address;
            this.create_At = company.CreateAt;
            this.fax = company.Fax;
            this.phone = company.Phone;
            this.isActive = company.IsActive;
            this.managerName = name;
        }
        public CompanyDTO(Account account)
        {
            this.companyId = account.Company.CompanyId;
            this.name = account.Company.Name;
            this.address = account.Company.Address;
            this.create_At = account.Company.CreateAt;
            this.fax = account.Company.Fax;
            this.phone = account.Company.Phone;
            this.isActive = account.Company.IsActive;
            this.managerName = account.Fullname;
        }

        public CompanyDTO(Company company)
        {
            this.companyId = company.CompanyId;
            this.name = company.Name;
            this.address = company.Address;
            this.create_At = company.CreateAt;
            this.fax = company.Fax;
            this.phone = company.Phone;
            this.isActive = company.IsActive;
        }

        [JsonProperty("CompanyId")]
        public int? companyId { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public DateTime? create_At { get; set; }
        public string fax { get; set; }
        public int? phone { get; set; }
        public bool? isActive { get; set; }
        public string managerName { get; set; }
    }
}
