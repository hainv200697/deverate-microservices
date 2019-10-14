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
                return configuration.ToList().OrderByDescending(x => x.configId).ToList();
            }
        }



        public static ConfigurationDTO CreateConfiguration(ConfigurationDTO configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var result = from a in db.Account
                                 where a.AccountId == configurationDTO.testOwnerId
                                 select a.CompanyId;
                int? companyId = result.First();
                var emps = from a in db.Account
                           where a.CompanyId == companyId && a.IsActive == true && a.RoleId == 3
                           select new AccountDTO(a);
                if(emps.ToList().Count == 0)
                {
                    return Message.noAvailableEmployee;
                }

                Configuration configuration = new Configuration();
                configuration.TestOwnerId = configurationDTO.testOwnerId;
                configuration.TotalQuestion = configurationDTO.totalQuestion;
                configuration.CreateDate = configurationDTO.createDate;
                configuration.StartDate = configurationDTO.startDate;
                configuration.EndDate = configurationDTO.endDate;
                configuration.Duration = configurationDTO.duration;
                configuration.Title = configurationDTO.title;
                configuration.IsActive = true;
                configuration.CatalogueInConfiguration = configurationDTO.catalogueInConfigurations;
                configuration.ConfigurationRank = configurationDTO.ConfigurationRank;
                db.Configuration.Add(configuration);
                db.SaveChanges();  
                return Message.createConfigSucceed;
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
                configuration.TestOwnerId = configurationDTO.testOwnerId;
                configuration.TotalQuestion = configurationDTO.totalQuestion;
                configuration.CreateDate = DateTime.Now;
                configuration.StartDate = configurationDTO.startDate;
                configuration.EndDate = configurationDTO.endDate;
                configuration.Duration = configurationDTO.duration;
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
                configuration.IsActive = item.isActive;
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
