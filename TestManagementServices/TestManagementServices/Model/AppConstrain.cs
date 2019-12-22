using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class AppConstrain
    {

        public const string SUBMITTED = "Submitted";
        public const string UNKNOWN_RANK = "Unknown rank";

        public const string logFile = "bug.txt";

        public const bool includeLowercase = true;
        public const bool includeUppercase = true;
        public const bool includeNumeric = true;
        public const bool includeSpecial = true;
        public const bool includeSpaces = false;
        public const int lengthOfPassword = 8;
        public const string gen_test_consumer = "GenerateTest";
        public const string gen_test_applicant = "GenerateApplicantTest";
        public const string test_mail = "TestEmployeeToEmail";

        public const string applicantDoTest = "Applicants did test";
        public const string totalApplicantDoTest = "Total applicants";

        public const int minDuration = 5;

        public const int empRole = 3;

        public const int scaleUpNumb = 100;
        public const int scaleDownNumb = 1;

        public const string hostname = "35.198.215.101";

        public static double RoundDownNumber(double value, int scaleUp)
        {

            double rNumb = Math.Round(value * scaleUp, 1);
            return rNumb > AppConstrain.scaleUpNumb ? AppConstrain.scaleUpNumb : rNumb;
        }


    }
}
