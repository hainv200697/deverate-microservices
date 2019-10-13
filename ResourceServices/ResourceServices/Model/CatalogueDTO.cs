using AuthenServices.Models;
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
            this.description = catalogue.Description;
            this.isActive = catalogue.IsActive;
            this.question = catalogue.Question;
            this.quesCount = this.question.Count;
        }



        [JsonProperty("CatalogueId")]
        public int catalogueId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int quesCount { get; set; }
        public bool? isActive { get; set; }
        public virtual ICollection<Question> question { get; set; }
    }
}
