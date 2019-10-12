using ResourceServices.Model;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Service
{
    public class ConfigurationDAO
    {
        DeverateContext context;

        public ConfigurationDAO(DeverateContext context)
        {
            this.context = context;
        }


        public static List<ConfigurationDTO> GetAllConfiguration(bool isActive)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var configuration = from con in db.Configuration
                                    where con.IsActive == isActive
                                    select new ConfigurationDTO(con, con.CatalogueInConfiguration.ToList(), con.ConfigurationRank.ToList());
                return configuration.ToList();
            }
        }

        public static string CreateConfiguration(ConfigurationDTO configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Configuration configuration = new Configuration();
                configuration.ConfigId = configurationDTO.ConfigId;
                configuration.TestOwnerId = configurationDTO.TestOwnerId;
                configuration.TotalQuestion = configurationDTO.TotalQuestion;
                configuration.CreateDate = configurationDTO.CreateDate;
                configuration.StartDate = configurationDTO.StartDate;
                configuration.EndDate = configurationDTO.EndDate;
                configuration.Duration = configurationDTO.Duration;
                configuration.IsActive = true;
                db.Configuration.Add(configuration);
                db.SaveChanges();
                
                foreach (var item in configurationDTO.catalogueInConfigurations)
                {
                    CatalogueInConfiguration catalogueInConfiguration = new CatalogueInConfiguration();
                    catalogueInConfiguration.ConfigId = configuration.ConfigId;
                    catalogueInConfiguration.CatalogueId = item.CatalogueId;
                    catalogueInConfiguration.WeightPoint = item.WeightPoint;
                    catalogueInConfiguration.IsActive = item.IsActive;
                    db.CatalogueInConfiguration.Add(catalogueInConfiguration);
                    db.SaveChanges();
                }
                
                foreach (var item in configurationDTO.ConfigurationRank)
                {
                    ConfigurationRank configurationRank = new ConfigurationRank();
                    configurationRank.ConfigId = configuration.ConfigId;
                    configurationRank.RankId = item.RankId;
                    configurationRank.WeightPoint = item.WeightPoint;
                    configurationRank.IsActive = item.IsActive;
                    db.ConfigurationRank.Add(configurationRank);
                    db.SaveChanges();
                }                     
                
                return null;
            }
        }
    }
}
