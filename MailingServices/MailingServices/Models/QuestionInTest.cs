using System;
using System.Collections.Generic;

namespace MailingServices.Models
{
    public partial class QuestionInTest
    {
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public bool IsActive { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Question Question { get; set; }
        public virtual Test Test { get; set; }
    }
}
