using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class QuestionInTest
    {
        public int Qitid { get; set; }
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public bool IsActive { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Question Question { get; set; }
        public virtual Test Test { get; set; }
    }
}
