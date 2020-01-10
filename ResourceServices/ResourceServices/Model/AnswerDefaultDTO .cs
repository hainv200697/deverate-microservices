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

        public AnswerDefaultDTO(Answer ans)
        {
            this.answerId = ans.AnswerId;
            this.answer = ans.AnswerText;
            this.percent = ans.Percent;
            this.questionId = ans.QuestionId;
            this.isActive = ans.IsActive;
        }

        [JsonProperty("AnswerId")]
        public int answerId { get; set; }
        public int questionId { get; set; }
        public string answer { get; set; }
        public int percent { get; set; }
        public bool isActive { get; set; }

    }
}
