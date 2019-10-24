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
                Statistic statistic = db.Statistic.Include(o => o.Rank).Last(o => o.TestId == test.TestId);
                var cass = db.ConfigurationRank
                            .Include(cir => cir.Rank)
                            .Include(cir => cir.CatalogueInRank)
                            .Where(cir => cir.ConfigId == cir.Config.ConfigId && cir.Config.ConfigId == test.ConfigId)
                            .ToList();
                List<ConfigurationRank> configurations = cass.ToList();
                List<CatalogueInRankDTO> catalogueInRanks = new List<CatalogueInRankDTO>();
                List<CatalogueDTO> catas = db.Catalogue.Select(o => new CatalogueDTO(o)).ToList();
                for(int i = 0; i < configurations.Count; i++)
                {
                    CatalogueInRankDTO catalogueInRank = new CatalogueInRankDTO(configurations[i].RankId, configurations[i].Rank.Name, null);
                    List<CatalogueDTO> catalogues = new List<CatalogueDTO>();
                    foreach(CatalogueInRank cir in configurations[i].CatalogueInRank.ToList())
                    {
                        foreach(CatalogueDTO c in catas)
                        {
                            if(cir.CatalogueId == c.catalogueId)
                            {
                                catalogues.Add(new CatalogueDTO(cir.CatalogueId, c.name, null, cir.WeightPoint));
                            }
                        }
                    }
                    catalogueInRank.catalogues = catalogues;
                    catalogueInRanks.Add(catalogueInRank);
                }
                List<DetailStatisticDTO> details = (from ds in db.DetailStatistic
                                                    where ds.StatisticId == statistic.StatisticId
                                                    select new DetailStatisticDTO(ds)).ToList();
                for (int i = 0; i < details.Count; i++)
                {
                    for (int j = 0; j < catas.Count; j++)
                    {
                        if (details[i].catalogueId == catas[j].catalogueId)
                        {
                            catas[j].overallPoint = RoundDownNumber(details[i].point.Value);
                            break;
                        }
                    }
                }
                statistic.Point = Math.Round(statistic.Point.Value, 2);
                return new ApplicantResultDTO(test.AccountId, catalogueInRanks, RoundDownNumber(statistic.Point.Value), statistic.RankId, statistic.Rank.Name);

            }

        }

        public static double RoundDownNumber(double value)
        {
            return (Math.Floor(value * 100) / 100);
        }
    }
}
