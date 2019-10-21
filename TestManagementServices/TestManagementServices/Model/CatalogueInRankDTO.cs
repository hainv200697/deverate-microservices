using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class CatalogueInRankDTO
    {
        public int? rankId { get; set; }
        public string rank { get; set; }
        public List<CatalogueDTO> catalogues { get; set; }
        public CatalogueInRankDTO() { }
        public CatalogueInRankDTO(int? rankId, string rank, List<CatalogueDTO> catalogues)
        {
            this.rankId = rankId;
            this.rank = rank;
            this.catalogues = catalogues;
        }
    }
}
