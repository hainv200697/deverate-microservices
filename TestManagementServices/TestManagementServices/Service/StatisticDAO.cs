using AuthenServices.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestManagementServices.Model;
using TestManagementServices.Models;

namespace TestManagementServices.Service
{
    public class StatisticDAO
    {
        public static GeneralStatisticDTO GetGeneralStatisticOfApplicantByTestOwnerId(int? testOwnerId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Account account = db.Account.Where(o => o.AccountId == testOwnerId).First();
                List<Configuration> configurations = db.Configuration
                    .Include(c => c.Account).Include(c => c.Test)
                    .Include(c => c.CatalogueInConfiguration)
                    .Where(c => c.Account.CompanyId == account.CompanyId && c.Type == false)
                    .ToList();
                List<int?> configIds = new List<int?>();
                configurations.ForEach(c => configIds.Add(c.ConfigId));
                List<CompanyCatalogue> catalogueInCompanies = db.CompanyCatalogue
                                        .Include(c => c.CatalogueInConfiguration)
                                        .Where(c => c.CompanyId == account.CompanyId)
                                        .ToList();
                catalogueInCompanies = catalogueInCompanies.GroupBy(c => c.CompanyCatalogueId).Select(c => c.First()).ToList();


                List<Test> tests = db.Test
                    .Include(t => t.Account)
                    .Include(t => t.DetailResult)
                    .Include(t => t.Config.CatalogueInConfiguration)
                    .Where(t => t.Account.CompanyId == account.CompanyId && t.ApplicantId != null).ToList();

                List<GeneralStatisticItemDTO> generalStatisticItems = new List<GeneralStatisticItemDTO>();
                for (int j = 0; j < configurations.Count; j++)
                {
                    int numberOfTest = configurations[j].Test.Where(t => t.Status == AppConstrain.SUBMITTED).ToList().Count();
                    int totalTest = configurations[j].Test.ToList().Count;
                    int numberOfFinishedTest = 0;
                    List<CompanyCatalogueDTO> cloneCatalogues = new List<CompanyCatalogueDTO>();
                    foreach (CompanyCatalogue c in catalogueInCompanies)
                    {
                        cloneCatalogues.Add(new CompanyCatalogueDTO(c.CompanyCatalogueId, c.Name, 0));
                    }
                    GeneralStatisticItemDTO gsi = new GeneralStatisticItemDTO();
                    gsi.configId = configurations[j].ConfigId;
                    double totalGPA = 0;
                    for (int k = 0; k < tests.Count; k++)
                    {
                        if (tests[k].ConfigId == configurations[j].ConfigId)
                        {
                            totalGPA += tests[k].Point == null ? 0 : tests[k].Point.Value;
                            List<DetailResult> details = tests[k].DetailResult.ToList();
                            if (details.Count > 0)
                            {
                                numberOfFinishedTest += 1;
                            }
                            for (int m = 0; m < details.Count; m++)
                            {

                                for (int n = 0; n < cloneCatalogues.Count; n++)
                                {
                                    if (details[m].CatalogueInConfig.CompanyCatalogueId == cloneCatalogues[n].companyCatalogueId)
                                    {
                                        cloneCatalogues[n].value += AppConstrain.RoundDownNumber(details[m].Point / numberOfTest, AppConstrain.SCALE_DOWN_NUMB);
                                        if (cloneCatalogues[n].value > AppConstrain.SCALE_UP_NUMB)
                                        {
                                            cloneCatalogues[n].value = AppConstrain.SCALE_UP_NUMB;
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    gsi.configGPA = numberOfTest == 0 ? 0 : AppConstrain.RoundDownNumber(totalGPA / numberOfTest, AppConstrain.SCALE_DOWN_NUMB);
                    gsi.series = cloneCatalogues;
                    gsi.createDate = configurations[j].CreateDate;
                    gsi.endDate = configurations[j].EndDate;
                    gsi.name = configurations[j].Title;
                    gsi.numberOfFinishedTest = numberOfFinishedTest > totalTest ? totalTest : numberOfFinishedTest;
                    gsi.totalTest = totalTest;
                    generalStatisticItems.Add(gsi);

                }

                return new GeneralStatisticDTO(generalStatisticItems);
            }
        }
        public static List<RankStatisticItemDTO> GetRankStatisticOfApplicantByTestOwnerId(int? testOwnerId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Account account = db.Account.Where(o => o.AccountId == testOwnerId).First();
                List<Account> accounts = db.Account
                    .Include(a => a.Configuration)
                    .ThenInclude(Configuration => Configuration.CatalogueInConfiguration)
                    .Where(a => a.CompanyId == account.CompanyId)
                    .ToList();
                List<Configuration> applicantConfigs = db.Configuration
                    .Include(c => c.Test)
                    .Where(c => c.Account.AccountId == testOwnerId && c.Type == false)
                    .ToList();
                if (applicantConfigs.Count == 0) return null;
                List<int?> testIds = new List<int?>();


                for (int i = 0; i < applicantConfigs.Count; i++)
                {
                    for (int j = 0; j < applicantConfigs[i].Test.Count; j++)
                    {
                        testIds.Add(applicantConfigs[i].Test.ToList()[j].TestId);
                    }
                }
                if (testIds.Count == 0) return null;
                List<Test> tests = db.Test.Where(t => testIds.Contains(t.TestId)).ToList();
                List<CompanyRankDTO> ranks = db.CompanyRank
                    .Where(r => r.IsActive == true && r.CompanyId == account.CompanyId)
                    .Select(r => new CompanyRankDTO(r))
                    .ToList();
                List<RankStatisticItemDTO> rankStatisticItems = new List<RankStatisticItemDTO>();
                int configCount = 0;


                for (int j = 0; j < applicantConfigs.Count; j++)
                {
                    int totalApp = 0;
                    try
                    {
                        totalApp = applicantConfigs[j].Test.Count;
                    }
                    catch (Exception)
                    {
                        totalApp = 0;
                    }
                    List<int?> totalOfDidTests = new List<int?>();
                    RankStatisticItemDTO rankStatisticItem = new RankStatisticItemDTO();
                    rankStatisticItem.configId = applicantConfigs[j].ConfigId;
                    rankStatisticItem.createDate = applicantConfigs[j].CreateDate;
                    rankStatisticItem.endDate = applicantConfigs[j].EndDate;
                    rankStatisticItem.name = applicantConfigs[j].Title;
                    List<CompanyRankDTO> cloneRanks = new List<CompanyRankDTO>();
                    foreach (CompanyRankDTO r in ranks)
                    {
                        cloneRanks.Add(new CompanyRankDTO(r.companyRankId, r.name, r.position, 0));
                    }
                    CompanyRankDTO notRanked = new CompanyRankDTO(-1, AppConstrain.UNKNOWN_RANK, -1, 0);
                    for (int k = 0; k < tests.Count; k++)
                    {
                        if (tests[k].ConfigId == applicantConfigs[j].ConfigId)
                        {

                            if (tests[k].CompanyRankId == null && tests[k].Point != null)
                            {
                                if (!totalOfDidTests.Contains(tests[k].ApplicantId))
                                {
                                    totalOfDidTests.Add(tests[k].ApplicantId);
                                }
                                notRanked.count += 1;
                                continue;
                            }
                            for (int m = 0; m < cloneRanks.Count; m++)
                            {
                                if (tests[k].CompanyRankId == cloneRanks[m].companyRankId)
                                {
                                    if (!totalOfDidTests.Contains(tests[k].ApplicantId))
                                    {
                                        totalOfDidTests.Add(tests[k].ApplicantId);
                                    }
                                    cloneRanks[m].count += 1;
                                }
                            }
                        }

                    }
                    rankStatisticItem.tested = new TestedItemDTO(totalOfDidTests.Count, AppConstrain.APPLICANT_DO_TEST);
                    rankStatisticItem.totalAccount = new TotalEmpItemDTO(totalApp, AppConstrain.TOTAL_APPLICANT_DO_TEST);
                    cloneRanks.Add(notRanked);
                    cloneRanks = cloneRanks.OrderBy(r => r.position).ToList();
                    rankStatisticItem.series = cloneRanks;
                    rankStatisticItems.Add(rankStatisticItem);
                    configCount++;
                }

                return rankStatisticItems;
            }
        }


        public static List<UserStatisticDTO> GetOverallPointStatisticByCompanyId(int? companyId, int? configId, bool? isEmployee)
        {
            using (DeverateContext db = new DeverateContext())
            {
                if (companyId == null && configId == null)
                {
                    return null;
                }

                Configuration configuration = null;
                if (configId != null)
                {
                    configuration = db.Configuration
                        .Where(c => c.ConfigId == configId)
                        .FirstOrDefault();
                    if (configuration == null)
                    {
                        return null;
                    }
                }
                else
                {
                    if (isEmployee == null)
                    {
                        configuration = db.Configuration
                            .Where(c => c.Account.CompanyId == companyId && c.Type == true)
                            .LastOrDefault();
                    }
                    else
                    {
                        configuration = db.Configuration
                            .Where(c => c.Account.CompanyId == companyId && c.Type == isEmployee)
                            .LastOrDefault();
                    }

                }
                if (configuration == null)
                {
                    return null;
                }
                List<Test> tests = db.Test
                    .Include(t => t.Account)
                    .Include(t => t.Applicant)
                    .Include(t => t.CompanyRank)
                    .Include(t => t.PotentialRank)
                    .Where(t => t.ConfigId == configuration.ConfigId && t.Status == AppConstrain.SUBMITTED).ToList();
                List<UserStatisticDTO> userStatistics = new List<UserStatisticDTO>();
                if (tests.Count == 0 || tests == null)
                {
                    return null;
                }
                List<int?> userIds = new List<int?>();
                foreach (Test t in tests)
                {
                    if ((t.AccountId == null && t.ApplicantId == null))
                    {
                        continue;
                    }
                    if (t.AccountId != null)
                    {
                        userStatistics.Add(new UserStatisticDTO(t.AccountId, t.Account.Username,
                            t.StartTime, t.CompanyRank == null ? AppConstrain.UNKNOWN_RANK: t.CompanyRank.Name,
                            t.PotentialRank == null ? AppConstrain.UNKNOWN_RANK : t.PotentialRank.Name,
                            t.Point == null  ? 0 : AppConstrain.RoundDownNumber(t.Point.Value, AppConstrain.SCALE_DOWN_NUMB),
                            configuration.Title, t.TestId));
                    }
                    else
                    {
                        userStatistics.Add(new UserStatisticDTO(t.ApplicantId, t.Applicant.Email,
                            t.StartTime, t.CompanyRank == null ? AppConstrain.UNKNOWN_RANK: t.CompanyRank.Name,
                            t.PotentialRank == null? AppConstrain.UNKNOWN_RANK: t.PotentialRank.Name,
                            t.Point == null ? 0 : AppConstrain.RoundDownNumber(t.Point.Value, AppConstrain.SCALE_DOWN_NUMB),
                            configuration.Title, t.TestId));
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
                List<Account> accounts = db.Account
                    .Include(a => a.Configuration)
                    .ThenInclude(Configuration => Configuration.CatalogueInConfiguration)
                    .Where(a => a.CompanyId == account.CompanyId)
                    .ToList();
                var result = from t in db.Test
                             join con in db.Configuration on t.ConfigId equals con.ConfigId
                             join acc in db.Account on con.AccountId equals acc.AccountId
                             where acc.CompanyId == account.CompanyId
                             select t;
                List<Test> tests = result.ToList();
                List<CompanyRankDTO> ranks = db.CompanyRank
                    .Where(r => r.IsActive == true && r.CompanyId == account.CompanyId)
                    .Select(r => new CompanyRankDTO(r))
                    .ToList();
                List<RankStatisticItemDTO> rankStatisticItems = new List<RankStatisticItemDTO>();
                int configCount = 0;
                for (int i = 0; i < accounts.Count; i++)
                {
                    List<Configuration> configurations = accounts[i].Configuration.ToList();
                    for (int j = 0; j < configurations.Count; j++)
                    {
                        int totalEmp = 0;
                        try
                        {
                            totalEmp = configurations[j].Test.Count;
                        }
                        catch (Exception)
                        {
                            totalEmp = 0;
                        }
                        List<int?> totalOfDidTests = new List<int?>();
                        RankStatisticItemDTO rankStatisticItem = new RankStatisticItemDTO();
                        rankStatisticItem.configId = configurations[j].ConfigId;
                        rankStatisticItem.createDate = configurations[j].CreateDate;
                        rankStatisticItem.endDate = configurations[j].EndDate;
                        rankStatisticItem.name = configurations[j].Title;
                        List<CompanyRankDTO> cloneRanks = new List<CompanyRankDTO>();
                        foreach (CompanyRankDTO r in ranks)
                        {
                            cloneRanks.Add(new CompanyRankDTO(r.companyRankId, r.name, r.position, 0));
                        }
                        CompanyRankDTO notRanked = new CompanyRankDTO(-1, AppConstrain.UNKNOWN_RANK, -1, 0);
                        for (int k = 0; k < tests.Count; k++)
                        {
                            if (tests[k].ConfigId == configurations[j].ConfigId)
                            {

                                if(tests[k].CompanyRankId == null && tests[k].Point != null)
                                {
                                    if (!totalOfDidTests.Contains(tests[k].AccountId))
                                    {
                                        totalOfDidTests.Add(tests[k].AccountId);
                                    }
                                    notRanked.count += 1;
                                    continue;
                                }
                                for (int m = 0; m < cloneRanks.Count; m++)
                                {
                                    if (tests[k].CompanyRankId == cloneRanks[m].companyRankId)
                                    {
                                        if (!totalOfDidTests.Contains(tests[k].AccountId))
                                        {
                                            totalOfDidTests.Add(tests[k].AccountId);
                                        }
                                        cloneRanks[m].count += 1;
                                    }
                                }
                            }

                        }
                        rankStatisticItem.tested = new TestedItemDTO(totalOfDidTests.Count);
                        rankStatisticItem.totalAccount = new TotalEmpItemDTO(totalEmp);
                        cloneRanks.Add(notRanked);
                        cloneRanks = cloneRanks.OrderBy(r => r.position).ToList();
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
            using (DeverateContext db = new DeverateContext())
            {
                Account account = db.Account.Where(o => o.AccountId == testOwnerId).First();
                List<Configuration> configurations = db.Configuration
                    .Include(c => c.Account)
                    .Include(c => c.CatalogueInConfiguration)
                    .Where(c => c.Account.CompanyId == account.CompanyId && c.Type == true)
                    .ToList();

                List<CompanyCatalogue> catalogueInCompanies = db.CompanyCatalogue
                                        .Include(c => c.CatalogueInConfiguration)
                                        .Where(c => c.CompanyId == account.CompanyId)
                                        .ToList();
                catalogueInCompanies = catalogueInCompanies.GroupBy(c => c.CompanyCatalogueId).Select(c => c.First()).ToList();


                List<Test> tests = db.Test
                    .Include(t => t.Account)
                    .Include(t => t.DetailResult)
                    .Include(t => t.Config.CatalogueInConfiguration)
                    .Where(t => t.Account.CompanyId == account.CompanyId && t.AccountId != null).ToList();

                List<GeneralStatisticItemDTO> generalStatisticItems = new List<GeneralStatisticItemDTO>();
                for (int j = 0; j < configurations.Count; j++)
                {
                    int totalTest = configurations[j].Test.Count;
                    int numberOfTest = configurations[j].Test.Where(t => t.Status == AppConstrain.SUBMITTED).ToList().Count();
                    int numberOfFinishedTest = 0;
                    List<CompanyCatalogueDTO> cloneCatalogues = new List<CompanyCatalogueDTO>();
                    foreach (CompanyCatalogue c in catalogueInCompanies)
                    {
                        cloneCatalogues.Add(new CompanyCatalogueDTO(c.CompanyCatalogueId, c.Name, 0));
                    }
                    GeneralStatisticItemDTO gsi = new GeneralStatisticItemDTO();
                    gsi.configId = configurations[j].ConfigId;
                    double totalGPA = 0;
                    for (int k = 0; k < tests.Count; k++)
                    {
                        if (tests[k].ConfigId == configurations[j].ConfigId)
                        {
                            
                            totalGPA += tests[k].Point == null ? 0: tests[k].Point.Value;
                            List<DetailResult> details = tests[k].DetailResult.ToList();
                            if(details.Count > 0)
                            {
                                numberOfFinishedTest += 1;
                            }
                            for (int m = 0; m < details.Count; m++)
                            {

                                for (int n = 0; n < cloneCatalogues.Count; n++)
                                {
                                    if (details[m].CatalogueInConfig.CompanyCatalogueId == cloneCatalogues[n].companyCatalogueId)
                                    {
                                        cloneCatalogues[n].value += AppConstrain.RoundDownNumber(details[m].Point / numberOfTest, AppConstrain.SCALE_DOWN_NUMB);
                                        if (cloneCatalogues[n].value > AppConstrain.SCALE_UP_NUMB)
                                        {
                                            cloneCatalogues[n].value = AppConstrain.SCALE_UP_NUMB;
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    gsi.configGPA = numberOfTest == 0 ? 0 : AppConstrain.RoundDownNumber(totalGPA / numberOfTest, AppConstrain.SCALE_DOWN_NUMB);
                    gsi.series = cloneCatalogues;
                    gsi.createDate = configurations[j].CreateDate;
                    gsi.endDate = configurations[j].EndDate;
                    gsi.name = configurations[j].Title;
                    gsi.numberOfFinishedTest = numberOfFinishedTest > totalTest ? totalTest : numberOfFinishedTest;
                    gsi.totalTest = totalTest;
                    generalStatisticItems.Add(gsi);

                }

                return new GeneralStatisticDTO(generalStatisticItems);
            }
        }
        public static CandidateResultDTO GetStatisticByTestId(int testId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Test test = db.Test
                    .Include(t => t.CompanyRank)
                    .Include(t => t.PotentialRank)
                    .Include(o => o.Config)
                    .ThenInclude(c => c.CatalogueInConfiguration)
                    .Include(o => o.DetailResult)
                    .ThenInclude(c => c.CatalogueInConfig)
                    .Where(o => o.TestId == testId).First();

                if (test.DetailResult.Count == 0)
                {
                    return null;
                }
                List<Test> tests = db.Test
                    .Where(t => t.ConfigId == test.ConfigId && t.IsActive == true)
                    .ToList();
                double lowerTestPoint = 0;
                foreach(Test t in tests)
                {
                    if(t.Point != null)
                    {
                        if(t.Point < test.Point)
                        {
                            lowerTestPoint += 1;
                        }
                    }
                    else
                    {
                        lowerTestPoint += 1;
                    }
                }
                double lowerTestPercent = 0;
                if(tests.Count > 0)
                {
                    lowerTestPercent = AppConstrain.RoundDownNumber(lowerTestPoint / tests.Count, AppConstrain.SCALE_UP_NUMB);
                }
                List<ConfigurationRankDTO> configurationRankDTOs = SystemDAO.GetRankPoint(test);
                List<CatalogueInConfigDTO> catalogueInConfigs = db.CatalogueInConfiguration.Include(c => c.CompanyCatalogue)
                    .Where(c => c.ConfigId == test.ConfigId)
                    .Select(c => new CatalogueInConfigDTO(c))
                    .ToList();
                List<int?> catalogueIds = new List<int?>();
                List<int> catalogeInConfigIds = new List<int>();
                foreach(CatalogueInConfigDTO cic in catalogueInConfigs)
                {
                    catalogueIds.Add(cic.companyCatalogueId);
                    catalogeInConfigIds.Add(cic.catalogueInConfigId);
                }
                List<CatalogueInRankDTO> catalogueInRankDTOs = new List<CatalogueInRankDTO>();
                List<CompanyCatalogueDTO> catas = db.CompanyCatalogue
                    .Where(c => catalogueIds.Contains(c.CompanyCatalogueId))
                    .Select(o => new CompanyCatalogueDTO(o))
                    .ToList();
                List<CatalogueInRank> catalogueInRanks = db.CatalogueInRank
                    .Where(cic => catalogeInConfigIds
                    .Contains(cic.CatalogueInConfigId))
                    .ToList();
                for (int i = 0; i < configurationRankDTOs.Count; i++)
                {

                    CatalogueInRankDTO catalogueInRank = new CatalogueInRankDTO(configurationRankDTOs[i].companyRankId,
                        configurationRankDTOs[i].rank, null);
                    List<CompanyCatalogueDTO> catalogues = new List<CompanyCatalogueDTO>();
                    foreach (CatalogueInRank cir in catalogueInRanks)
                    {

                        if (cir.CompanyRankId == configurationRankDTOs[i].companyRankId)
                        {
                            catalogues.Add(new CompanyCatalogueDTO(cir.CatalogueInConfig.CompanyCatalogue.CompanyCatalogueId,
                                cir.CatalogueInConfig.CompanyCatalogue.Name, null,
                                AppConstrain.RoundDownNumber(cir.Point, AppConstrain.SCALE_DOWN_NUMB)));
                        }
                    }
                    catalogueInRank.catalogues = catalogues;
                    catalogueInRankDTOs.Add(catalogueInRank);
                }

                for (int i = 0; i < test.DetailResult.Count; i++)
                {
                    for (int j = 0; j < catas.Count; j++)
                    {
                        if (test.DetailResult.ToList()[i].CatalogueInConfig.CompanyCatalogueId == catas[j].companyCatalogueId)
                        {
                            
                            catas[j].overallPoint = AppConstrain.RoundDownNumber(test.DetailResult.ToList()[i].Point, AppConstrain.SCALE_DOWN_NUMB);
                            foreach (CatalogueInRank cir in catalogueInRanks)
                            {

                                if (cir.CatalogueInConfig.CompanyCatalogueId == test.DetailResult.ToList()[i].CatalogueInConfig.CompanyCatalogueId && cir.CompanyRankId == test.PotentialRankId)
                                {
                                    catas[i].differentPoint = test.DetailResult.ToList()[i].Point - cir.Point;
                                    if(catas[i].differentPoint >= 0)
                                    {
                                        catas[i].differentPoint = 0;
                                    }
                                    else
                                    {
                                        catas[i].differentPoint *= -1;
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                string potentialRank = test.PotentialRank == null ? AppConstrain.UNKNOWN_RANK : test.PotentialRank.Name;
                double statisticPoint = AppConstrain.RoundDownNumber(test.Point.Value, AppConstrain.SCALE_DOWN_NUMB);
                return new CandidateResultDTO(test.AccountId, configurationRankDTOs, catas,
                    catalogueInRankDTOs, catalogueInConfigs, statisticPoint,
                    test.CompanyRankId, (test.CompanyRank == null ? AppConstrain.UNKNOWN_RANK : test.CompanyRank.Name),
                    test.PotentialRankId, potentialRank, lowerTestPercent);
            }
        }

        public static List<TestHistoryDTO> GetHistory(int? accountId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                List<Test> tests = db.Test
                    .Include(t => t.Account)
                    .Include(t => t.DetailResult)
                    .ThenInclude(t => t.CatalogueInConfig)
                    .Where(t => t.AccountId == accountId).ToList();
                if (tests.Count == 0) return null;
                List<CompanyRank> ranks = db.CompanyRank.ToList();
                List<TestHistoryDTO> testHistories = new List<TestHistoryDTO>();
                List<CompanyCatalogue> catas = db.CompanyCatalogue.ToList();

                List<CompanyCatalogue> companyCatalogues = db.CompanyCatalogue
                    .Include(c => c.CatalogueInConfiguration)
                    .ThenInclude(c => c.Config)
                    .Where(c => c.CompanyId == tests.ToList()[0].Account.CompanyId)
                    .ToList();

                companyCatalogues = companyCatalogues.GroupBy(c => c.CompanyCatalogueId).Select(c => c.First()).ToList();


                for (int i = 0; i < tests.Count; i++)
                {
                    TestHistoryDTO test = new TestHistoryDTO();
                    test.testId = tests[i].TestId;
                    test.title = tests[i].Config.Title;
                    test.point = tests[i].Point;
                    test.rankId = tests[i].CompanyRankId;
                    test.createDate = tests[i].CreateDate;
                    test.startTime = tests[i].StartTime;
                    foreach (CompanyRank r in ranks)
                    {
                        if (r.CompanyRankId == tests[i].CompanyRankId)
                        {
                            test.rank = r.Name;
                            break;
                        }
                    }
                    List<CompanyCatalogueDTO> cloneCatalogues = new List<CompanyCatalogueDTO>();
                    foreach (CompanyCatalogue c in companyCatalogues)
                    {
                        cloneCatalogues.Add(new CompanyCatalogueDTO(c.CompanyCatalogueId, c.Name, 0, 0));
                    }
                    foreach (DetailResult ds in tests[i].DetailResult.ToList())
                    {
                        for (int j = 0; j < cloneCatalogues.Count; j++)
                        {
                            if (cloneCatalogues[j].companyCatalogueId == ds.CatalogueInConfig.CompanyCatalogueId)
                            {
                                cloneCatalogues[j].overallPoint = AppConstrain.RoundDownNumber(ds.Point, AppConstrain.SCALE_DOWN_NUMB);
                            }
                        }
                    }
                    test.catalogues = cloneCatalogues;
                    testHistories.Add(test);
                }
                return testHistories;
            }
        }
        public static object GetInfoByTestId(int testId)
        {
            using (DeverateContext db = new DeverateContext())
            {

                var info = db.Test.Include(x => x.Account).Include(x => x.Applicant).Where(x => x.TestId == testId).FirstOrDefault();
                if (info.AccountId != null)
                {
                    return new AccountDTO(info.AccountId.Value, info.Account.Username,
                        info.Account.Fullname, info.Account.Phone, info.Account.Email,
                        info.Account.Address, info.Account.Gender);

                }
                else
                {
                    return new ApplicantDTO(info.Applicant);
                }
            }
        }
    }
}
