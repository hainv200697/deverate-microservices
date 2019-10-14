using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class Company
    {
        public Company()
        {
            CompanyCatalogue = new HashSet<CompanyCatalogue>();
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool? IsActive { get; set; }
        public int? Phone { get; set; }
        public string Fax { get; set; }

        public virtual ICollection<CompanyCatalogue> CompanyCatalogue { get; set; }
    }
}
