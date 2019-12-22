using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class Test
    {
        public Test()
        {
            DetailResult = new HashSet<DetailResult>();
            QuestionInTest = new HashSet<QuestionInTest>();
        }

        public int TestId { get; set; }
        public int ConfigId { get; set; }
        public int? AccountId { get; set; }
        public int? ApplicantId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public int? PotentialRankId { get; set; }
        public int? CompanyRankId { get; set; }
        public double? Point { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }

        public virtual Account Account { get; set; }
        public virtual Applicant Applicant { get; set; }
        public virtual CompanyRank CompanyRank { get; set; }
        public virtual Configuration Config { get; set; }
        public virtual CompanyRank PotentialRank { get; set; }
        public virtual ICollection<DetailResult> DetailResult { get; set; }
        public virtual ICollection<QuestionInTest> QuestionInTest { get; set; }
    }
}
