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

        public QuestionDTO(Question ques, ICollection<Answer> ans)
        {
            this.questionId = ques.QuestionId;
            this.cicid = ques.Cicid;
            this.question1 = ques.Question1;
            this.maxPoint = ques.MaxPoint;
            this.creatAt = ques.CreateAt;
            this.isActive = ques.IsActive;
            this.accountId = ques.AccountId;
            this.answer = ans;
        }

        public QuestionDTO(Question ques)
        {
            this.questionId = ques.QuestionId;
            this.cicid = ques.Cicid;
            this.question1 = ques.Question1;
            this.maxPoint = ques.MaxPoint;
            this.creatAt = ques.CreateAt;
            this.isActive = ques.IsActive;
            this.accountId = ques.AccountId;
        }

        public QuestionDTO(List<Question> question, string cataName,int cicid)
        {

            this.Questions = question;
            this.catalogueName = cataName;
            this.cicid = cicid;
        }

        [JsonProperty("QuestionId")]
        public int questionId { get; set; }
        public int? cicid { get; set; }
        public string question1 { get; set; }
        public int? maxPoint { get; set; }
        public int? accountId { get; set; }
        public bool? isActive { get; set; }
        public DateTime? creatAt { get; set; }
        public string catalogueName { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Answer> answer { get; set; }

    }
}
