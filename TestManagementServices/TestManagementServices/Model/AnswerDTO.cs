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
        [JsonProperty("AnswerId")]
        public int? AnswerId { get; set; }
        [JsonProperty("Point")]
        public int? Point { get; set; }
        [JsonProperty("IsActive")]
        public bool? IsActive { get; set; }
        public AnswerDTO()
        {

        }

        public AnswerDTO(Answer answer)
        {
            this.AnswerId = answer.AnswerId;
            this.Point = answer.Point;
            this.IsActive = answer.IsActive;
        }


    }
}
