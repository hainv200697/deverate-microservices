using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Model;
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
            this.quescount = this.Question.Count;
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
