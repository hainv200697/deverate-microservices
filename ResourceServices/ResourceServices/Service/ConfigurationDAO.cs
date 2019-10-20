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
                                    join acc in db.Account on con.TestOwnerId equals acc.AccountId
                                    where con.IsActive == isActive
                                    select new ConfigurationDTO(con, con.CatalogueInConfiguration.ToList(), con.ConfigurationRank.ToList(), acc.Fullname);
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
                    return null;
                }

                Configuration configuration = new Configuration();
                configuration.TestOwnerId = configurationDTO.testOwnerId;
                configuration.Title = configurationDTO.title;
                configuration.TotalQuestion = configurationDTO.totalQuestion;
                configuration.CreateDate = configurationDTO.createDate;
                configuration.StartDate = configurationDTO.startDate;
                configuration.EndDate = configurationDTO.endDate;
                configuration.Duration = configurationDTO.duration;
                configuration.Title = configurationDTO.title;
                configuration.IsActive = true;
                configuration.CatalogueInConfiguration = configurationDTO.catalogueInConfigurations;
                configuration.ConfigurationRank = configurationDTO.ConfigurationRank;
                var configSave = db.Configuration.Add(configuration);
                db.SaveChanges();

                foreach (var item in configurationDTO.ConfigurationRank)
                {
                    CatalogueInRank catalogueInRank = new CatalogueInRank();
                    catalogueInRank.ConfigurationRankId = item.ConfigurationRankId;
                    catalogueInRank.CatalogueId = item.CatalogueInRank.ToList()[0].CatalogueId;
                    catalogueInRank.WeightPoint = item.CatalogueInRank.ToList()[0].WeightPoint;
                    catalogueInRank.IsActive = true;
                    db.SaveChanges();
                }
                configurationDTO.configId = configSave.Entity.ConfigId;
                return configurationDTO;
            }
        }

        public static ConfigurationDTO GetConfigurationById(int id)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var configuration = from con in db.Configuration
                                    where con.ConfigId == id
                                    join acc in db.Account on con.TestOwnerId equals acc.AccountId
                                    select new ConfigurationDTO(con, con.CatalogueInConfiguration.ToList(), con.ConfigurationRank.ToList(), acc.Fullname);
                return configuration.FirstOrDefault();
            }
        }

        public static string UpdateConfiguration(ConfigurationDTO configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Configuration configuration = db.Configuration.SingleOrDefault(con => con.ConfigId == configurationDTO.configId);
                configuration.Title = configurationDTO.title;
                configuration.TotalQuestion = configurationDTO.totalQuestion;
                configuration.StartDate = configurationDTO.startDate;
                configuration.EndDate = configurationDTO.endDate;
                configuration.Duration = configurationDTO.duration;
                configuration.IsActive = true;
                db.Configuration.Update(configuration);
                db.SaveChanges();

                var catas = from cif in db.CatalogueInConfiguration
                            join c in db.Configuration on cif.ConfigId equals c.ConfigId
                            where c.ConfigId == configurationDTO.configId
                            select cif;
                List<CatalogueInConfiguration> catalogues = catas.ToList();

                for(int i = 0; i < configurationDTO.catalogueInConfigurations.Count; i++)
                {
                    if(catalogues.Any(o => o.ConfigId == configurationDTO.catalogueInConfigurations[i].ConfigId))
                    {
                        catalogues.Find(o => o.ConfigId == configurationDTO.catalogueInConfigurations[i].ConfigId).IsActive = false;
                    }
                    else
                    {
                        db.CatalogueInConfiguration.Add(configurationDTO.catalogueInConfigurations[i]); 
                    }
                    db.SaveChanges();
                }


                //foreach (var item in configurationDTO.ConfigurationRank)
                //{
                //    ConfigurationRank configurationRank = db.ConfigurationRank.SingleOrDefault(con => con.ConfigurationRankId == item.ConfigurationRankId);
                //    configurationRank.ConfigId = configuration.ConfigId;
                //    configurationRank.RankId = item.RankId;
                //    configurationRank.WeightPoint = item.WeightPoint;
                //    configurationRank.IsActive = item.IsActive;
                //    db.ConfigurationRank.Update(configurationRank);
                //    db.SaveChanges();
                //}

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
