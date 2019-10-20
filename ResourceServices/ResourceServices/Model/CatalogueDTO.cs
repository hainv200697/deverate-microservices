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
            this.CatalogueId = catalogue.CatalogueId;
            this.Name = catalogue.Name;
            this.IsActive = catalogue.IsActive;
            this.Question = catalogue.Question;
            this.quescount = catalogue.Question.Count;
            this.Description = catalogue.Description;
        }

        public CatalogueDTO(Catalogue catalogue, int count)
        {
            this.CatalogueId = catalogue.CatalogueId;
            this.Name = catalogue.Name;
            this.IsActive = catalogue.IsActive;
            this.Question = catalogue.Question;
            this.quescount = count;
            this.Description = catalogue.Description;
        }

        public CatalogueDTO(Catalogue catalogue, List<Question> ques)
        {
            this.CatalogueId = catalogue.CatalogueId;
            this.Name = catalogue.Name;
            this.IsActive = catalogue.IsActive;
            this.Question = catalogue.Question;
            this.Description = catalogue.Description;
            this.Question = ques;   

        }

        [JsonProperty("CatalogueId")]
        public int CatalogueId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string Description { get; set; }
        public int quescount { get; set; }

        public virtual ICollection<Question> Question { get; set; }
    }
}
