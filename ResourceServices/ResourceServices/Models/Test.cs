using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class Test
    {
        public Test()
        {
            DetailResult = new HashSet<DetailResult>();
            QuestionInTest = new HashSet<QuestionInTest>();
        }

        public int TestId { get; set; }
        public int SemesterId { get; set; }
        public int? AccountId { get; set; }
        public int? ApplicantId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public int? PotentialRankId { get; set; }
        public int? RankId { get; set; }
        public double? Point { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApprove { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Account Account { get; set; }
        public virtual Applicant Applicant { get; set; }
        public virtual Rank PotentialRank { get; set; }
        public virtual Rank Rank { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ICollection<DetailResult> DetailResult { get; set; }
        public virtual ICollection<QuestionInTest> QuestionInTest { get; set; }
    }
}
