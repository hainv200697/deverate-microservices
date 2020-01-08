using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class CatalogueDefaultDTO
    {
        public CatalogueDefaultDTO()
        {
        }
        public CatalogueDefaultDTO(Catalogue catalogue, int counts)
        {
            this.name = catalogue.Name;
            this.catalogueId = catalogue.CatalogueId;
            this.isActive = catalogue.IsActive;
            this.count = counts;
            this.description = catalogue.Description;
        }

        public CatalogueDefaultDTO(Catalogue catalogue)
        {
            this.name = catalogue.Name;
            this.catalogueId = catalogue.CatalogueId;
            this.isActive = catalogue.IsActive;
            this.description = catalogue.Description;
        }

        public int catalogueId { get; set; }
        public int count { get; set; }
        public string name { get; set; }
        public bool isActive { get; set; }
        public string description { get; set; }
        public bool isDefault { get; set; }
    }
}
