using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class EmployeeTestDTO
    {
        public List<int> accountIds { get; set; }
        public string configId { get; set; }
        public bool oneForAll {get;set;}
        public EmployeeTestDTO() { }
        public EmployeeTestDTO(List<int> accountIds, string configId, bool oneForAll = false)
        {
            this.accountIds = accountIds;
            this.configId = configId;
            this.oneForAll = oneForAll;
        }
    }
}
