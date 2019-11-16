using AuthenServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("AnswerDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AnswerDTO
    {
        [JsonProperty("answerId")]
        public int? answerId { get; set; }
        [JsonProperty("answer")]
        public string answer { get; set; }
        [JsonProperty("point")]
        public int? point { get; set; }
        [JsonProperty("isActive")]
        public bool? isActive { get; set; }
        public AnswerDTO()
        {

        }

        public AnswerDTO(Answer answer)
        {
            this.answerId = answer.AnswerId;
            this.answer = answer.Answer1;
            this.point = answer.Point;
            this.isActive = answer.IsActive;
        }


    }
}
