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
        public int companyRankId { get; set; }
        public string name { get; set; }
        public double point { get; set; }
        public DateTime CreateDate { get; set; }
        public bool isActive { get; set; }
        [JsonProperty(PropertyName = "value")]
        public int count { get; set; }
        public RankDTO() { }
        public RankDTO(Rank rank)
        {
            this.companyRankId = rank.RankId;
            this.name = rank.Name;
            this.isActive = rank.IsActive;
            this.CreateDate = rank.CreateDate;

        }

        public RankDTO(Rank rank, double point)
        {
            this.companyRankId = rank.RankId;
            this.name = rank.Name;
            this.isActive = rank.IsActive;
            this.CreateDate = rank.CreateDate;
            this.point = point;

        }

        public RankDTO(int companyRankId, string name, int count, double point)
        {
            this.companyRankId = companyRankId;
            this.name = name;
            this.count = count;
            this.point = point;
        }
    }
}
