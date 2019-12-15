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
        public double point { get; set; }

        public ConfigurationRankDTO() { }
        public ConfigurationRankDTO(int companyRankId, double weightPoint)
        {
            this.companyRankId = companyRankId;
            this.point = weightPoint;

        }
    }
}
