﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("CatalogueInConfigDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CatalogueInConfigDTO
    {
        public int catalogueInConfigId { get; set; }
        public int configId { get; set; }
        public int catalogueId { get; set; }
        public string catalogueName { get; set; }
        public double weightPoint { get; set; }
        public bool isActive { get; set; }
        public CatalogueInConfigDTO() { }
        public CatalogueInConfigDTO(CatalogueInSemester c)
        {
            this.catalogueInConfigId = c.CatalogueInSemesterId;
            this.configId = c.SemesterId;
            this.catalogueId = c.CatalogueId;
            this.catalogueName = c.Catalogue.Name;
            this.weightPoint = c.WeightPoint;
            this.isActive = c.IsActive;
        }
    }
}
