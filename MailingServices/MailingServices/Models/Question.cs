using System;
using System.Collections.Generic;

namespace MailingServices.Models
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
        public string QuestionText { get; set; }
        public double Point { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual ICollection<Answer> Answer { get; set; }
        public virtual ICollection<QuestionInTest> QuestionInTest { get; set; }
    }
}
