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
        public static ApplicantResultDTO GetStatisticByAccountId(int? testId)
        {
            using(DeverateContext db = new DeverateContext())
            {
                Test test = db.Test.Include(o => o.Config).Include(o => o.Statistic).Where(o => o.TestId == testId).First();
                //Test test = result.ToList().Where(o => o.TestId == testId).First();
                Statistic statistic = db.Statistic.Include(o => o.Rank).Last(o => o.TestId == test.TestId);

                var cas = from con in db.Configuration
                          join cr in db.ConfigurationRank on con.ConfigId equals cr.ConfigId
                          join cir in db.CatalogueInRank on cr.ConfigurationRankId equals cir.ConfigurationRankId
                          join c in db.Catalogue on cir.CatalogueId equals c.CatalogueId
                          where con.ConfigId == test.ConfigId && cr.RankId == statistic.RankId
                          select new CatalogueDTO(c.CatalogueId, c.Name, null, RoundDownNumber(cir.WeightPoint.Value));
                List<CatalogueDTO> catalogues = cas.ToList();
                List<DetailStatisticDTO> details = (from ds in db.DetailStatistic
                                                   where ds.StatisticId == statistic.StatisticId
                                                   select new DetailStatisticDTO(ds)).ToList();
                for(int i = 0; i < details.Count; i++)
                {
                    for(int j = 0; j < catalogues.Count; j++)
                    {
                        if(details[i].catalogueId == catalogues[j].catalogueId)
                        {
                            catalogues[j].overallPoint = RoundDownNumber(details[i].point.Value);
                            break;
                        }
                    }
                }
                statistic.Point = Math.Round(statistic.Point.Value, 2);
                return new ApplicantResultDTO(test.AccountId, catalogues, RoundDownNumber(statistic.Point.Value), statistic.Rank.Name);
            }
        }

        public static double RoundDownNumber(double value)
        {
            return (Math.Floor(value * 100) / 100);
        }
    }
}
