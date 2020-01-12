using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class ListRankAndListCatalogueDTO
    {
        public List<RankDTO> defaultRankDTOs { get; set; }
        public List<CatalogueDefaultDTO> catalogueDefaultDTOs { get; set; }
    }
}
