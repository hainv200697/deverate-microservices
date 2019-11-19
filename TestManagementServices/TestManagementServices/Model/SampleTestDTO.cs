using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    [JsonObject("AccountDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SampleTestDTO
    {
        public List<CatalogueDTO> catalogues { get; set; }
        public List<QuestionDTO> questions { get; set; }
        public SampleTestDTO() { }
        public SampleTestDTO(List<CatalogueDTO> catalogues, List<QuestionDTO> questions)
        {
            this.catalogues = catalogues;
            this.questions = questions;
        }
    }
}
