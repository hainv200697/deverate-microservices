using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("RankDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RankDTO
    {
        public int? companyRankId { get; set; }
        public string name { get; set; }
        public DateTime? create_at { get; set; }
        public bool? isActive { get; set; }
        [JsonProperty(PropertyName = "value")]
        public int? count { get; set; }
        public RankDTO() { }
        public RankDTO(CompanyRank rank)
        {
            this.companyRankId = rank.CompanyRankId;
            this.name = rank.Name;
            this.isActive = rank.IsActive;
            this.create_at = rank.CreateDate;

        }

        public RankDTO(int? companyRankId, string name, int? count)
        {
            this.companyRankId = companyRankId;
            this.name = name;
            this.count = count;
        }
    }
}
