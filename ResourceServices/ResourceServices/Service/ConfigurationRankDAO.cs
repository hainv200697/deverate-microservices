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
                var con = from configRank in db.ConfigurationRank
                            join config in db.Configuration on configRank.ConfigId equals config.ConfigId
                            join rank in db.Rank on configRank.RankId equals rank.RankId
                            where configRank.IsActive == isActive
                            select new ConfigurationRankDTO(configRank, config, rank);
                return con.ToList();
            }
        }
    }
}
