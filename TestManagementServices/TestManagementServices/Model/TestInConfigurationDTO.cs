using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("TestInConfigurationDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class TestInConfigurationDTO
    {
        public int? configId { get; set; }
        public List<Test> tests { get; set; }
        public TestInConfigurationDTO() { }
        public TestInConfigurationDTO(int? configId, List<Test> tests)
        {
            this.configId = configId;
            this.tests = tests;
        }
    }
}
