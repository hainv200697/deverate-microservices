
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    [JsonObject("TestMailDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class TestMailDTO
    {
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("fullName")]
        public string fullName { get; set; }
        [JsonProperty("title")]
        public string title { get; set; }
        [JsonProperty("startDate")]
        public DateTime? startDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime? endDate { get; set; }
        [JsonProperty("code")]
        public string code { get; set; }

        public TestMailDTO() { }
        public TestMailDTO(string email, string fullName, string title, DateTime? startDate, DateTime? endDate, string code)
        {
            this.email = email;
            this.fullName = fullName;
            this.title = title;
            this.startDate = startDate;
            this.endDate = endDate;
            this.code = code;
        }
    }
}
