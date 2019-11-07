﻿using Microsoft.EntityFrameworkCore;
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
        public static List<UserStatisticDTO> GetOverallPointStatisticByCompanyId(int? companyId)
        {
            using(DeverateContext db = new DeverateContext())
            {
                if(companyId == null)
                {
                    return null;
                }
                Configuration configuration = db.Configuration.Where(c => c.TestOwner.CompanyId == companyId).LastOrDefault();
                List<Test> tests = db.Test
                    .Include(t => t.Account)
                    .Include(t => t.Statistic)
                    .Where(t => t.ConfigId == configuration.ConfigId).ToList();
                List<UserStatisticDTO> userStatistics = new List<UserStatisticDTO>();
               if(tests.Count == 0 || tests == null)
                {
                    return null;
                }
                List<int?> userIds = new List<int?>();
               foreach(Test t in tests)
                {
                    if (!userIds.Contains(t.AccountId)){
                        userIds.Add(t.AccountId);
                        userStatistics.Add(new UserStatisticDTO(t.AccountId, t.Account.Fullname, t.StartTime, (t.Statistic == null || t.Statistic.Count == 0) ? 0 : RoundDownNumber(t.Statistic.Last().Point.Value, AppConstrain.scaleUpNumb), configuration.Title, configuration.CreateDate));
                    }
                }
                return userStatistics;
            }
        }


        public static List<RankStatisticItemDTO> GetRankStatisticByTestOwnerId(int? testOwnerId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Account account = db.Account.Where(o => o.AccountId == testOwnerId).First();
                List<Account> accounts = db.Account.Include(a => a.Configuration).ThenInclude(Configuration=> Configuration.CatalogueInConfiguration).Where(a => a.CompanyId == account.CompanyId).ToList();
                var result = from t in db.Test
                             join con in db.Configuration on t.ConfigId equals con.ConfigId
                             join acc in db.Account on con.TestOwnerId equals acc.AccountId
                             where acc.CompanyId == account.CompanyId
                             select t;
                List<Test> tests = result.ToList();
                List<int?> testIds = new List<int?>();
                foreach (Test t in tests)
                {
                    testIds.Add(t.TestId);
                }
                List<Statistic> statistics = db.Statistic.Include(s => s.DetailStatistic).Where(s => testIds.Contains(s.TestId)).ToList();
                List<RankDTO> ranks = db.Rank.Where(r => r.IsActive == true).Select(r => new RankDTO(r)).ToList();
                List<RankStatisticItemDTO> rankStatisticItems = new List<RankStatisticItemDTO>();
                int configCount = 0;
                for (int i = 0; i < accounts.Count; i++)
                {
                    List<Configuration> configurations = accounts[i].Configuration.ToList();
                    for (int j = 0; j < configurations.Count; j++)
                    {
                        if(configCount == 5)
                        {
                            break;
                        }
                        RankStatisticItemDTO rankStatisticItem = new RankStatisticItemDTO();
                        rankStatisticItem.configId = configurations[j].ConfigId;
                        rankStatisticItem.createDate = configurations[j].CreateDate;
                        rankStatisticItem.endDate = configurations[j].EndDate;
                        rankStatisticItem.name = configurations[j].Title;
                        List<RankDTO> cloneRanks = new List<RankDTO>();
                        foreach (RankDTO r in ranks)
                        {
                            cloneRanks.Add(new RankDTO(r.rankId, r.name, 0));
                        }
                        for (int k = 0; k < statistics.Count; k++)
                        {
                            if (statistics[k].Test.ConfigId == configurations[j].ConfigId)
                            {
                                for(int m = 0; m < cloneRanks.Count; m++)
                                {
                                    if(statistics[k].RankId == cloneRanks[m].rankId)
                                    {
                                        cloneRanks[m].count += 1;
                                    }
                                }
                            }

                        }
                        rankStatisticItem.series = cloneRanks;
                        rankStatisticItems.Add(rankStatisticItem);
                        configCount++;
                    }
                }

                return rankStatisticItems;
            }
        }

        public static GeneralStatisticDTO GetGeneralStatisticByTestOwnerId(int? testOwnerId)
        {
            using(DeverateContext db = new DeverateContext())
            {
                Account account = db.Account.Where(o => o.AccountId == testOwnerId).First();
                List<Configuration> configurations = db.Configuration.Include(c => c.TestOwner).Include(c => c.CatalogueInConfiguration)
                                                     .Where(c => c.TestOwner.CompanyId == account.CompanyId).ToList();
                List<Account> accounts = db.Account.Include(a => a.Configuration).Where(a => a.CompanyId == account.CompanyId).ToList();
                var result = from t in db.Test
                                    join con in db.Configuration on t.ConfigId equals con.ConfigId
                                    join cif in db.CatalogueInConfiguration on con.ConfigId equals cif.ConfigId
                                    join acc in db.Account on con.TestOwnerId equals acc.AccountId
                                    where acc.CompanyId == account.CompanyId
                                    select t;
                List<Test> tests = result.ToList();
                List<int?> testIds = new List<int?>();
                foreach(Test t in tests)
                {
                    testIds.Add(t.TestId);
                }
                List<Statistic> statistics = db.Statistic.Include(s => s.DetailStatistic).Where(s => testIds.Contains(s.TestId)).ToList();
                List<CatalogueDTO> catalogues = db.Catalogue.Select(c => new CatalogueDTO(c.CatalogueId, c.Name, 0)).ToList();
                List<CatalogueInCompany> catalogueInCompanies = db.CatalogueInCompany.Include(c => c.Catalogue).Where(c => c.CompanyId == account.CompanyId).ToList();
                List<GeneralStatisticItemDTO> generalStatisticItems = new List<GeneralStatisticItemDTO>();
                for(int j = 0; j < configurations.Count; j++)
                {
                    int numberOfTest = configurations[j].Test.ToList().Count();
                    int numberOfFinishedTest = 0;
                    List<CatalogueDTO> cloneCatalogues = new List<CatalogueDTO>();
                    //foreach (CatalogueDTO c in catalogues)
                    //{
                    //    foreach (CatalogueInConfiguration cif in configurations[j].CatalogueInConfiguration)
                    //    {
                    //        if(cif.CatalogueId == c.catalogueId)
                    //        {
                    //            cloneCatalogues.Add(new CatalogueDTO(c.catalogueId, c.name, 0));
                    //            break;
                    //        }
                    //    }
                            
                    //}
                    foreach (CatalogueInCompany c in catalogueInCompanies)
                    {
                        cloneCatalogues.Add(new CatalogueDTO(c.Catalogue.CatalogueId, c.Catalogue.Name, 0));
                    }
                    GeneralStatisticItemDTO gsi = new GeneralStatisticItemDTO();
                    gsi.configId = configurations[j].ConfigId;
                    double totalGPA = 0;
                    for(int k = 0; k < statistics.Count; k++)
                    {
                        if(statistics[k].Test.ConfigId == configurations[j].ConfigId)
                        {
                            numberOfFinishedTest += 1;
                            totalGPA += statistics[k].Point == null ? 0: statistics[k].Point.Value;
                            List<DetailStatistic> details = statistics[k].DetailStatistic.ToList();
                            for (int m = 0; m < details.Count; m++)
                            {
                                    
                                for (int n = 0; n < cloneCatalogues.Count; n++)
                                {
                                    if (details[m].CatalogueId == cloneCatalogues[n].catalogueId)
                                    {
                                        cloneCatalogues[n].value +=  RoundDownNumber(details[m].Point.Value / numberOfTest, 1);
                                        break;
                                    }
                                }
                            }
                        }
                            
                    }
                    gsi.configGPA = numberOfTest == 0 ? 0:  RoundDownNumber(totalGPA / numberOfTest, AppConstrain.scaleUpNumb);
                    gsi.series = cloneCatalogues;
                    gsi.createDate = configurations[j].CreateDate;
                    gsi.endDate = configurations[j].EndDate;
                    gsi.name = configurations[j].Title;
                    gsi.numberOfFinishedTest = numberOfFinishedTest > numberOfTest ? numberOfTest : numberOfFinishedTest;
                    gsi.totalTest = numberOfTest;
                    generalStatisticItems.Add(gsi); 

                }
                 
                return new GeneralStatisticDTO(generalStatisticItems);
            }
        }
        public static CandidateResultDTO GetStatisticByTestId(int? testId)
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
                List<CatalogueInConfigDTO> catalogueInConfigs = db.CatalogueInConfiguration.Where(c => c.ConfigId == test.ConfigId).Select(c => new CatalogueInConfigDTO(c)).ToList();
                configurations.ForEach(c => c.WeightPoint = c.WeightPoint * AppConstrain.scaleUpNumb);
                List<CatalogueInRankDTO> catalogueInRanks = new List<CatalogueInRankDTO>();
                List<CatalogueDTO> catas = db.Catalogue.Select(o => new CatalogueDTO(o)).ToList();
                List<ConfigurationRankDTO> configurationRanks = new List<ConfigurationRankDTO>();
                for(int i = 0; i < configurations.Count; i++)
                {
                    configurationRanks.Add(new ConfigurationRankDTO(configurations[i]));
                    CatalogueInRankDTO catalogueInRank = new CatalogueInRankDTO(configurations[i].RankId, configurations[i].Rank.Name, null);
                    List<CatalogueDTO> catalogues = new List<CatalogueDTO>();
                    foreach(CatalogueInRank cir in configurations[i].CatalogueInRank.ToList())
                    {
                        foreach(CatalogueDTO c in catas)
                        {
                            if(cir.CatalogueId == c.catalogueId)
                            {
                                catalogues.Add(new CatalogueDTO(cir.CatalogueId, c.name, null, RoundDownNumber(cir.WeightPoint.Value, AppConstrain.scaleUpNumb) ));
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
                            catas[j].overallPoint = RoundDownNumber(details[i].point.Value, AppConstrain.scaleUpNumb);
                            break;
                        }
                    }
                }
                double statisticPoint = RoundDownNumber(statistic.Point.Value, AppConstrain.scaleUpNumb);
                return new CandidateResultDTO(test.AccountId, configurationRanks,  catas,  catalogueInRanks, catalogueInConfigs, statisticPoint, statistic.RankId, statistic.Rank.Name);
            }
        }

        public static List<TestHistoryDTO> GetHistory(int? accountId)
        {
            using(DeverateContext db = new DeverateContext())
            {
                List<Statistic> statistics = db.Statistic.Include(t => t.Test.Config).Include(t => t.DetailStatistic).Where(t => t.Test.AccountId == accountId).ToList();
                List<Rank> ranks = db.Rank.ToList();
                List<TestHistoryDTO> testHistories = new List<TestHistoryDTO>();
                List<Catalogue> catas = db.Catalogue.ToList();
                for(int i = 0; i < statistics.Count; i++)
                {
                    TestHistoryDTO test = new TestHistoryDTO();
                    test.testId = statistics[i].TestId;
                    test.title = statistics[i].Test.Config.Title;
                    test.point = statistics[i].Point;
                    test.rankId = statistics[i].RankId;
                    test.createDate = statistics[i].Test.CreateDate;
                    test.startTime = statistics[i].Test.StartTime;
                    foreach (Rank r in ranks)
                    {
                        if(r.RankId == statistics[i].RankId)
                        {
                            test.rank = r.Name;
                            break;
                        }
                    }
                    List<CatalogueDTO> catalogues = new List<CatalogueDTO>();
                    foreach(DetailStatistic ds in statistics[i].DetailStatistic.ToList())
                    {
                        CatalogueDTO ca = new CatalogueDTO();
                        ca.catalogueId = ds.CatalogueId;
                        ca.overallPoint = ds.Point * AppConstrain.scaleUpNumb;
                        ca.weightPoint = null;
                        foreach(Catalogue c in catas)
                        {
                            if(c.CatalogueId == ds.CatalogueId)
                            {
                                ca.name = c.Name;
                                break;
                            }
                        }
                        catalogues.Add(ca);
                    }
                    test.catalogues = catalogues;
                    testHistories.Add(test);
                }
                return testHistories;
            }
        }

        public static double RoundDownNumber(double value, int scaleUp)
        {

            double rNumb = Math.Round(value * scaleUp, 1);
            return rNumb > scaleUp ? scaleUp : rNumb;
        }
    }
}
