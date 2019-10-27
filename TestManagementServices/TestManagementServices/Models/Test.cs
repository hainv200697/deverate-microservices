using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class Test
    {
        public Test()
        {
            QuestionInTest = new HashSet<QuestionInTest>();
            Statistic = new HashSet<Statistic>();
        }

        public int TestId { get; set; }
        public int ConfigId { get; set; }
        public int? AccountId { get; set; }
        public int? ApplicantId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; }
        public bool IsActive { get; set; }

        public virtual Account Account { get; set; }
        public virtual Applicant Applicant { get; set; }
        public virtual Configuration Config { get; set; }
        public virtual ICollection<QuestionInTest> QuestionInTest { get; set; }
        public virtual ICollection<Statistic> Statistic { get; set; }
    }
}
