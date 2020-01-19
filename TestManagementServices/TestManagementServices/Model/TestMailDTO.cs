
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
        public string email { get; set; }
        public string fullName { get; set; }
        public string title { get; set; }
        public DateTime? startDate { get; set; }
        public int expỉredDays { get; set; }
        public string code { get; set; }
        public string testId { get; set; }

        public TestMailDTO() { }
        public TestMailDTO(string email, string fullName, string title, DateTime? startDate, int expỉredDays, string code, string testId)
        {
            this.email = email;
            this.fullName = fullName;
            this.title = title;
            this.startDate = startDate;
            this.expỉredDays = expỉredDays;
            this.code = code;
            this.testId = testId;
        }
    }
}
