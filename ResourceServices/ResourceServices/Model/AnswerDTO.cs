using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("AnswerDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AnswerDTO
    {
        

        public AnswerDTO()
        {
        }

        public AnswerDTO(Answer ans)
        {
            this.answerId = ans.AnswerId;
            this.answer = ans.Answer1;
            this.point = ans.Point;
            this.questionId = ans.QuestionId;
            this.isActive = ans.IsActive;
        }

        [JsonProperty("AnswerId")]
        public int answerId { get; set; }
        public int questionId { get; set; }
        public string answer { get; set; }
        public int point { get; set; }
        public bool? isActive { get; set; }

    }
}
