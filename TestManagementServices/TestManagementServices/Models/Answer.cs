﻿using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class Answer
    {
        public Answer()
        {
            QuestionInTest = new HashSet<QuestionInTest>();
        }

        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string Answer1 { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<QuestionInTest> QuestionInTest { get; set; }
    }
}
