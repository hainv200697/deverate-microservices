using ResourceServices.Model;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Service
{
    public class ConfigurationCatalogueDAO
    {
        DeverateContext context;

        public ConfigurationCatalogueDAO(DeverateContext context)
        {
            this.context = context;
        }

        public static List<ConfigurationCatalogueDTO> GetAllConfigurationCatalogue(bool isActive)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var con = from configCata in db.CatalogueInConfiguration
                          join config in db.Configuration on configCata.ConfigId equals config.ConfigId
                          join cata in db.Catalogue on configCata.CatalogueId equals cata.CatalogueId
                          where configCata.IsActive == isActive 
                          select new ConfigurationCatalogueDTO(configCata, config, cata);
                return con.ToList();
            }
        }

        public static List<ConfigurationCatalogueDTO> GetConfigurationCatalogueByConfigId(int id)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var con = from configCata in db.CatalogueInConfiguration
                          join config in db.Configuration on configCata.ConfigId equals config.ConfigId
                          join cata in db.Catalogue on configCata.CatalogueId equals cata.CatalogueId
                          where configCata.ConfigId == id
                          select new ConfigurationCatalogueDTO(configCata, config, cata);
                return con.ToList();
            }
        }
    }
}
