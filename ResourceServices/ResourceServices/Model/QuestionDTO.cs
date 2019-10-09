﻿using AuthenServices.Models;
using Newtonsoft.Json;
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
            this.QuestionId = ques.QuestionId;
            this.CatalogueId = ques.CatalogueId;
            this.Question1 = ques.Question1;
            this.MaxPoint = ques.MaxPoint;
            this.CreatAt = ques.CreatAt;
            this.IsActive = ques.IsActive;
            this.CreateBy = ques.CreateBy;
            this.Answer = ans;

        }



        [JsonProperty("QuestionId")]
        public int QuestionId { get; set; }
        public int? CatalogueId { get; set; }
        public string Question1 { get; set; }
        public int? MaxPoint { get; set; }
        public int? CreateBy { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatAt { get; set; }

        public virtual ICollection<Answer> Answer { get; set; }

    }
}
