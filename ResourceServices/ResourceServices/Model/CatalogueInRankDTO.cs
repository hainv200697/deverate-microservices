using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("CatalogueInRankDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CatalogueInRankDTO
    {
        public int? rankId { get; set; }
        public string rank { get; set; }
        public List<CatalogueDTO> catalogues { get; set; }
        public CatalogueDTO catalogue { get; set; }
        public CatalogueInRankDTO() { }
        public CatalogueInRankDTO(int? rankId, string rank, List<CatalogueDTO> catalogues)
        {
            this.rankId = rankId;
            this.rank = rank;
            this.catalogues = catalogues;
        }

        public CatalogueInRankDTO(CatalogueInRank c)
        {
            catalogue = new CatalogueDTO(c.Catalogue, c.WeightPoint);
        }

    }
}
