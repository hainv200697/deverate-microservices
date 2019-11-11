using AuthenServices.Model;
using Microsoft.EntityFrameworkCore;
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


        public static List<ConfigurationDTO> GetAllConfiguration(bool isActive, int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Configuration.Include(a => a.Account).Where(c => c.Account.CompanyId == companyId && c.IsActive == isActive).Select(c => new ConfigurationDTO(c)).ToList();
            }
        }



        public static ConfigurationDTO CreateConfiguration(ConfigurationDTO configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                if (configurationDTO.type.Value)
                {
                    var result = db.Account.Where(a => a.AccountId == configurationDTO.testOwnerId).Select(a => a.CompanyId);
                    int? companyId = result.First();
                    var emps = db.Account.Where(a => a.CompanyId == companyId && a.IsActive && a.RoleId == 3).Select(a => new AccountDTO(a));
                    if (emps.ToList().Count == 0)
                    {
                        return null;
                    }

                    Configuration configuration = new Configuration();
                    configuration.AccountId = configurationDTO.testOwnerId.Value;
                    configuration.Title = configurationDTO.title;
                    configuration.TotalQuestion = configurationDTO.totalQuestion.Value;
                    configuration.CreateDate = configurationDTO.createDate;
                    configuration.StartDate = configurationDTO.startDate;
                    configuration.EndDate = configurationDTO.endDate;
                    configuration.Duration = configurationDTO.duration.Value;
                    configuration.Title = configurationDTO.title;
                    configuration.Type = configurationDTO.type.Value;
                    configuration.IsActive = true;
                    configuration.CatalogueInConfiguration = configurationDTO.catalogueInConfigurations;
                    configuration.ConfigurationRank = configurationDTO.ConfigurationRank;
                    var configSave = db.Configuration.Add(configuration);

                    foreach (var item in configurationDTO.ConfigurationRank)
                    {
                        CatalogueInRank catalogueInRank = new CatalogueInRank();
                        catalogueInRank.ConfigurationRankId = item.ConfigurationRankId;
                        catalogueInRank.CatalogueId = item.CatalogueInRank.ToList()[0].CatalogueId;
                        catalogueInRank.WeightPoint = item.CatalogueInRank.ToList()[0].WeightPoint;
                        catalogueInRank.IsActive = true;
                    }
                    db.SaveChanges();
                    configurationDTO.configId = configSave.Entity.ConfigId;
                    return configurationDTO;
                }
                else
                {
                    var config = db.Configuration.Where(c => c.IsActive && !c.Type);
                    config.ForEachAsync(c => c.IsActive = false);
                    Configuration configuration = new Configuration();
                    configuration.AccountId = configurationDTO.testOwnerId.Value;
                    configuration.Title = configurationDTO.title;
                    configuration.TotalQuestion = configurationDTO.totalQuestion.Value;
                    configuration.CreateDate = configurationDTO.createDate;
                    configuration.StartDate = configurationDTO.startDate;
                    configuration.EndDate = configurationDTO.endDate;
                    configuration.Duration = configurationDTO.duration.Value;
                    configuration.Title = configurationDTO.title;
                    configuration.Type = configurationDTO.type.Value;
                    configuration.IsActive = true;
                    configuration.CatalogueInConfiguration = configurationDTO.catalogueInConfigurations;
                    configuration.ConfigurationRank = configurationDTO.ConfigurationRank;
                    var configSave = db.Configuration.Add(configuration);

                    foreach (var item in configurationDTO.ConfigurationRank)
                    {
                        CatalogueInRank catalogueInRank = new CatalogueInRank();
                        catalogueInRank.ConfigurationRankId = item.ConfigurationRankId;
                        catalogueInRank.CatalogueId = item.CatalogueInRank.ToList()[0].CatalogueId;
                        catalogueInRank.WeightPoint = item.CatalogueInRank.ToList()[0].WeightPoint;
                        catalogueInRank.IsActive = true;
                    }
                    db.SaveChanges();
                    configurationDTO.configId = configSave.Entity.ConfigId;
                    return configurationDTO;
                }
            }
        }

        public static ConfigurationDTO GetConfigurationById(int configId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var cass = db.ConfigurationRank
                            .Include(cir => cir.Config.Account)
                            .Include(cir => cir.Rank)
                            .Include(cir => cir.CatalogueInRank)
                            .Where(cir => cir.ConfigId == configId)
                            .ToList();
                List<CatalogueInConfigDTO> catalogueInConfigs = db.CatalogueInConfiguration.Include(c => c.Catalogue).Where(c => c.ConfigId == configId).Select(c => new CatalogueInConfigDTO(c)).ToList();
                List<Catalogue> catalogues = db.Catalogue.ToList();
                return new ConfigurationDTO(cass[0].Config, cass[0].Config.CatalogueInConfiguration.Select(c => new CatalogueInConfigDTO(c)).ToList(), cass.Select(c => new ConfigurationRankDTO(c)).ToList(), cass[0].Config.Account.Fullname, 0);
            }
        }

        public static string UpdateConfiguration(ConfigurationDTO configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Configuration configuration = db.Configuration.SingleOrDefault(con => con.ConfigId == configurationDTO.configId);
                configuration.Title = configurationDTO.title;
                configuration.TotalQuestion = configurationDTO.totalQuestion.Value;
                configuration.StartDate = configurationDTO.startDate;
                configuration.EndDate = configurationDTO.endDate;
                configuration.Duration = configurationDTO.duration.Value;
                configuration.Type = configurationDTO.type.Value;
                configuration.IsActive = true;
                db.Configuration.Update(configuration);
                db.SaveChanges();

                var catas = from cif in db.CatalogueInConfiguration
                            join c in db.Configuration on cif.ConfigId equals c.ConfigId
                            where c.ConfigId == configurationDTO.configId
                            select cif;
                List<CatalogueInConfiguration> catalogues = catas.ToList();

                for (int i = 0; i < configurationDTO.catalogueInConfigurations.Count; i++)
                {
                    if (catalogues.Any(o => o.ConfigId == configurationDTO.catalogueInConfigurations[i].ConfigId))
                    {
                        catalogues.Find(o => o.ConfigId == configurationDTO.catalogueInConfigurations[i].ConfigId).IsActive = configurationDTO.catalogueInConfigurations[i].IsActive;
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
                    configuration.IsActive = item.isActive.Value;
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
