using System;
using System.Collections.Generic;

namespace MailingServices.Models
{
    public partial class Semester
    {
        public Semester()
        {
            CatalogueInSemester = new HashSet<CatalogueInSemester>();
            RankInSemester = new HashSet<RankInSemester>();
            Test = new HashSet<Test>();
        }

        public int SemesterId { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public int Duration { get; set; }
        public bool Type { get; set; }
        public bool IsActive { get; set; }
        public int ExpiredDays { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<CatalogueInSemester> CatalogueInSemester { get; set; }
        public virtual ICollection<RankInSemester> RankInSemester { get; set; }
        public virtual ICollection<Test> Test { get; set; }
    }
}
