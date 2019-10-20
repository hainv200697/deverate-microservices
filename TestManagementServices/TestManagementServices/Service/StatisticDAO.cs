using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Model;
using TestManagementServices.Models;

namespace TestManagementServices.Service
{
    public class StatisticDAO
    {
        public static ApplicantResultDTO GetStatisticByAccountId(int? accountId)
        {
            using(DeverateContext db = new DeverateContext())
            {
                Test test = db.Test.Include(o => o.Config).Include(o => o.Statistic).
                    OrderByDescending(o => o.StartTime).FirstOrDefault(o => o.AccountId == accountId);
                Statistic statistic = db.Statistic.Include(o => o.Rank).FirstOrDefault(o => o.StatisticId == test.Statistic.First().StatisticId);
                var cas = from con in db.Configuration
                          join cr in db.ConfigurationRank on con.ConfigId equals cr.ConfigId
                          join cir in db.CatalogueInRank on cr.ConfigurationRankId equals cir.ConfigurationRankId
                          join c in db.Catalogue on cir.CatalogueId equals c.CatalogueId
                          where con.ConfigId == test.ConfigId && cr.RankId == statistic.RankId
                          select new CatalogueDTO(c.CatalogueId, c.Name, null, cir.WeightPoint);
                List<CatalogueDTO> catalogues = cas.ToList();
                List<DetailStatisticDTO> details = (from ds in db.DetailStatistic
                                                   where ds.StatisticId == test.Statistic.First().StatisticId
                                                   select new DetailStatisticDTO(ds)).ToList();
                for(int i = 0; i < details.Count; i++)
                {
                    for(int j = 0; j < catalogues.Count; j++)
                    {
                        if(details[i].catalogueId == catalogues[j].catalogueId)
                        {
                            catalogues[j].overallPoint = details[i].point;
                            break;
                        }
                    }
                }

                return new ApplicantResultDTO(test.AccountId, catalogues, statistic.Point, statistic.Rank.Name);
            }
        }
    }
}
