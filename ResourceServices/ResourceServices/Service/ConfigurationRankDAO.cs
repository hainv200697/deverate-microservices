using Microsoft.EntityFrameworkCore;
using ResourceServices.Model;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Service
{
    public class ConfigurationRankDAO
    {
        DeverateContext context;

        public ConfigurationRankDAO(DeverateContext context)
        {
            this.context = context;
        }

        public static List<ConfigurationRankDTO> GetAllRank(bool isActive)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var con = db.RankInConfiguration.Include(c => c.Config)
                    .Include(c => c.CompanyRank)
                    .Where(c => c.IsActive == isActive)
                    .Select(c => new ConfigurationRankDTO(c, c.Config, c.CompanyRank));
                return con.ToList();
            }
        }
    }
}
