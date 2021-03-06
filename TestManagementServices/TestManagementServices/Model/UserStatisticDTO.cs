﻿using Newtonsoft.Json;
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
        public string rank { get; set; }
        public string potentialRank { get; set; }
        public double overallPoint { get; set; }
        public string configName { get; set; }
        public DateTime? configCreateDate { get; set; }
        public int testId { get; set; }
        public UserStatisticDTO() { }

        public UserStatisticDTO(int? accountId, string username, DateTime? startDate, string rank, string potentialRank, double overallPoint, string configName, int testId)
        {
            this.accountId = accountId;
            this.username = username;
            this.startDate = startDate;
            this.rank = rank;
            this.potentialRank = potentialRank;
            this.overallPoint = overallPoint;
            this.configName = configName;
            this.configCreateDate = configCreateDate;
            this.testId = testId;
        }
    }
}
