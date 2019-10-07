using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class CatalogueWeightPointDTO
    {
        public int? catalogueId { get; set; }
        public double? weightPoint { get; set; }
        public CatalogueWeightPointDTO() { }
        public CatalogueWeightPointDTO(int? catalogueId, double? weightPoint)
        {
            this.catalogueId = catalogueId;
            this.weightPoint = weightPoint;
        }
    }
}
