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
            this.QuestionId = ques.QuestionId;
            this.CatalogueId = ques.CatalogueId;
            this.Question1 = ques.Question1;
            this.MaxPoint = ques.MaxPoint;
            this.CreatAt = ques.CreatAt;
            this.IsActive = ques.IsActive;
            this.CreateBy = ques.CreateBy;
            this.Answer = ans;
            if (String.IsNullOrEmpty(name))
            {
                this.CatalogueName = "";
            }
            else
            {
                this.CatalogueName = name;
            }
        }

        public QuestionDTO(Question ques)
        {
            this.QuestionId = ques.QuestionId;
            this.CatalogueId = ques.CatalogueId;
            this.Question1 = ques.Question1;
            this.MaxPoint = ques.MaxPoint;
            this.CreatAt = ques.CreatAt;
            this.IsActive = ques.IsActive;
            this.CreateBy = ques.CreateBy;
            this.Answer = ques.Answer;
        }

        [JsonProperty("QuestionId")]
        public int QuestionId { get; set; }
        public int? CatalogueId { get; set; }
        public string Question1 { get; set; }
        public int? MaxPoint { get; set; }
        public int? CreateBy { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatAt { get; set; }
        public string CatalogueName { get; set; }

        public virtual ICollection<Answer> Answer { get; set; }

    }
}
