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
        public int? rankId { get; set; }
        public string name { get; set; }
        public DateTime? create_at { get; set; }
        public DateTime? update_at { get; set; }
        public bool? isActive { get; set; }
        public int? count { get; set; }
        public RankDTO() { }
        public RankDTO(Rank rank)
        {
            this.rankId = rank.RankId;
            this.name = rank.Name;
            this.isActive = rank.IsActive;
            this.create_at = rank.CreateAt;
            this.update_at = rank.UpdateAt;
        }

        public RankDTO(int? rankId, string name, int? count)
        {
            this.rankId = rankId;
            this.name = name;
            this.count = count;
        }
    }
}
