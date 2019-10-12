using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class Test
    {
        public Test()
        {
            AccountInTest = new HashSet<AccountInTest>();
            Configuration = new HashSet<Configuration>();
            QuestionInTest = new HashSet<QuestionInTest>();
        }

        public int TestId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? StartTime { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<AccountInTest> AccountInTest { get; set; }
        public virtual ICollection<Configuration> Configuration { get; set; }
        public virtual ICollection<QuestionInTest> QuestionInTest { get; set; }
    }
}
