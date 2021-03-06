﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("ApplicantDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApplicantDTO
    {
        public int applicantId { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public bool isActive { get; set; }

        public ApplicantDTO() { }
        public ApplicantDTO(Applicant applicant) {
            this.applicantId = applicant.ApplicantId;
            this.fullName = applicant.Fullname;
            this.email = applicant.Email;
        }
        public ApplicantDTO(int applicantId, string fullName, string email, bool isActive)
        {
            this.applicantId = applicantId;
            this.fullName = fullName;
            this.email = email;
            this.isActive = isActive;
        }
    }
}
