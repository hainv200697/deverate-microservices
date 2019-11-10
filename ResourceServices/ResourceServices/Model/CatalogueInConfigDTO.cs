﻿using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("CatalogueInConfigDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CatalogueInConfigDTO
    {
        public int? cicId { get; set; }
        public int? configId { get; set; }
        public int? catalogueId { get; set; }
        public string catalogueName { get; set; }
        public double? weightPoint { get; set; }
        public bool? isActive { get; set; }
        public CatalogueInConfigDTO() { }
        public CatalogueInConfigDTO(CatalogueInConfiguration c)
        {
            this.cicId = c.Cicid;
            this.configId = c.ConfigId;
            this.catalogueId = c.CatalogueId;
            this.catalogueName = c.Catalogue.Name;
            this.weightPoint = c.WeightPoint;
            this.isActive = c.IsActive;
        }
    }
}