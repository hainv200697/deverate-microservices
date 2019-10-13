using AuthenServices.Model;
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
                var result = from a in db.Account
                                 where a.AccountId == configurationDTO.TestOwnerId
                                 select a.CompanyId;
                int? companyId = result.First();
                var emps = from a in db.Account
                           where a.CompanyId == companyId && a.IsActive == true && a.RoleId == 3
                           select new AccountDTO(a);
                if(emps.ToList().Count == 0)
                {
                    return "No available employee";
                }

                Configuration configuration = new Configuration();
                configuration.ConfigId = configurationDTO.configId;
                configuration.TestOwnerId = configurationDTO.TestOwnerId;
                configuration.TotalQuestion = configurationDTO.TotalQuestion;
                configuration.CreateDate = DateTime.Now;
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
                
                return "Create configuration success";
            }
        }

        public static ConfigurationDTO GetConfigurationById(int id)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var configuration = from con in db.Configuration
                                    where con.ConfigId == id
                                    select new ConfigurationDTO(con, con.CatalogueInConfiguration.ToList(), con.ConfigurationRank.ToList());
                return configuration.FirstOrDefault();
            }
        }

        public static string UpdateConfiguration(ConfigurationDTO configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Configuration configuration = db.Configuration.SingleOrDefault(con => con.ConfigId == configurationDTO.configId);
                configuration.ConfigId = configurationDTO.configId;
                configuration.TestOwnerId = configurationDTO.TestOwnerId;
                configuration.TotalQuestion = configurationDTO.TotalQuestion;
                configuration.CreateDate = DateTime.Now;
                configuration.StartDate = configurationDTO.StartDate;
                configuration.EndDate = configurationDTO.EndDate;
                configuration.Duration = configurationDTO.Duration;
                configuration.IsActive = true;
                db.Configuration.Update(configuration);
                db.SaveChanges();

                foreach (var item in configurationDTO.catalogueInConfigurations)
                {
                    CatalogueInConfiguration catalogueInConfiguration = db.CatalogueInConfiguration.SingleOrDefault(con => con.Cicid == item.Cicid);
                    catalogueInConfiguration.ConfigId = configuration.ConfigId;
                    catalogueInConfiguration.CatalogueId = item.CatalogueId;
                    catalogueInConfiguration.WeightPoint = item.WeightPoint;
                    catalogueInConfiguration.IsActive = item.IsActive;
                    db.SaveChanges();
                }

                foreach (var item in configurationDTO.ConfigurationRank)
                {
                    ConfigurationRank configurationRank = db.ConfigurationRank.SingleOrDefault(con => con.ConfigurationRankId == item.ConfigurationRankId);
                    configurationRank.ConfigId = configuration.ConfigId;
                    configurationRank.RankId = item.RankId;
                    configurationRank.WeightPoint = item.WeightPoint;
                    configurationRank.IsActive = item.IsActive;
                    db.ConfigurationRank.Update(configurationRank);
                    db.SaveChanges();
                }

                return null;
            }
        }

        public static string ChangeStatusConfiguration(List<ConfigurationDTO> configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                foreach (var item in configurationDTO)
                { 
                Configuration configuration = db.Configuration.SingleOrDefault(con => con.ConfigId == item.configId);
                configuration.IsActive = item.IsActive;
                db.Configuration.Update(configuration);
                db.SaveChanges();
                

                foreach (var item1 in item.catalogueInConfigurations)
                {
                    CatalogueInConfiguration catalogueInConfiguration = db.CatalogueInConfiguration.SingleOrDefault(con => con.Cicid == item1.Cicid);
                    catalogueInConfiguration.ConfigId = configuration.ConfigId;
                    catalogueInConfiguration.IsActive = configuration.IsActive;
                    db.CatalogueInConfiguration.Update(catalogueInConfiguration);
                    db.SaveChanges();
                }

                foreach (var item2 in item.ConfigurationRank)
                {
                    ConfigurationRank configurationRank = db.ConfigurationRank.SingleOrDefault(con => con.ConfigurationRankId == item2.ConfigurationRankId);
                    configurationRank.ConfigId = configuration.ConfigId;
                    configurationRank.IsActive = configuration.IsActive;
                    db.ConfigurationRank.Update(configurationRank);
                    db.SaveChanges();
                }
                }
                return null;
            }
        }
    }
}
