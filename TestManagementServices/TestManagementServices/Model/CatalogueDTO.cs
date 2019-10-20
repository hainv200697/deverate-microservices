using AuthenServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("CataloguesDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
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
        }

        public CatalogueDTO(int? catalogueId, string name, int? numberOfQuestion, double? weightPoint,
            List<QuestionDTO> questions, bool? isActive)
        {
            this.catalogueId = catalogueId;
            this.name = name;
            this.numberOfQuestion = numberOfQuestion;
            this.weightPoint = weightPoint;
            this.questions = questions;
            this.isActive = isActive;
        }

        public CatalogueDTO(int? catalogueId, string name, double? overallPoint, double? thresholdPoint)
        {
            this.catalogueId = catalogueId;
            this.name = name;
            this.overallPoint = overallPoint;
            this.thresholdPoint = thresholdPoint;
        }
        [JsonProperty("CatalogueId")]
        public int? catalogueId { get; set; }
        public string name { get; set; }
        public int? numberOfQuestion { get; set; }
        public double? overallPoint { get; set; }
        public double? weightPoint { get; set; }
        public double? thresholdPoint { get; set; }
        public List<QuestionDTO> questions { get; set; }
        public bool? isActive { get; set; }
    }
}
