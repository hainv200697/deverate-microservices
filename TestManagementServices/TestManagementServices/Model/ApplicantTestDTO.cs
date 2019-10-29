using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class ApplicantTestDTO
    {
        public string configId { get; set; }
        public List<ApplicantDTO> applicants { get; set; }
        public ApplicantTestDTO() { }
        public ApplicantTestDTO(string configId, List<ApplicantDTO> applicants)
        {
            this.configId = configId;
            this.applicants = applicants;
        }
    }
}
