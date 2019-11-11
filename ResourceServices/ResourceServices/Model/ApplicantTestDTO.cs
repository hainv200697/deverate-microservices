using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class ApplicantTestDTO
    {
        public int configId { get; set; }
        public List<ApplicantDTO> applicants { get; set; }
        public ApplicantTestDTO() { }
        public ApplicantTestDTO(int configId, List<ApplicantDTO> applicants)
        {
            this.configId = configId;
            this.applicants = applicants;
        }
    }
}
