using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class CatalogueDTO
    {
        

        public CatalogueDTO()
        {
        }
        public CatalogueDTO(Catalogue catalogue)
        {
            this.name = catalogue.Name;
            this.companyCatalogueId = catalogue.CatalogueId;
            this.isActive = catalogue.IsActive;
            this.description = catalogue.Description;
        }
        public CatalogueDTO(Catalogue catalogue,bool status , int count)
        {
            this.name = catalogue.Name;
            this.quescount = count;
            this.companyCatalogueId = catalogue.CatalogueId;
            this.isActive = status;
            this.description = catalogue.Description;
        }

        public CatalogueDTO(CompanyCatalogue catalogue, double? weightPoint)
        {
            this.name = catalogue.Name;
            this.companyCatalogueId = catalogue.CompanyCatalogueId;
            this.description = catalogue.Description;
            this.weightPoint = weightPoint;
        }



        public int companyCatalogueId { get; set; }
        public string name { get; set; }
        public int companyId { get; set; }
        public bool isActive { get; set; }
        public string description { get; set; }
        public int quescount { get; set; }
        public double? weightPoint { get; set; }

        public virtual ICollection<Question> question { get; set; }
    }
}
