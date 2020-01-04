using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class QuestionDTO
    {
        

        public QuestionDTO()
        {
        }


        public QuestionDTO(Question ques, string cataName,int catalogueCompanyId)
        {
                this.questionId = ques.QuestionId;
                this.companyCatalogueId = ques.CatalogueId;
                this.question1 = ques.Question1;
                this.point = ques.Point;
                this.creatAt = ques.CreateAt;
                this.isActive = ques.IsActive;
                this.catalogueName = cataName;
                this.companyCatalogueId = catalogueCompanyId;
            
        }

        [JsonProperty("QuestionId")]
        public int questionId { get; set; }
        public int companyCatalogueId { get; set; }
        public string question1 { get; set; }
        public double point { get; set; }
        public bool isActive { get; set; }
        public DateTime creatAt { get; set; }
        public string catalogueName { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Answer> answer { get; set; }

    }
}
