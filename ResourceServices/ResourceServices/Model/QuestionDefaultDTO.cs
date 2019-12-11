using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("QuestionDefaultDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class QuestionDefaultDTO
    {
        

        public QuestionDefaultDTO()
        {
        }


        public QuestionDefaultDTO(DefaultQuestion ques)
        {
            this.questionDefaultId = ques.DefaultQuestionId;
            this.catalogueDefaultId = ques.DefaultCatalogueId;
            this.question = ques.Question;
            this.point = ques.Point;
            this.creatAt = DateTime.UtcNow;
            this.isActive = ques.IsActive;
        }

        [JsonProperty("QuestionDefaultId")]
        public int questionDefaultId { get; set; }
        public int catalogueDefaultId { get; set; }
        public string question { get; set; }
        public double point { get; set; }
        public bool isActive { get; set; }
        public DateTime creatAt { get; set; }
        public string catalogueName { get; set; }

        public virtual ICollection<DefaultAnswer> answer { get; set; }

    }
}
