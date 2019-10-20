using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class Message
    {
        public const string createCatalogueSucceed = "{\"message\" : \"Creating catalogue success\"}";
        public const string updateCatalogueSucceed = "{\"message\" : \"Updating catalogue success\"}";
        public const string removeCatalogueSucceed = "{\"message\" : \"Removed catalogue success\"}";
        public const string noAvailableEmployee = "No available employee";

        public const string createConfigSucceed = "Creating configuration success";
        public const string updateConfigSucceed = "Updating configuration success";
        public const string removeConfigSucceed = "Removed configuration success";

        public const string createQuestionSucceed = "{\"message\" : \"Creating question success\"}";
        public const string updateQuestionSucceed = "{\"message\" : \"Updating question success\"}";
        public const string removeQuestionSucceed = "{\"message\" : \"Removed question success\"}";

        public const string createAnswerSucceed =  "{\"message\" : \"Creating answer success\"}" ;
        public const string updateAnswerSucceed =  "{\"message\" : \"Updating answer success\"}";
        public const string removeAnswerSucceed =  "{\"message\" : \"Removed answer success\"}";
    }
}
