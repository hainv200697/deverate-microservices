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
        public const string removeCatalogueSucceed = "{\"message\" : \"Change status catalogue success\"}";
        public const string noAvailableEmployee = "No available employee";

        public const string createConfigSucceed = "Creating configuration success";
        public const string updateConfigSucceed = "Updating configuration success";
        public const string removeConfigSucceed = "Removed configuration success";

        public const string createCompanyRankSucceed = "{\"message\" : \"Creating rank success\"}";
        public const string updateCompanyRankSucceed = "{\"message\" : \"Updating rank success\"}";
        public const string changeStatusCompanyRankSucceed = "{\"message\" : \"Change status rank success\"}";

        public const string createDefaultRankSucceed = "{\"message\" : \"Creating rank success\"}";
        public const string DefaultRankExisted = "{\"message\" : \"Rank name is existed\"}";
        public const string updateDefaultRankSucceed = "{\"message\" : \"Updating rank success\"}";
        public const string changeStatusDefaultRankSucceed = "{\"message\" : \"Change status rank success\"}";
        public const string inputDefaultRank = "{\"message\" : \"Please input field required\"}";
        public const string chooseDefaultRank = "{\"message\" : \"Please choose rank\"}";

        public const string updateCompanySucceed = "{\"message\" : \"Updating company success\"}";
        public const string removeCompanySucceed = "{\"message\" : \"Change status company success\"}";

        public const string createQuestionSucceed = "{\"message\" : \"Creating question success\"}";
        public const string updateQuestionSucceed = "{\"message\" : \"Updating question success\"}";
        public const string removeQuestionSucceed = "{\"message\" : \"Removed question success\"}";

        public const string createAnswerSucceed =  "{\"message\" : \"Creating answer success\"}" ;
        public const string updateAnswerSucceed =  "{\"message\" : \"Updating answer success\"}";
        public const string removeAnswerSucceed =  "{\"message\" : \"Removed answer success\"}";
    }
}
