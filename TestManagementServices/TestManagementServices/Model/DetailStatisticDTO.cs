using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    public class DetailStatisticDTO
    {
        public int? detailId { get; set; }
        public int? statisticId { get; set; }
        public int? catalogueId { get; set; }
        public double? point { get; set; }
        public bool? isActive { get; set; }

        public DetailStatisticDTO() { }
        public DetailStatisticDTO(DetailStatistic detail)
        {
            this.detailId = detail.DetailId;
            this.statisticId = detail.StatisticId;
            this.catalogueId = detail.CatalogueId;
            this.point = detail.Point;
            this.isActive = detail.IsActive;
        }
    }
}
