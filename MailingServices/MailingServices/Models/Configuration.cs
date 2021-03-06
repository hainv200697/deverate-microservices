﻿using System;
using System.Collections.Generic;

namespace MailingServices.Models
{
    public partial class Configuration
    {
        public Configuration()
        {
            CatalogueInConfiguration = new HashSet<CatalogueInConfiguration>();
            ConfigurationRank = new HashSet<ConfigurationRank>();
            Test = new HashSet<Test>();
        }

        public int ConfigId { get; set; }
        public int AccountId { get; set; }
        public int TotalQuestion { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Duration { get; set; }
        public bool Type { get; set; }
        public bool IsActive { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
        public virtual ICollection<ConfigurationRank> ConfigurationRank { get; set; }
        public virtual ICollection<Test> Test { get; set; }
    }
}
