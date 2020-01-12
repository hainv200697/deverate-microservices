using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class ListRankAndListCatalogueDTO
    {
        public List<RankDTO> RankDTOs { get; set; }
        public List<CatalogueDTO> catalogueDTOs { get; set; }
    }
}
