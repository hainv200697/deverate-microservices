﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    public class ConfigurationDTO
    {
        public int? configId { get; set; }
        public int? testOwnerId { get; set; }
        public string title { get; set; }
        public int? accountId { get; set; }
        public int? applicantId { get; set; }
        public int? totalQuestion { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? duration { get; set; }
        public bool? isActive { get; set; }
        public bool? status { get; set; }

        public ConfigurationDTO() { }
        public ConfigurationDTO(Configuration config,int? accountId,int? applicantId, bool? status)
        {
            this.title = config.Title;
            this.configId = config.ConfigId;
            this.testOwnerId = config.TestOwnerId;
            this.totalQuestion = config.TotalQuestion;
            this.createDate = config.CreateDate;
            this.startDate = config.StartDate;
            this.endDate = config.EndDate;
            this.duration = config.Duration;
            this.isActive = config.IsActive;
            this.accountId = accountId;
            this.applicantId = applicantId;
            this.status = status;
        }

    }
}
