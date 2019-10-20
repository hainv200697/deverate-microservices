using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class Message
    {
        public const string evaluateSucceed = "Evaluate successful";
        public const string evaluateFailed = "Evaluate Failed";
        public const string createSucceed = "Create Successfully";

        public const string durationExceptopn = "Test duration at least 5 minutes";
        public const string noCatalogueException = "Can't generate test because there's no catalogue chose";
        public const string numberQuestionExceptopn = "Number of questions equal or greater than number of catalogue";
        public const string noEmployeeException = "There'is no available employee";

        public const string sendMailSucceed = "Send test mail succeed";
    }
}
