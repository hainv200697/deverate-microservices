using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("QuestionDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class QuestionDTO
    {
        

        public QuestionDTO()
        {
        }
        public QuestionDTO(Question ques,string name, ICollection<Answer> ans)
        {
            this.questionId = ques.QuestionId;
            this.catalogueId = ques.CatalogueId;
            this.question1 = ques.Question1;
            this.maxPoint = ques.MaxPoint;
            this.creatAt = ques.CreatAt;
            this.isActive = ques.IsActive;
            this.createBy = ques.CreateBy;
            this.answer = ans;
            if (String.IsNullOrEmpty(name))
            {
                this.catalogueName = "";
            }
            else
            {
                this.catalogueName = name;
            }
        }

        public QuestionDTO(Question ques)
        {
            this.questionId = ques.QuestionId;
            this.catalogueId = ques.CatalogueId;
            this.question1 = ques.Question1;
            this.maxPoint = ques.MaxPoint;
            this.creatAt = ques.CreatAt;
            this.isActive = ques.IsActive;
            this.createBy = ques.CreateBy;
            this.answer = ques.Answer;
        }

        [JsonProperty("QuestionId")]
        public int questionId { get; set; }
        public int? catalogueId { get; set; }
        public string question1 { get; set; }
        public int? maxPoint { get; set; }
        public int? createBy { get; set; }
        public bool? isActive { get; set; }
        public DateTime? creatAt { get; set; }
        public string catalogueName { get; set; }

        public virtual ICollection<Answer> answer { get; set; }

    }
}
