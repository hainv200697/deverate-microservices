﻿using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class Company
    {
        public Company()
        {
            Account = new HashSet<Account>();
            CatalogueInCompany = new HashSet<CatalogueInCompany>();
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool? IsActive { get; set; }
        public int? Phone { get; set; }
        public string Fax { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<CatalogueInCompany> CatalogueInCompany { get; set; }
    }
}
