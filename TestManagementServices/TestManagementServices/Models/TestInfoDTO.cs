﻿using System;
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
        public string accountName { get; set; }
        public string applicantName { get; set; }
        public string status { get; set; }
        public bool isActive { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int expiredDays { get; set; }
        public TestInfoDTO()
        {

        }

        public TestInfoDTO(int? configId, int? accountId, int? testId, string title, string code,string status,DateTime start, DateTime end, int expiredDays, bool isActive)
        {
            this.configId = configId;
            this.accountId = accountId;
            this.testId = testId;
            this.title = title;
            this.code = code;
            this.status = status;
            this.startDate = start;
            this.endDate = end;
            this.expiredDays = expiredDays;
            this.isActive = isActive;
        }

        public TestInfoDTO(Test test, string configTitle, string name, string applicantName)
        {
            this.configId = test.SemesterId;
            this.accountId = test.AccountId;
            this.testId = test.TestId;
            this.title = configTitle;
            this.accountName = name;
            this.applicantName = applicantName;
            this.status = test.Status;
            this.isActive = test.IsActive;
        }

        public TestInfoDTO(Test test)
        {
            this.configId = test.SemesterId;
            this.accountId = test.AccountId;
            this.testId = test.TestId;
            this.code = test.Code;
            this.isActive = test.IsActive;
        }
    }
}
