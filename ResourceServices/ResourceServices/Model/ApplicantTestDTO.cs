using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class ApplicantTestDTO
    {
        public int configId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public List<ApplicantDTO> applicants { get; set; }
        public ApplicantTestDTO() { }
        public ApplicantTestDTO(int configId, List<ApplicantDTO> applicants, DateTime startDate, DateTime endDate)
        {
            this.configId = configId;
            this.applicants = applicants;
            this.startDate = startDate;
            this.endDate = endDate;
        }
    }
}
