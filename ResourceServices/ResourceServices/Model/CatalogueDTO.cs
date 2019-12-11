using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("CatalogueDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CatalogueDTO
    {
        

        public CatalogueDTO()
        {
        }
        public CatalogueDTO(CompanyCatalogue catalogue)
        {
            this.name = catalogue.Name;
            this.catalogueId = catalogue.CompanyCatalogueId;
            this.isActive = catalogue.IsActive;
            this.description = catalogue.Description;
        }
        public CatalogueDTO(CompanyCatalogue catalogue,bool status , int count)
        {
            this.name = catalogue.Name;
            this.quescount = count;
            this.catalogueId = catalogue.CompanyCatalogueId;
            this.isActive = status;
            this.description = catalogue.Description;
        }

        public CatalogueDTO(CompanyCatalogue catalogue, double? weightPoint)
        {
            this.name = catalogue.Name;
            this.catalogueId = catalogue.CompanyCatalogueId;
            this.description = catalogue.Description;
            this.weightPoint = weightPoint;
        }



        public int catalogueId { get; set; }
        public string name { get; set; }
        public int companyId { get; set; }
        public bool isActive { get; set; }
        public string description { get; set; }
        public int quescount { get; set; }
        public double? weightPoint { get; set; }

        public virtual ICollection<Question> question { get; set; }
    }
}
