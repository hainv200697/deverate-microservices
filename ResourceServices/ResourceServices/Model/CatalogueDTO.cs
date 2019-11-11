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
        public CatalogueDTO(Catalogue catalogue)
        {
            this.name = catalogue.Name;
            this.catalogueId = catalogue.CatalogueId;
            this.isActive = catalogue.IsActive;
            this.type = catalogue.Type;
            this.description = catalogue.Description;
        }
        public CatalogueDTO(Catalogue catalogue,bool status , int count)
        {
            this.name = catalogue.Name;
            this.quescount = count;
            this.catalogueId = catalogue.CatalogueId;
            this.isActive = status;
            this.type = catalogue.Type;
            this.description = catalogue.Description;
        }

        public CatalogueDTO(Catalogue catalogue, double? weightPoint)
        {
            this.name = catalogue.Name;
            this.catalogueId = catalogue.CatalogueId;
            this.type = catalogue.Type;
            this.description = catalogue.Description;
            this.weightPoint = weightPoint;

        }



        public int catalogueId { get; set; }
        public string name { get; set; }
        public int companyId { get; set; }
        public bool isActive { get; set; }
        public bool? type { get; set; }
        public string description { get; set; }
        public int quescount { get; set; }
        public double? weightPoint { get; set; }

        public virtual ICollection<Question> question { get; set; }
    }
}
