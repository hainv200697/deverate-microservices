﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Model;

namespace TestManagementServices.Models
{
    public class CandidateResultDTO
    {
        public int? accountId { get; set; } 
        public List<ConfigurationRankDTO> configurationRanks { get; set; }
        public List<CatalogueInRankDTO> catalogueInRanks { get; set; }
        public List<CatalogueDTO> catalogues { get; set; }
        public double? point { get; set; }
        public int? rankId { get; set; }
        public string rank { get; set; }

        public CandidateResultDTO() { }

        public CandidateResultDTO(int? accountId, List<ConfigurationRankDTO> configurationRanks, List<CatalogueDTO> catalogues, List<CatalogueInRankDTO> catalogueInRanks, double? point, int? rankId, string rank)
        {
            this.accountId = accountId;
            this.configurationRanks = configurationRanks;
            this.catalogues = catalogues;
            this.catalogueInRanks = catalogueInRanks;
            this.point = point;
            this.rankId = rankId;
            this.rank = rank;
        }


    }


}