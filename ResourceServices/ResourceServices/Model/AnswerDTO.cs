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
            this.AnswerId = ans.AnswerId;
            this.Answer = ans.Answer1;
            this.Point = ans.Point;
            this.QuestionId = ans.QuestionId;
            this.IsActive = ans.IsActive;
        }

        [JsonProperty("AnswerId")]
        public int AnswerId { get; set; }
        public int? QuestionId { get; set; }
        public string Answer { get; set; }
        public int Point { get; set; }
        public bool? IsActive { get; set; }

    }
}
