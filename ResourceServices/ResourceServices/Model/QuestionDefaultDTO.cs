using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class QuestionDefaultDTO
    {
        

        public QuestionDefaultDTO()
        {
        }


        public QuestionDefaultDTO(Question ques)
        {
            this.questionDefaultId = ques.QuestionId;
            this.catalogueDefaultId = ques.CatalogueId;
            this.question = ques.Question1;
            this.point = ques.Point;
            this.creatAt = DateTime.UtcNow;
            this.isActive = ques.IsActive;
        }

        public QuestionDefaultDTO(Question ques, string cataName)
        {
            this.questionDefaultId = ques.QuestionId;
            this.catalogueName = cataName;
            this.question = ques.Question1;
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

        public virtual ICollection<Answer> answer { get; set; }

    }
}
