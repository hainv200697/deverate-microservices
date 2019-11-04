using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    [JsonObject("UserStatisticDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UserStatisticDTO
    {
        public int? accountId { get; set; }
        public string username { get; set; }
        public DateTime? startDate { get; set; }
        public double? overallPoint { get; set; }
        public string configName { get; set; }
        public DateTime? configCreateDate { get; set; }
        public UserStatisticDTO() { }
        public UserStatisticDTO(int? accountId, string username, DateTime? startDate, double? overallPoint, string configName, DateTime? configCreateDate)
        {
            this.accountId = accountId;
            this.username = username;
            this.startDate = startDate;
            this.overallPoint = overallPoint;
            this.configName = configName;
            this.configCreateDate = configCreateDate;
        }
    }
}
