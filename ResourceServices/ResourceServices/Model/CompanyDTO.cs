﻿using AuthenServices.Models;
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

        public CompanyDTO(Company company, string userName, string email, string fullname)
        {
            this.companyId = company.CompanyId;
            this.name = company.Name;
            this.address = company.Address;
            this.create_At = company.CreateAt;
            this.fax = company.Fax;
            this.phone = company.Phone;
            this.isActive = company.IsActive;
            this.managerUserName = userName;
            this.managerMail = email;
            this.managerName = fullname;
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

        [JsonProperty("companyId")]
        public int? companyId { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public DateTime? create_At { get; set; }
        public string fax { get; set; }
        public int? phone { get; set; }
        public bool? isActive { get; set; }
        public string managerUserName { get; set; }
        public string managerMail { get; set; }
        public string managerName { get; set; }
    }
}
