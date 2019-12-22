using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("CompanyRankDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CompanyRankDTO
    {
        [JsonProperty("companyRankId")]
        public int companyRankId { get; set; }
        public int companyId { get; set; }
        public string name { get; set; }
        public DateTime creatAt { get; set; }
        public int position { get; set; }
        public bool isActive { get; set; }

        public CompanyRankDTO()
        {

        }

        public CompanyRankDTO(CompanyRank rank)
        {
            this.companyRankId = rank.CompanyRankId;
            this.companyId = rank.CompanyId;    
            this.name = rank.Name;
            this.creatAt = rank.CreateDate;
            this.isActive = rank.IsActive;
            this.position = rank.Position;
        }
    }
}