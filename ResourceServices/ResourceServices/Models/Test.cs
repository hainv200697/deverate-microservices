using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class Test
    {
        public Test()
        {
            AccountInTest = new HashSet<AccountInTest>();
            QuestionInTest = new HashSet<QuestionInTest>();
        }

        public int TestId { get; set; }
        public int? ConfigId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? StartTime { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; }
        public bool IsActive { get; set; }

        public virtual Configuration Config { get; set; }
        public virtual ICollection<AccountInTest> AccountInTest { get; set; }
        public virtual ICollection<QuestionInTest> QuestionInTest { get; set; }
    }
}
