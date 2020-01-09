using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class CompanyRankDTO
    {
        public int rankId { get; set; }
        public int companyId { get; set; }
        public string name { get; set; }
        public DateTime creatDate { get; set; }
        public bool isActive { get; set; }
        public List<CatalogueInRankDTO> catalogueInRanks;

        public CompanyRankDTO()
        {

        }

        public CompanyRankDTO(Rank rank)
        {
            this.rankId = rank.RankId;
            this.companyId = rank.CompanyId.Value;    
            this.name = rank.Name;
            this.creatDate = rank.CreateDate;
            this.isActive = rank.IsActive;
            this.catalogueInRanks = rank.CatalogueInRank.Select(x => new CatalogueInRankDTO(x)).OrderBy(x => x.catalogueId).ToList();
        }
    }
}