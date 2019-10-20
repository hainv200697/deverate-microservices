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
            this.catalogueId = catalogue.CatalogueId;
            this.name = catalogue.Name;
            this.isActive = catalogue.IsActive;
            this.question = catalogue.Question;
            this.quescount = catalogue.Question.Count;
            this.description = catalogue.Description;
        }

        public CatalogueDTO(Catalogue catalogue, int count)
        {
            this.catalogueId = catalogue.CatalogueId;
            this.name = catalogue.Name;
            this.isActive = catalogue.IsActive;
            this.question = catalogue.Question;
            this.quescount = count;
            this.description = catalogue.Description;
        }

        public CatalogueDTO(Catalogue catalogue, List<Question> ques)
        {
            this.catalogueId = catalogue.CatalogueId;
            this.name = catalogue.Name;
            this.isActive = catalogue.IsActive;
            this.question = catalogue.Question;
            this.description = catalogue.Description;
            this.question = ques;   

        }

        public int catalogueId { get; set; }
        public string name { get; set; }
        public bool? isActive { get; set; }
        public string description { get; set; }
        public int quescount { get; set; }

        public virtual ICollection<Question> question { get; set; }
    }
}
