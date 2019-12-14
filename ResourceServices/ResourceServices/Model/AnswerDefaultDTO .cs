using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class AnswerDefaultDTO
    {
        

        public AnswerDefaultDTO()
        {
        }

        public AnswerDefaultDTO(DefaultAnswer ans)
        {
            this.answerId = ans.DefaultAnswerId;
            this.answer = ans.Answer;
            this.percent = ans.Percent;
            this.questionId = ans.DefaultQuestionId;
            this.isActive = ans.IsActive;
        }

        [JsonProperty("AnswerId")]
        public int answerId { get; set; }
        public int? questionId { get; set; }
        public string answer { get; set; }
        public int percent { get; set; }
        public bool? isActive { get; set; }

    }
}
