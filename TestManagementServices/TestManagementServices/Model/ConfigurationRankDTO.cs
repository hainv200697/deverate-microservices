using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    public class ConfigurationRankDTO
    {
        public int companyRankId { get; set; }
        public string rank { get; set; }
        public double point { get; set; }

        public ConfigurationRankDTO() { }
        public ConfigurationRankDTO(int companyRankId, string rank,  double weightPoint)
        {
            this.companyRankId = companyRankId;
            this.rank = rank;
            this.point = weightPoint;

        }
    }
}
