using System;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    public class ApplicantResult
    {
        public string fullName { get; set; }
        public string rank { get; set; }
        public double point { get; set; }
        public string email { get; set; }

        public ApplicantResult(Test t)
        {
            fullName = t.Applicant.Fullname;
            if (t.Rank == null)
            {
                rank = "Not Ranked";
            } else
            {
                rank = t.Rank.Name;
            }
            point = t.Point.Value;
            email = t.Applicant.Email;
        }
    }
}
