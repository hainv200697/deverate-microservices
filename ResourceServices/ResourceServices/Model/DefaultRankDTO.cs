using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class DefaultRankDTO
    {
        public int rankId { get; set; }
        public string name { get; set; }
        public DateTime createDate { get; set; }
        public bool isActive { get; set; }
        public bool isDefault { get; set; }
        public List<CatalogueInRankDTO> catalogueInRankDTOs;

        public DefaultRankDTO()
        {

        }
        public DefaultRankDTO(Rank defaultRank)
        {
            this.rankId = defaultRank.RankId;
            this.name = defaultRank.Name;
            this.createDate = defaultRank.CreateDate;
            this.isActive = defaultRank.IsActive;
            this.isDefault = defaultRank.IsDefault;
            this.catalogueInRankDTOs = defaultRank.CatalogueInRank.Select(x => new CatalogueInRankDTO(x)).ToList();        }
    }
}
