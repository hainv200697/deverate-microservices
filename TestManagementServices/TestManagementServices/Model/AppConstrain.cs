using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class AppConstrain
    {

        public const string SUBMITTED = "Submitted";
        public const string EXPIRED = "Expired";
        public const string PENDING = "Pending";
        public const string DOING = "Doing";
        public const string UNKNOWN_RANK = "Not ranked";

        public const string logFile = "bug.txt";

        public const bool includeLowercase = true;
        public const bool includeUppercase = true;
        public const bool includeNumeric = true;
        public const bool includeSpecial = true;
        public const bool includeSpaces = false;
        public const int lengthOfPassword = 8;
        public const string GEN_TEST_CONSUMER = "GenerateTest";
        public const string GEN_TEST_APPLICANT = "GenerateApplicantTest";
        public const string TEST_MAIL = "TestEmployeeToEmail";

        public const string APPLICANT_DO_TEST = "Applicants did test";
        public const string TOTAL_APPLICANT_DO_TEST = "Total applicants";

        public const int MIN_DURATION = 5;

        public const int EMP_ROLE = 3;

        public const int SCALE_UP_NUMB = 100;
        public const int SCALE_DOWN_NUMB = 1;

        public const string HOSTNAME = "34.87.45.18";

        public static double RoundDownNumber(double value, int scaleUp)
        {

            double rNumb = Math.Round(value * scaleUp, 1);
            return rNumb > AppConstrain.SCALE_UP_NUMB ? AppConstrain.SCALE_UP_NUMB : rNumb;
        }

        public static string GenerateCode()
        {
            string code = PasswordGenerator.GeneratePassword(AppConstrain.includeLowercase, AppConstrain.includeUppercase,
                AppConstrain.includeNumeric, AppConstrain.includeSpecial,
                AppConstrain.includeSpaces, AppConstrain.lengthOfPassword);

            while (!PasswordGenerator.PasswordIsValid(AppConstrain.includeLowercase, AppConstrain.includeUppercase,
                AppConstrain.includeNumeric, AppConstrain.includeSpecial, AppConstrain.includeSpaces, code))
            {
                code = PasswordGenerator.GeneratePassword(AppConstrain.includeLowercase, AppConstrain.includeUppercase,
                    AppConstrain.includeNumeric, AppConstrain.includeSpecial,
                    AppConstrain.includeSpaces, AppConstrain.lengthOfPassword);
            }
            return code;
        }
    }
}
