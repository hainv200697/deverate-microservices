﻿using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class Question
    {
        public Question()
        {
            Answer = new HashSet<Answer>();
            QuestionInTest = new HashSet<QuestionInTest>();
        }

        public int QuestionId { get; set; }
        public int CatalogueId { get; set; }
        public string Question1 { get; set; }
        public double Point { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual ICollection<Answer> Answer { get; set; }
        public virtual ICollection<QuestionInTest> QuestionInTest { get; set; }
    }
}
