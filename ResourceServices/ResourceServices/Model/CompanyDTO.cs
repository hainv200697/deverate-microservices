﻿using AuthenServices.Models;
using Newtonsoft.Json;
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

        public CompanyDTO(Company company)
        {
            this.CompanyId = company.CompanyId;
            this.Name = company.Name;
            this.Address = company.Address;
            this.Create_At = company.CreateAt;
            this.Fax = company.Fax;
            this.Phone = company.Phone;
            this.IsActive = company.IsActive;
        }

        public CompanyDTO(Company company, string name)
        {
            this.CompanyId = company.CompanyId;
            this.Name = company.Name;
            this.Address = company.Address;
            this.Create_At = company.CreateAt;
            this.Fax = company.Fax;
            this.Phone = company.Phone;
            this.IsActive = company.IsActive;
            this.ManagerName = name;
        }

        [JsonProperty("CompanyId")]
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? Create_At { get; set; }
        public string Fax { get; set; }
        public int? Phone { get; set; }
        public bool? IsActive { get; set; }
        public string ManagerName { get; set; }
    }
}
