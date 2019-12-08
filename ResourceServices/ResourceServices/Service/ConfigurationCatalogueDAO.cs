using Microsoft.EntityFrameworkCore;
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
                var con = db.CatalogueInConfiguration
                    .Include(c => c.Config)
                    .Include(c => c.CompanyCatalogue)
                    .Where(c => c.IsActive == isActive)
                    .Select(c => new ConfigurationCatalogueDTO(c, c.Config, c.CompanyCatalogue));

                return con.ToList();
            }
        }

        public static List<ConfigurationCatalogueDTO> GetConfigurationCatalogueByConfigId(int id)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var con = db.CatalogueInConfiguration
                    .Include(c => c.Config)
                    .Include(c => c.CompanyCatalogue)
                    .Where(c => c.ConfigId == id)
                    .Select(c => new ConfigurationCatalogueDTO(c, c.Config, c.CompanyCatalogue));
                return con.ToList();
            }
        }
    }
}
