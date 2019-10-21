using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class CatalogueInAllRankDTO
    {
        public int? accountId { get; set; }
        public List<CatalogueInRankDTO> cirs { get; set; }
        public double? point { get; set; }
        public string rank { get; set; }

        public CatalogueInAllRankDTO() { }

        public CatalogueInAllRankDTO(int? accountId, List<CatalogueInRankDTO> cirs, double? point, string rank)
        {
            this.accountId = accountId;
            this.cirs = cirs;
            this.point = point;
            this.rank = rank;
        }

    }
}
