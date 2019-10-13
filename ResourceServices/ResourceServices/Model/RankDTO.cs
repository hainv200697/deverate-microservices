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
        public string name { get; set; }
        public DateTime? creatAt { get; set; }
        public DateTime? updateAt { get; set; }
        public bool? isActive { get; set; }

        public RankDTO()
        {

        }

        public RankDTO(Rank rank)
        {
            this.rankId = rank.RankId;
            this.name = rank.Name;
            this.creatAt = rank.CreateAt;
            this.updateAt = rank.UpdateAt;
            this.isActive = rank.IsActive;
        }
    }
}