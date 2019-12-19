using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("CompanyRankDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CompanyRankDTO
    {
        public int companyRankId { get; set; }
        public string name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool isActive { get; set; }
        [JsonProperty(PropertyName = "value")]
        public int count { get; set; }
        public CompanyRankDTO() { }
        public CompanyRankDTO(CompanyRank rank)
        {
            this.companyRankId = rank.CompanyRankId;
            this.name = rank.Name;
            this.isActive = rank.IsActive;
            this.CreateDate = rank.CreateDate;

        }

        public CompanyRankDTO(int companyRankId, string name, int count)
        {
            this.companyRankId = companyRankId;
            this.name = name;
            this.count = count;
        }
    }
}
