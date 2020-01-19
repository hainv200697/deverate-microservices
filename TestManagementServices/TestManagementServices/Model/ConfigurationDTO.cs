using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    public class ConfigurationDTO
    {
        public int configId { get; set; }
        public int companyId { get; set; }
        public string title { get; set; }
        public int? accountId { get; set; }
        public int? applicantId { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? startDate { get; set; }
        public int expỉredDays { get; set; }
        public int duration { get; set; }
        public int timeRemaining { get; set; }
        public bool isActive { get; set; }
        public string status { get; set; }

        public ConfigurationDTO() { }
        public ConfigurationDTO(Configuration config, Test test)
        {
            this.title = config.Title;
            this.configId = config.ConfigId;
            this.companyId = config.CompanyId;
            this.createDate = config.CreateDate;
            this.startDate = config.CreateDate;
            this.expỉredDays = config.ExpiredDays;
            this.duration = config.Duration;
            this.isActive = config.IsActive;
            this.accountId = test.AccountId;
            this.applicantId = test.ApplicantId;
            this.status = test.Status;
            if (test.StartTime == null)
            {
                this.timeRemaining = config.Duration * 60;
            }
            else {
                if (test.Status == "Submitted" || test.Status == "Expired")
                {
                    this.timeRemaining = 0;
                } else
                {
                    this.timeRemaining = (int)Math.Floor(test.StartTime.Value.AddMinutes(config.Duration).Subtract(DateTime.UtcNow).TotalSeconds);
                }
            }
        }

    }
}
