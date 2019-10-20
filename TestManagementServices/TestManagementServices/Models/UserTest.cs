using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public class UserTest
    {
        public int accountId { get; set; }
        public int testId { get; set; }
        public string code { get; set; }
        public List<QuestionInTestDTO> questionInTest { get; set; }
    }
}
