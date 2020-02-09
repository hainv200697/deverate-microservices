using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class RankDTO
    {
        public int rankId { get; set; }
        public string name { get; set; }
        public DateTime createDate { get; set; }
        public bool isActive { get; set; }
        public bool isDefault { get; set; }
        public List<CatalogueInRankDTO> catalogueInRanks { get; set; }

        public RankDTO()
        {

        }
        public RankDTO(Rank defaultRank)
        {
            this.rankId = defaultRank.RankId;
            this.name = defaultRank.Name;
            this.createDate = defaultRank.CreateDate;
            this.isActive = defaultRank.IsActive;
            this.isDefault = defaultRank.IsDefault;
            this.catalogueInRanks = defaultRank.CatalogueInRank.Where(x => x.Catalogue.IsActive).Select(x => new CatalogueInRankDTO(x)).OrderBy(x => x.catalogueId).ToList();
        }
    }
}
