using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("RankDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RankDTO
    {
        [JsonProperty("rankId")]
        public int? rankId { get; set; }
        public int? companyId { get; set; }
        public string name { get; set; }
        public DateTime? creatAt { get; set; }
        public DateTime? updateAt { get; set; }
        public bool? isActive { get; set; }

        public RankDTO()
        {

        }

        public RankDTO(CompanyRank rank)
        {
            this.rankId = rank.CompanyRankId;
            this.companyId = rank.CompanyId;    
            this.name = rank.Name;
            this.creatAt = rank.CreateDate;
            this.isActive = rank.IsActive;
        }
    }
}