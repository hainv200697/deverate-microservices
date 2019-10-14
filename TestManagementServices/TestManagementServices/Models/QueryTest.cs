using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Models
{
    public class QueryTest
    {
        public int accountId { get; set; }
        public int configId { get; set; }
        public string code { get; set; }

        public QueryTest(int accountId, int configId, string code)
        {
            this.accountId = accountId;
            this.configId = configId;
            this.code = code;
        }
    }
}
