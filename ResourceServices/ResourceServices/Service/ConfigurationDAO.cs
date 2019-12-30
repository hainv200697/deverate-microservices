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

        public static List<ConfigurationDTO> GetAllConfigurationForApplicant(bool type, int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Configuration.Include(a => a.Account).Where(c => c.Account.CompanyId == companyId && c.Type == type).Select(c => new ConfigurationDTO(c)).OrderByDescending(x => x.configId).ToList();
            }
        }

        public static List<ConfigurationDTO> GetAllConfiguration(bool type, int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Configuration.Include(a => a.Account).Where(c => c.Account.CompanyId == companyId && c.Type == type).Select(c => new ConfigurationDTO(c)).OrderByDescending(x => x.configId).ToList();
            }
        }

        public static List<ConfigurationDTO> GetConfigurationForEmployee(int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Configuration.Include(x => x.Account).Where(x => x.EndDate >= DateTime.UtcNow && x.Account.CompanyId == companyId && x.IsActive == true && x.Type == true).Select(x => new ConfigurationDTO(x)).ToList();
            }
        }

        public static void CreateConfiguration(ConfigurationDTO configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {

                Configuration configuration = new Configuration();

                var newlstcatalougeinconfig = new List<CatalogueInConfiguration>();

                foreach (var item in configurationDTO.catalogueInConfigurationDTO)
                {
                    var newCatalogueInRank = new List<CatalogueInRank>();
                    foreach (var item2 in item.catalogueInRankDTO)
                    {
                        var clstatalougeinrank = new CatalogueInRank
                        {
                            CompanyRankId = item2.companyRankId,
                            IsActive = true,
                            Point = item2.point,
                        };
                        newCatalogueInRank.Add(clstatalougeinrank);
                    }
                    var catalougeinconfig = new CatalogueInConfiguration
                    {
                        CatalogueInRank = newCatalogueInRank,
                        CompanyCatalogueId = item.companyCatalogueId,
                        WeightPoint = item.weightPoint,
                        NumberQuestion = item.numberQuestion,
                        IsActive = true
                    };
                    newlstcatalougeinconfig.Add(catalougeinconfig);
                }

                configuration.AccountId = configurationDTO.accountId;
                configuration.Title = configurationDTO.title;
                configuration.CreateDate = DateTime.UtcNow;
                configuration.StartDate = configurationDTO.startDate;
                configuration.EndDate = configurationDTO.endDate;
                configuration.Duration = configurationDTO.duration;
                configuration.Title = configurationDTO.title;
                configuration.Type = configurationDTO.type;
                configuration.IsActive = true;
                configuration.CatalogueInConfiguration = newlstcatalougeinconfig;
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
                    .ThenInclude(x => x.CatalogueInRank)
                    .ThenInclude(x => x.CompanyRank)
                    .Include(x => x.CatalogueInConfiguration)
                    .ThenInclude(x => x.CompanyCatalogue)
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

        public static void ChangeStatusConfiguration(List<ConfigurationDTO> configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {
                foreach (var item in configurationDTO)
                {
                    Configuration configuration = db.Configuration.SingleOrDefault(con => con.ConfigId == item.configId);
                    configuration.IsActive = item.isActive;
                    db.Configuration.Update(configuration);
                    db.SaveChanges();
                }
            }
        }
    }
}
