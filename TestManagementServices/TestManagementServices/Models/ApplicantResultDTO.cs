using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Model;

namespace TestManagementServices.Models
{
    public class ApplicantResultDTO
    {
        public int? accountId { get; set; } 
        public List<CatalogueDTO> catalogues { get; set; }
        public double? point { get; set; }
        public string rank { get; set; }

        public ApplicantResultDTO() { }

        public ApplicantResultDTO(int? accountId, List<CatalogueDTO> catalogues, double? point, string rank)
        {
            this.accountId = accountId;
            this.catalogues = catalogues;
            this.point = point;
            this.rank = rank;
        }


    }
}
