using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class CatalogueInRankDTO
    {
        public int rankId { get; set; }
        public int catalogueId { get; set; }
        public double point { get; set; }
        public bool isActive { get; set; }
        public CatalogueInRankDTO() { }

        public CatalogueInRankDTO(CatalogueInRank catalogueInRank)
        {
            this.rankId = catalogueInRank.RankId;
            this.catalogueId = catalogueInRank.CatalogueId;
            this.point = catalogueInRank.Point;
            this.isActive = catalogueInRank.IsActive;
        }
    }
}
