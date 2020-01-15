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
        public static List<ConfigurationDTO> GetAllConfigurationForApplicant(bool type, int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Configuration.Include(a => a.Company).Where(c => c.CompanyId == companyId && c.Type == type).Select(c => new ConfigurationDTO(c)).OrderByDescending(x => x.configId).ToList();
            }
        }

        public static List<ConfigurationDTO> GetAllConfiguration(bool type, int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Configuration.Include(a => a.Company).Where(c => c.CompanyId == companyId && c.Type == type).Select(c => new ConfigurationDTO(c)).OrderByDescending(x => x.configId).ToList();
            }
        }

        public static List<ConfigurationDTO> GetConfigurationForEmployee(int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Configuration.Where(x => x.EndDate.Value >= DateTime.UtcNow.AddHours(1).AddMinutes(x.Duration)  && x.CompanyId == companyId && x.IsActive == true && x.Type == true).Select(x => new ConfigurationDTO(x)).ToList();
            }
        }

        public static void CreateConfiguration(ConfigurationDTO configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {

                Configuration configuration = new Configuration();

                var newLstCatalougeInConfig = new List<CatalogueInConfiguration>();
                var newLstRankInConfig = new List<RankInConfig>();

                foreach (var item in configurationDTO.catalogueInConfigurations)
                {
                    var catalougeInConfig = new CatalogueInConfiguration
                    {
                        CatalogueId = item.catalogueId,
                        WeightPoint = item.weightPoint,
                        NumberQuestion = item.numberQuestion,
                        IsActive = true
                    };
                    newLstCatalougeInConfig.Add(catalougeInConfig);
                }

                foreach (var item in configurationDTO.rankInConfigs)
                {
                    var rankInConfig = new RankInConfig
                    {
                        RankId = item.rankId,
                        Point = item.point,
                        IsActive = true
                    };
                    newLstRankInConfig.Add(rankInConfig);
                }

                configuration.CompanyId = configurationDTO.companyId;
                configuration.Title = configurationDTO.title;
                configuration.CreateDate = DateTime.UtcNow;
                configuration.StartDate = configurationDTO.startDate;
                configuration.EndDate = configurationDTO.endDate;
                configuration.Duration = configurationDTO.duration;
                configuration.Title = configurationDTO.title;
                configuration.Type = configurationDTO.type;
                configuration.IsActive = true;
                configuration.CatalogueInConfiguration = newLstCatalougeInConfig;
                configuration.RankInConfig = newLstRankInConfig;
                db.Configuration.Add(configuration);
                db.SaveChanges();
            }
        }

        public static ConfigurationDTO GetConfigurationById(int configId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var config = db.Configuration
                    .Include(c => c.CatalogueInConfiguration)
                    .ThenInclude(c => c.Catalogue)
                    .Include(c => c.RankInConfig)
                    .ThenInclude(c => c.Rank)
                    .Where(c => c.ConfigId == configId)
                    .Select(c => new ConfigurationDTO(c));
                return config.FirstOrDefault();
            }
        }

        public static void UpdateConfiguration(ConfigurationDTO configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Configuration configuration = db.Configuration.SingleOrDefault(con => con.ConfigId == configurationDTO.configId);
                configuration.Title = configurationDTO.title;
                configuration.StartDate = configurationDTO.startDate;
                configuration.EndDate = configurationDTO.endDate;
                configuration.Duration = configurationDTO.duration;
                db.SaveChanges();
            }
        }

        public static void ChangeStatusConfiguration(List<int> configIds, bool isActive)
        {
            using (DeverateContext db = new DeverateContext())
            {
                db.Configuration
                    .Where(x => configIds.Contains(x.ConfigId))
                    .ToList()
                    .ForEach(x => x.IsActive = isActive);
                db.SaveChanges();
            }
        }
    }
}
