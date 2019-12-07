using System;
using System.Collections.Generic;

namespace MailingServices.Models
{
    public partial class Applicant
    {
        public Applicant()
        {
            Test = new HashSet<Test>();
        }

        public int ApplicantId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Test> Test { get; set; }
    }
}
