using System;
namespace MailingServices.Model
{
    public class TestMailDTO
    {
        public TestMailDTO()
        {
        }
        public string email { get; set; }
        public string fullName { get; set; }
        public string title { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string code { get; set; }
        public string testId { get; set; }
    }
}
