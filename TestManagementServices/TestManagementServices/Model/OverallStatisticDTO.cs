using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    [JsonObject("OverallStatisticDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class OverallStatisticDTO
    {
        public int? companyId { get; set; }
        public List<int?> configIds { get; set; }
        public bool? isEmployee { get; set; }
        public OverallStatisticDTO() { }
        public OverallStatisticDTO(int? companyId, List<int?> configIds, bool? isEmployee)
        {
            this.companyId = companyId;
            this.configIds = configIds;
            this.isEmployee = isEmployee;
        }
    }
}
