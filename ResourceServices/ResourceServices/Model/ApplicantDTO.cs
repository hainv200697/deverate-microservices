using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("ApplicantDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApplicantDTO
    {
        [JsonProperty("ApplicantId")]
        public int? applicantId { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public bool isActive { get; set; }

        public ApplicantDTO()
        {
        }
        public ApplicantDTO(Applicant app)
        {
            this.applicantId = app.ApplicantId;
            this.fullname = app.Fullname;
            this.email = app.Email;
            this.isActive = app.IsActive;
        }
    }
}
