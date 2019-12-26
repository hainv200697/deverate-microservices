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
        public int catalogueInConfigId { get; set; }
        public int companyRankId { get; set; }
        public double point { get; set; }
        public bool isActive { get; set; }
        public string name { get; set; }
        public CatalogueInRankDTO() { }

        public CatalogueInRankDTO(CatalogueInRank catalogueInRank)
        {
            this.catalogueInConfigId = catalogueInRank.CatalogueInConfigId;
            this.companyRankId = catalogueInRank.CompanyRankId;
            this.point = catalogueInRank.Point;
            this.name = catalogueInRank.CompanyRank.Name;
            this.isActive = catalogueInRank.IsActive;
        }
    }
}
