﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    [JsonObject("GeneralStatisticItemDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GeneralStatisticItemDTO
    {
        public int? configId { get; set; }
        public List<CompanyCatalogueDTO> series { get; set; }
        public double? configGPA { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? endDate { get; set; }
        public string name { get; set; }
        public int? numberOfFinishedTest { get; set; }
        public int? totalTest { get; set; }

        public GeneralStatisticItemDTO() { }
        public GeneralStatisticItemDTO(int? configId, List<CompanyCatalogueDTO> catalogues, double? configGPA,
            DateTime? createDate, DateTime? endDate, string title, int? numberOfFinishedTest, int? totalTest)
        {
            this.configId = configId;
            this.series = catalogues;
            this.configGPA = configGPA;
            this.createDate = createDate;
            this.endDate = endDate;
            this.name = title;
            this.numberOfFinishedTest = numberOfFinishedTest;
            this.totalTest = totalTest;
        }

    }
}
