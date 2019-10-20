using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Models
{
    public class TestInfoDTO
    {
        public int? configId { get; set; }
        public int? accountId { get; set; }
        public int? testId { get; set; }
        public string title { get; set; }
        public string code { get; set; }

        public TestInfoDTO(int? configId, int? accountId, int? testId, string title, string code)
        {
            this.configId = configId;
            this.accountId = accountId;
            this.testId = testId;
            this.title = title;
            this.code = code;
        }
    }
}
