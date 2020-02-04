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
        /// <summary>
        /// Thông kê chi tiết từng catalogue của ứng viên 
        /// </summary>
        /// <param name="testOwnerId"></param>
        /// <returns></returns>
        public static GeneralStatisticDTO GetGeneralStatisticOfApplicantByTestOwnerId(int? testOwnerId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Account account = db.Account.Where(o => o.AccountId == testOwnerId).First();
                List<Configuration> configurations = db.Configuration
                    .Include(c => c.Company).Include(c => c.Test)
                    .Include(c => c.CatalogueInConfiguration)
                    .Where(c => c.CompanyId == account.CompanyId && c.Type == false)
                    .ToList();
                List<int?> configIds = new List<int?>();
                configurations.ForEach(c => configIds.Add(c.ConfigId));
                List<Catalogue> catalogueInCompanies = db.Catalogue
                                        .Include(c => c.CatalogueInConfiguration)
                                        .Where(c => c.CompanyId == account.CompanyId)
                                        .ToList();
                catalogueInCompanies = catalogueInCompanies.GroupBy(c => c.CatalogueId).Select(c => c.First()).ToList();
                List<Test> tests = db.Test
                    .Include(t => t.Account)
                    .Include(t => t.DetailResult)
                    .Include(t => t.Config.CatalogueInConfiguration)
                    .Where(t => t.Account.CompanyId == account.CompanyId && t.ApplicantId != null)
                    .ToList();
                List<GeneralStatisticItemDTO> generalStatisticItems = new List<GeneralStatisticItemDTO>();
                for (int j = 0; j < configurations.Count; j++)
                {
                    int numberOfTest = configurations[j].Test.Where(t => t.Status == AppConstrain.SUBMITTED).ToList().Count();
                    int totalTest = configurations[j].Test.ToList().Count;
                    int numberOfFinishedTest = 0;
                    List<CatalogueDTO> cloneCatalogues = new List<CatalogueDTO>();
                    foreach (Catalogue c in catalogueInCompanies)
                    {
                        cloneCatalogues.Add(new CatalogueDTO(c.CatalogueId, c.Name, 0));
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
                                    if (details[m].CatalogueInConfig.CatalogueId == cloneCatalogues[n].catalogueId)
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
                    gsi.expiredDays = configurations[j].ExpiredDays;
                    gsi.name = configurations[j].Title;
                    gsi.numberOfFinishedTest = numberOfFinishedTest > totalTest ? totalTest : numberOfFinishedTest;
                    gsi.totalTest = totalTest;
                    generalStatisticItems.Add(gsi);
                }
                return new GeneralStatisticDTO(generalStatisticItems);
            }
        }

        /// <summary>
        /// Thông kê chi tiết từng rank theo ứng viên
        /// </summary>
        /// <param name="testOwnerId"></param>
        /// <returns></returns>
        public static List<RankStatisticItemDTO> GetRankStatisticOfApplicantByTestOwnerId(int? testOwnerId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Account account = db.Account.Where(o => o.AccountId == testOwnerId).First();
                List<Configuration> applicantConfigs = db.Configuration
                    .Include(c => c.Test)
                    .Where(c => c.CompanyId == testOwnerId && c.Type == false)
                    .ToList();
                if (applicantConfigs.Count == 0) return null;
                List<RankDTO> ranks = db.Rank
                    .Where(r => r.IsActive == true && r.CompanyId == account.CompanyId)
                    .Select(r => new RankDTO(r))
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
                    rankStatisticItem.expiredDays = applicantConfigs[j].ExpiredDays;
                    rankStatisticItem.name = applicantConfigs[j].Title;
                    List<RankDTO> cloneRanks = new List<RankDTO>();
                    foreach (RankDTO r in ranks)
                    {
                        cloneRanks.Add(new RankDTO(r.companyRankId, r.name, 0));
                    }
                    RankDTO notRanked = new RankDTO(-1, AppConstrain.UNKNOWN_RANK, 0);
                    List<Test> tests = applicantConfigs[j].Test.ToList();
                    for (int k = 0; k < tests.Count; k++)
                    {
                        if (tests[k].RankId == null && tests[k].Point != null)
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
                            if (tests[k].RankId == cloneRanks[m].companyRankId)
                            {
                                if (!totalOfDidTests.Contains(tests[k].ApplicantId))
                                {
                                    totalOfDidTests.Add(tests[k].ApplicantId);
                                }
                                cloneRanks[m].count += 1;
                            }
                        }
                    }
                    rankStatisticItem.tested = new TestedItemDTO(totalOfDidTests.Count, AppConstrain.APPLICANT_DO_TEST);
                    rankStatisticItem.totalAccount = new TotalEmpItemDTO(totalApp, AppConstrain.TOTAL_APPLICANT_DO_TEST);
                    cloneRanks.Add(notRanked);
                    rankStatisticItem.series = cloneRanks;
                    rankStatisticItems.Add(rankStatisticItem);
                    configCount++;
                }
                return rankStatisticItems;
            }
        }

        public static List<StatusTestApplicant> GroupStatusTestByConfigId(int configId, DateTime? from, DateTime? to)
        {
            using (DeverateContext context = new DeverateContext())
            {
                if (from == null)
                {
                    from = context.Configuration.FirstOrDefault(x => x.ConfigId == configId).CreateDate;
                }
                if (to == null)
                {
                    to = DateTime.UtcNow;
                }
                var tests = context.Test
                    .Include(x => x.Rank)
                    .Include(x => x.Applicant)
                    .Where(t => t.ConfigId == configId && t.CreateDate >= from && t.CreateDate <= to)
                    .ToList();
                List<StatusTestApplicant> result = new List<StatusTestApplicant>();
                result.Add(new StatusTestApplicant { name = "Total", value = tests.Count });
                result.Add(new StatusTestApplicant { name = "Pending", value = tests.Where(x => x.Status == "Pending" || x.Status == "Doing").ToList().Count });
                result.Add(new StatusTestApplicant { name = "Submitted", value = tests.Where(x => x.Status == "Submitted").ToList().Count });
                result.Add(new StatusTestApplicant { name = "Expired", value = tests.Where(x => x.Status == "Expired").ToList().Count });
                return result;
            }
        }

        public static List<ApplicantResult> ApplicantResultByConfigId(int configId, DateTime? from, DateTime? to)
        {
            using (DeverateContext context = new DeverateContext())
            {
                if (from == null)
                {
                    from = context.Configuration.FirstOrDefault(x => x.ConfigId == configId).CreateDate;
                }
                if (to == null)
                {
                    to = DateTime.UtcNow;
                }
                return context.Test
                    .Include(x => x.Rank)
                    .Include(x => x.Applicant)
                    .Where(t => t.ConfigId == configId && t.CreateDate >= from && t.CreateDate <= to && t.Status == "Submitted")
                    .OrderBy(x => x.Point)
                    .Select(t => new ApplicantResult(t))
                    .ToList();
            }
        }

        public static List<RankApplicantStatistic> RankStatisticApplicant(int configId, DateTime? from, DateTime? to)
        {
            using(DeverateContext context = new DeverateContext())
            {
                if (from == null)
                {
                    from = context.Configuration.FirstOrDefault(x => x.ConfigId == configId).CreateDate;
                }
                if (to == null)
                {
                    to = DateTime.UtcNow;
                }
                return context.Test
                    .Include(x => x.Rank)
                    .Where(t => t.ConfigId == configId && t.CreateDate >= from && t.CreateDate <= to && t.Status == "Submitted")
                    .OrderBy(x => x.CreateDate)
                    .GroupBy(t => t.Rank.Name)
                    .Select(g => new RankApplicantStatistic { name = g.Key, value = g.Count() })
                    .ToList();
            }
        }

        public static List<CatalogueStatistic> CatalogueStatisticApplicant(int configId, DateTime? from, DateTime? to)
        {
            using(DeverateContext context = new DeverateContext())
            {
                List<CatalogueStatistic> result = new List<CatalogueStatistic>();
                var configuration = context.Configuration
                                            .Include(x => x.CatalogueInConfiguration)
                                            .ThenInclude(x => x.Catalogue)
                                            .FirstOrDefault(x => x.ConfigId == configId);
                foreach(var catalogueInConfig in configuration.CatalogueInConfiguration)
                {
                    result.Add(new CatalogueStatistic
                    {
                        CatalogueInConfigId = catalogueInConfig.CatalogueInConfigId,
                        name = catalogueInConfig.Catalogue.Name,
                        series = new List<CatalogueSeriesItem>()
                    });
                }
                if (from == null)
                {
                    from = configuration.CreateDate;
                }
                if (to == null)
                {
                    to = DateTime.UtcNow;
                }
                var tests = context.Test
                    .Include(x => x.DetailResult)
                    .Where(t => t.ConfigId == configId && t.CreateDate >= from && t.CreateDate <= to && t.Status == "Submitted")
                    .OrderBy(x => x.CreateDate)
                    .ToList();
                foreach(var test in tests)
                {
                    var date = test.CreateDate.Value.ToString("dd/MM/yyyy");
                    foreach(var detailResult in test.DetailResult)
                    {
                        var index = result.FindIndex(x => x.CatalogueInConfigId == detailResult.CatalogueInConfigId);
                        var seriesIndex = result[index].series.FindIndex(x => x.name == date);
                        if (seriesIndex >= 0)
                        {
                            result[index].series[seriesIndex].value += detailResult.Point;
                            result[index].series[seriesIndex].numberTest++;
                        } else
                        {
                            result[index].series.Add(new CatalogueSeriesItem
                            {
                                name = date,
                                value = detailResult.Point,
                                numberTest = 1
                            });
                        }
                    }
                }

                // Average for series
                foreach(var catalogueStatistic in result)
                {
                    foreach(var catalogueseriesItem in catalogueStatistic.series)
                    {
                        catalogueseriesItem.value = catalogueseriesItem.value / catalogueseriesItem.numberTest;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Thông kê chi tiết điểm số trong công ty
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="configId"></param>
        /// <param name="isEmployee"></param>
        /// <returns></returns>
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
                            .Where(c => c.CompanyId == companyId && c.Type == true)
                            .LastOrDefault();
                    }
                    else
                    {
                        configuration = db.Configuration
                            .Where(c => c.CompanyId == companyId && c.Type == isEmployee)
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
                    .Include(t => t.Rank)
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
                            t.StartTime, t.Rank == null ? AppConstrain.UNKNOWN_RANK: t.Rank.Name,
                            t.PotentialRank == null ? AppConstrain.UNKNOWN_RANK : t.PotentialRank.Name,
                            t.Point == null  ? 0 : AppConstrain.RoundDownNumber(t.Point.Value, AppConstrain.SCALE_DOWN_NUMB),
                            configuration.Title, t.TestId));
                    }
                    else
                    {
                        userStatistics.Add(new UserStatisticDTO(t.ApplicantId, t.Applicant.Email,
                            t.StartTime, t.Rank == null ? AppConstrain.UNKNOWN_RANK: t.Rank.Name,
                            t.PotentialRank == null? AppConstrain.UNKNOWN_RANK: t.PotentialRank.Name,
                            t.Point == null ? 0 : AppConstrain.RoundDownNumber(t.Point.Value, AppConstrain.SCALE_DOWN_NUMB),
                            configuration.Title, t.TestId));
                    }
                }
                return userStatistics;
            }
        }

        /// <summary>
        /// Thống kê chi tiết từng rank trong công ty
        /// </summary>
        /// <param name="testOwnerId"></param>
        /// <returns></returns>
        public static List<RankStatisticItemDTO> GetRankStatisticByTestOwnerId(int? testOwnerId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Account account = db.Account.Where(o => o.AccountId == testOwnerId).First();
                List<Configuration> configurations = db.Configuration
                    .Include(t => t.Test)
                    .ThenInclude(t => t.Account)
                    .Where(t => t.CompanyId == account.CompanyId).ToList();
                List<RankDTO> ranks = db.Rank
                    .Where(r => r.IsActive == true && r.CompanyId == account.CompanyId)
                    .Select(r => new RankDTO(r))
                    .ToList();
                List<RankStatisticItemDTO> rankStatisticItems = new List<RankStatisticItemDTO>();
                int configCount = 0;
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
                    List<Test> tests = configurations[j].Test.ToList();
                    List<int?> totalOfDidTests = new List<int?>();
                    RankStatisticItemDTO rankStatisticItem = new RankStatisticItemDTO();
                    rankStatisticItem.configId = configurations[j].ConfigId;
                    rankStatisticItem.createDate = configurations[j].CreateDate;
                    rankStatisticItem.expiredDays = configurations[j].ExpiredDays;
                    rankStatisticItem.name = configurations[j].Title;
                    List<RankDTO> cloneRanks = new List<RankDTO>();
                    foreach (RankDTO r in ranks)
                    {
                        cloneRanks.Add(new RankDTO(r.companyRankId, r.name, 0));
                    }
                    RankDTO notRanked = new RankDTO(-1, AppConstrain.UNKNOWN_RANK, 0);
                    for (int k = 0; k < tests.Count; k++)
                    {
                        if(tests[k].RankId == null && tests[k].Point != null)
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
                            if (tests[k].RankId == cloneRanks[m].companyRankId)
                            {
                                if (!totalOfDidTests.Contains(tests[k].AccountId))
                                {
                                    totalOfDidTests.Add(tests[k].AccountId);
                                }
                                cloneRanks[m].count += 1;
                            }
                        }
                    }
                    rankStatisticItem.tested = new TestedItemDTO(totalOfDidTests.Count);
                    rankStatisticItem.totalAccount = new TotalEmpItemDTO(totalEmp);
                    cloneRanks.Add(notRanked);
                    rankStatisticItem.series = cloneRanks;
                    rankStatisticItems.Add(rankStatisticItem);
                    configCount++;
                }
                return rankStatisticItems;
            }
        }

        /// <summary>
        /// Lấy thông kê tổng quát của công ty
        /// </summary>
        /// <param name="testOwnerId"></param>
        /// <returns></returns>
        public static GeneralStatisticDTO GetGeneralStatisticByTestOwnerId(int? testOwnerId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Account account = db.Account.Where(o => o.AccountId == testOwnerId).First();
                List<Configuration> configurations = db.Configuration
                    .Include(c => c.Company)
                    .Include(c => c.CatalogueInConfiguration)
                    .Where(c => c.CompanyId == account.CompanyId && c.Type == true)
                    .ToList();
                List<Catalogue> catalogueInCompanies = db.Catalogue
                                        .Include(c => c.CatalogueInConfiguration)
                                        .Where(c => c.CompanyId == account.CompanyId)
                                        .ToList();
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
                    List<CatalogueDTO> cloneCatalogues = new List<CatalogueDTO>();
                    foreach (Catalogue c in catalogueInCompanies)
                    {
                        cloneCatalogues.Add(new CatalogueDTO(c.CatalogueId, c.Name, 0));
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
                                    if (details[m].CatalogueInConfig.CatalogueId == cloneCatalogues[n].catalogueId)
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
                    gsi.expiredDays = configurations[j].ExpiredDays;
                    gsi.name = configurations[j].Title;
                    gsi.numberOfFinishedTest = numberOfFinishedTest > totalTest ? totalTest : numberOfFinishedTest;
                    gsi.totalTest = totalTest;
                    generalStatisticItems.Add(gsi);

                }
                return new GeneralStatisticDTO(generalStatisticItems);
            }
        }

        /// <summary>
        /// Lấy kết quả bài test
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        public static CandidateResultDTO GetStatisticByTestId(int testId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Test test = db.Test
                    .Include(t => t.Rank)
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
                List<CatalogueInConfigDTO> catalogueInConfigs = db.CatalogueInConfiguration
                    .Include(c => c.Catalogue)
                    .Where(c => c.ConfigId == test.ConfigId)
                    .Select(c => new CatalogueInConfigDTO(c))
                    .ToList();
                List<int> catalogueIds = catalogueInConfigs.Select(c => c.catalogueId).ToList();
                List<CatalogueInRankDTO> catalogueInRankDTOs = new List<CatalogueInRankDTO>();
                List<CatalogueDTO> catas = db.Catalogue
                    .Where(c => catalogueIds.Contains(c.CatalogueId))
                    .Select(o => new CatalogueDTO(o))
                    .ToList();
                List<int> rankIds = db.RankInConfig
                    .OrderBy(r => r.Point)
                    .Where(r => r.ConfigId == test.ConfigId)
                    .Select(r => r.RankId)
                    .ToList();
                List<Rank> ranks = db.Rank
                    .Include(r => r.CatalogueInRank)
                    .ThenInclude(r => r.Catalogue)
                    .Where(r => rankIds.Contains(r.RankId))
                    .ToList();
                List<CatalogueInRank> catalogueInRanks = db.CatalogueInRank
                    .Include(c => c.Catalogue)
                    .Include(c => c.Rank)
                    .Where(cic => rankIds
                    .Contains(cic.RankId))
                    .ToList();
                for (int i = 0; i < configurationRankDTOs.Count; i++)
                {
                    CatalogueInRankDTO catalogueInRank = new CatalogueInRankDTO(configurationRankDTOs[i].rankId,
                        configurationRankDTOs[i].rank, null);
                    List<CatalogueDTO> catalogues = new List<CatalogueDTO>();
                    foreach (CatalogueInRank cir in catalogueInRanks)
                    {
                        if (cir.RankId == configurationRankDTOs[i].rankId && cir.Point != 0 && catalogueIds.Contains(cir.CatalogueId))
                        {
                            catalogues.Add(new CatalogueDTO(cir.CatalogueId,
                                cir.Catalogue.Name, null,
                                AppConstrain.RoundDownNumber(cir.Point, AppConstrain.SCALE_DOWN_NUMB)));
                        }
                    }
                    catalogueInRank.catalogues = catalogues;
                    catalogueInRankDTOs.Add(catalogueInRank);
                }
                string nextRank = null;
                for (int i = 0; i < test.DetailResult.Count; i++)
                {
                    for (int j = 0; j < catas.Count; j++)
                    {
                        if (test.DetailResult.ToList()[i].CatalogueInConfig.CatalogueId == catas[j].catalogueId)
                        {
                            int pos = 0;
                            catas[j].overallPoint = AppConstrain.RoundDownNumber(test.DetailResult.ToList()[i].Point, AppConstrain.SCALE_DOWN_NUMB);
                            if (test.Rank == null && test.Point != null)
                            {
                                nextRank = ranks[pos].Name;
                                for (int k = 0; k < ranks[pos].CatalogueInRank.Count; k++)
                                {
                                    CatalogueInRank cir = ranks[pos].CatalogueInRank.ToList()[k];
                                    if (cir.CatalogueId == test.DetailResult.ToList()[i].CatalogueInConfig.CatalogueId && catas[i].catalogueId == cir.CatalogueId)
                                    {
                                        catas[i].differentPoint = test.DetailResult.ToList()[i].Point - cir.Point;
                                        if (catas[i].differentPoint >= 0)
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
                            }
                            else if(test.RankId == test.PotentialRankId)
                            {
                                pos = getRankPos(ranks, test.RankId);
                                if (pos == -1)
                                {
                                    nextRank = null;
                                    break;
                                }
                                nextRank = ranks[pos + 1].Name;
                                for (int k = 0; k < ranks[pos].CatalogueInRank.Count; k++)
                                {
                                    CatalogueInRank cir = ranks[pos].CatalogueInRank.ToList()[k];
                                    if (cir.CatalogueId == test.DetailResult.ToList()[i].CatalogueInConfig.CatalogueId && catas[i].catalogueId == cir.CatalogueId)
                                    {
                                        catas[i].differentPoint = test.DetailResult.ToList()[i].Point - cir.Point;
                                        if (catas[i].differentPoint >= 0)
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
                            }
                            else
                            {
                                foreach (CatalogueInRank cir in catalogueInRanks)
                                {

                                    if (cir.CatalogueId == test.DetailResult.ToList()[i].CatalogueInConfig.CatalogueId && cir.RankId == test.PotentialRankId)
                                    {
                                        catas[i].differentPoint = test.DetailResult.ToList()[i].Point - cir.Point;
                                        if (catas[i].differentPoint >= 0)
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
                }
                string potentialRank = test.PotentialRank == null ? AppConstrain.UNKNOWN_RANK : test.PotentialRank.Name;
                double statisticPoint = AppConstrain.RoundDownNumber(test.Point.Value, AppConstrain.SCALE_DOWN_NUMB);
                return new CandidateResultDTO(test.AccountId, configurationRankDTOs, catas,
                    catalogueInRankDTOs, catalogueInConfigs, statisticPoint,
                    test.RankId, (test.Rank == null ? AppConstrain.UNKNOWN_RANK : test.Rank.Name),
                    test.PotentialRankId, potentialRank, lowerTestPercent, nextRank);
            }
        }
        public static int getRankPos(List<Rank> ranks, int? rankId)
        {
            for(int i = 0; i < ranks.Count - 1; i++)
            {
                if(ranks[i].RankId == rankId)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Lấy lịch sử làm kiểm tra của nhân viên
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static List<TestHistoryDTO> GetHistory(int? accountId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                List<Test> tests = db.Test
                    .Include(t => t.Account)
                    .Include(t => t.DetailResult)
                    .ThenInclude(t => t.CatalogueInConfig)
                    .Where(t => t.AccountId == accountId)
                    .ToList();
                if (tests.Count == 0) return null;
                List<Rank> ranks = db.Rank
                    .Where(r => r.CompanyId == tests.ToList()[0].Account.CompanyId)
                    .ToList();
                List<TestHistoryDTO> testHistories = new List<TestHistoryDTO>();

                List<Catalogue> catalogues = db.Catalogue
                    .Include(c => c.CatalogueInConfiguration)
                    .ThenInclude(c => c.Config)
                    .Where(c => c.CompanyId == tests.ToList()[0].Account.CompanyId)
                    .ToList();
                for (int i = 0; i < tests.Count; i++)
                {
                    TestHistoryDTO test = new TestHistoryDTO();
                    test.testId = tests[i].TestId;
                    test.title = tests[i].Config.Title;
                    test.point = tests[i].Point;
                    test.rankId = tests[i].RankId;
                    test.createDate = tests[i].CreateDate;
                    test.startTime = tests[i].StartTime;
                    test.status = tests[i].Status;
                    foreach (Rank r in ranks)
                    {
                        if (tests[i].RankId == null)
                        {
                            test.rank = AppConstrain.UNKNOWN_RANK;
                            break;
                        }
                        if (r.RankId == tests[i].RankId)
                        {
                            test.rank = r.Name;
                            break;
                        }
                    }
                    List<CatalogueDTO> cloneCatalogues = catalogues.Select(c => new CatalogueDTO(c.CatalogueId, c.Name, 0, 0)).ToList();
                    foreach (DetailResult ds in tests[i].DetailResult.ToList())
                    {
                        for (int j = 0; j < cloneCatalogues.Count; j++)
                        {
                            if (cloneCatalogues[j].catalogueId == ds.CatalogueInConfig.CatalogueId)
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

        /// <summary>
        /// Lấy thông tin bài test
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        public static object GetInfoByTestId(int testId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var info = db.Test
                    .Include(x => x.Account)
                    .Include(x => x.Applicant)
                    .Where(x => x.TestId == testId)
                    .FirstOrDefault();
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
