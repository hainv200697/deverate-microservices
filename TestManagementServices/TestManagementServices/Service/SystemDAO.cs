using AuthenServices.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TestManagementServices.Model;
using TestManagementServices.Models;
using TestManagementServices.RabbitMQ;

namespace TestManagementServices.Service
{
    public class SystemDAO
    {
        public static string SendTestMail(int configId, bool isUpdate)
        {
            using (DeverateContext db = new DeverateContext())
            {
                {
                    var result = from c in db.Configuration
                                 join t in db.Test on c.ConfigId equals t.ConfigId
                                 join a in db.Account on t.AccountId equals a.AccountId
                                 where c.ConfigId == configId && c.IsActive == true
                                 select new TestMailDTO(a.Email, a.Fullname, c.Title, t.StartDate, t.EndDate,
                                 isUpdate == false ? t.Code : null, t.TestId.ToString());
                    if (result.ToList().Count == 0)
                    {
                        return null;
                    }
                    List<TestMailDTO> mails = result.ToList();
                    string message = JsonConvert.SerializeObject(mails);
                    Producer.PublishMessage(message, AppConstrain.TEST_MAIL);
                }
                return Message.sendMailSucceed;
            }
        }

        /// <summary>
        /// Set trạng thái quá hạn cho bài kiểm tra
        /// </summary>
        /// <param name="testId"></param>
        public static void ExpireTest(int testId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var test = context.Test.FirstOrDefault(t => t.TestId == testId);
                test.Status = AppConstrain.EXPIRED;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Trộn câu hỏi
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        public static List<QuestionDTO> Shuffle(List<QuestionDTO> questions)
        {
            Random rand = new Random();
            int n = questions.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                QuestionDTO value = questions[k];
                questions[k] = questions[n];
                questions[n] = value;
            }
            return questions;
        }

        /// <summary>
        /// Lấy ID của ứng viên theo bài test
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        public static int GetApplicantId(int testId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var test = db.Test.FirstOrDefault(t => t.TestId == testId);
                if (test.ApplicantId == null)
                {
                    return 0;
                }
                return test.ApplicantId.Value;
            }
        }

        /// <summary>
        /// Trộn câu hỏi
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        public static List<QuestionDTO> ShuffleQuestion(List<QuestionDTO> questions)
        {
            Random rand = new Random();
            int n = questions.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                QuestionDTO value = questions[k];
                questions[k] = questions[n];
                questions[n] = value;
            }
            return questions;
        }

        /// <summary>
        /// Trộn câu hỏi
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        public static List<Question> ShuffleQuestion(List<Question> questions)
        {
            Random rand = new Random();
            int n = questions.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Question value = questions[k];
                questions[k] = questions[n];
                questions[n] = value;
            }
            return questions;
        }

        /// <summary>
        /// Tạo test cho ứng viên
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="applicants"></param>
        /// <param name="oneForAll"></param>
        /// <returns></returns>
        public static string GenerateTestForApplicants(string configId, List<ApplicantDTO> applicants, DateTime startDate, DateTime endDate, bool oneForAll = false)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Configuration con = db.Configuration
                    .Include(c => c.CatalogueInConfiguration)
                    .ThenInclude(t => t.Catalogue)
                    .ThenInclude(t => t.Question)
                    .SingleOrDefault(o => o.ConfigId == Int32.Parse(configId));
                if (con.Duration < AppConstrain.MIN_DURATION)
                {
                    return Message.durationExceptopn;
                }
                List<CatalogueDTO> catas = GetCatalogueWeights(con.ConfigId);
                if (catas.Count == 0 || catas == null)
                {
                    return Message.noCatalogueException;
                }
                if (applicants.Count == 0)
                {
                    return Message.noApplicantException;
                }
                CreateTestForApplicant(applicants, con, startDate, endDate, oneForAll);
                List<int> applicantIds = new List<int>();
                foreach (ApplicantDTO applicant in applicants)
                {
                    applicantIds.Add(applicant.applicantId);
                }
                SendMailQuizCode(applicantIds, false);
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra xem đã tạo bài test chưa
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="tests"></param>
        /// <returns></returns>
        public static int isAvailableTest(int? accountId, List<Test> tests)
        {
            for (int i = 0; i < tests.Count; i++)
            {
                if (tests[i].AccountId == accountId) return i;
            }
            return -1;
        }

        /// <summary>
        /// Disable những bài test cũ
        /// </summary>
        /// <param name="accountIds"></param>
        /// <param name="tests"></param>
        /// <returns></returns>
        public static List<Test> removeAvailableTests(List<int?> accountIds, List<Test> tests)
        {
            for (int i = 0; i < tests.Count; i++)
            {
                for (int j = 0; j < accountIds.Count; j++)
                {
                    if (accountIds[j] == tests[i].AccountId)
                    {
                        tests[i].IsActive = false;
                        break;
                    }
                }
            }
            return tests;
        }

        /// <summary>
        /// Tạo bài test cho nhân viên
        /// </summary>
        /// <param name="accountIds"></param>
        /// <param name="configId"></param>
        /// <param name="oneForAll"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static string GenerateTest(List<int> accountIds, string configId, DateTime startDate, DateTime endDate, bool oneForAll = false)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Configuration con = db.Configuration
                    .Include(c => c.CatalogueInConfiguration)
                    .ThenInclude(t => t.Catalogue)
                    .ThenInclude(t => t.Question)
                    .SingleOrDefault(o => o.ConfigId == Int32.Parse(configId));
                if (con.Duration < AppConstrain.MIN_DURATION)
                {
                    return Message.durationExceptopn;
                }
                List<CatalogueDTO> catas = GetCatalogueWeights(con.ConfigId);
                if (catas.Count == 0)
                {
                    return Message.noCatalogueException;
                }
                List<AccountDTO> accounts = db.Account
                    .Where(a => accountIds.Contains(a.AccountId) && a.IsActive == true)
                    .Select(a => new AccountDTO(a))
                    .ToList();
                if (accounts.Count == 0)
                {
                    return Message.noEmployeeException;
                }
                List<int> testIds = CreateTestForEmployee(accounts, con, startDate, endDate, oneForAll);
                SendMailQuizCode(testIds, true);
                return null;
            }
        }

        /// <summary>
        /// Tạo bài test mẫu
        /// </summary>
        /// <param name="sampleConfig"></param>
        /// <returns></returns>
        public static SampleTestDTO CreateSampleTest(SampleConfigDTO sampleConfig)
        {
            using (DeverateContext db = new DeverateContext())
            {
                List<QuestionDTO> questions = new List<QuestionDTO>();
                List<int> catalogueIds = new List<int>();
                foreach(CatalogueInSampleTestDTO sc in sampleConfig.catalogueInSamples)
                {
                    catalogueIds.Add(sc.companyCatalogueId);
                }

                List<CatalogueDTO> companyCatalogues = db.Catalogue.Include(c => c.Question)
                                                            .ThenInclude(c => c.Answer)
                                                            .Where(c => catalogueIds.Contains(c.CatalogueId))
                                                            .Select(c => new CatalogueDTO(c.CatalogueId, c.Name, 0, 0, c.Question.ToList()))
                                                            .ToList();
                for (int i = 0; i < companyCatalogues.Count; i++)
                {
                    foreach (CatalogueInSampleTestDTO sc in sampleConfig.catalogueInSamples)
                    {
                        if(sc.companyCatalogueId == companyCatalogues[i].catalogueId)
                        {
                            companyCatalogues[i].numberOfQuestion = sc.numberQuestion > companyCatalogues[i].questionList.Count ? companyCatalogues[i].questionList.Count : sc.numberQuestion;
                            companyCatalogues[i].questionList = ShuffleQuestion(companyCatalogues[i].questionList.ToList());
                            List<Question> tempQuestions = companyCatalogues[i].questionList.Take(companyCatalogues[i].numberOfQuestion).ToList();
                            foreach (Question q in tempQuestions)
                            {
                                questions.Add(new QuestionDTO(q.QuestionId, q.QuestionText, q.Answer.ToList()));
                            }
                            companyCatalogues[i].questionList = null;
                        }
                    }
                }
                questions = ShuffleQuestion(questions);
                return new SampleTestDTO(companyCatalogues, questions);
            }
        }

        /// <summary>
        /// Tạo bài test cho ứng viên
        /// </summary>
        /// <param name="applicants"></param>
        /// <param name="config"></param>
        /// <param name="oneForAll"></param>
        public static void CreateTestForApplicant(List<ApplicantDTO> applicants, Configuration config, DateTime startDate, DateTime endDate, bool oneForAll = false)
        {
            using (DeverateContext db = new DeverateContext())
            {
                List<QuestionDTO> questions = new List<QuestionDTO>();
                List<CatalogueInConfiguration> catalogues = config.CatalogueInConfiguration.ToList();
                for (int i = 0; i < catalogues.Count; i++)
                {
                    catalogues[i].NumberQuestion = catalogues[i].NumberQuestion > catalogues[i].Catalogue.Question.Count ? catalogues[i].Catalogue.Question.Count : catalogues[i].NumberQuestion;
                }
                List<Test> tests = new List<Test>();
                if (oneForAll == true)
                {
                    for (int i = 0; i < catalogues.Count; i++)
                    {
                        catalogues[i].Catalogue.Question = ShuffleQuestion(catalogues[i].Catalogue.Question.ToList());
                        List<Question> tempQuestions = catalogues[i].Catalogue.Question.Take(catalogues[i].NumberQuestion).ToList();
                        foreach (Question q in tempQuestions)
                        {
                            questions.Add(new QuestionDTO(q.QuestionId, q.QuestionText, q.Answer.ToList()));
                        }
                    }
                    foreach (ApplicantDTO app in applicants)
                    {
                        questions = ShuffleQuestion(questions);
                        List<QuestionInTest> inTests = new List<QuestionInTest>();
                        foreach (QuestionDTO q in questions)
                        {
                            QuestionInTest qit = new QuestionInTest()
                            {
                                QuestionId = q.questionId,
                                IsActive = true
                            };
                            inTests.Add(qit);
                        }

                        Test t = new Test()
                        {
                            ApplicantId = app.applicantId,
                            QuestionInTest = inTests,
                            ConfigId = config.ConfigId,
                            CreateDate = DateTime.UtcNow,
                            Code = AppConstrain.GenerateCode(),
                            Status = AppConstrain.PENDING,
                            IsActive = true,
                            StartDate = startDate,
                            EndDate = endDate
                        };
                        tests.Add(t);
                    }
                }
                else
                {
                    foreach (ApplicantDTO app in applicants)
                    {
                        for (int i = 0; i < catalogues.Count; i++)
                        {
                            catalogues[i].Catalogue.Question = ShuffleQuestion(catalogues[i].Catalogue.Question.ToList());
                            List<Question> tempQuestions = catalogues[i].Catalogue.Question.Take(catalogues[i].NumberQuestion).ToList();
                            foreach (Question q in tempQuestions)
                            {
                                questions.Add(new QuestionDTO(q.QuestionId, q.QuestionText, q.Answer.ToList()));
                            }
                        }
                        questions = ShuffleQuestion(questions);
                        List<QuestionInTest> inTests = new List<QuestionInTest>();
                        foreach (QuestionDTO q in questions)
                        {
                            QuestionInTest qit = new QuestionInTest()
                            {
                                QuestionId = q.questionId,
                                IsActive = true
                            };
                            inTests.Add(qit);
                        }
                        Test t = new Test()
                        {
                            ApplicantId = app.applicantId,
                            QuestionInTest = inTests,
                            ConfigId = config.ConfigId,
                            CreateDate = DateTime.UtcNow,
                            Code = AppConstrain.GenerateCode(),
                            Status = AppConstrain.PENDING,
                            IsActive = true,
                            StartDate = startDate,
                            EndDate = endDate
                        };
                        tests.Add(t);
                        questions = new List<QuestionDTO>();
                    }
                }
                db.Test.AddRange(tests);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Tạo bài test cho nhân viên công ty
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="config"></param>
        /// <param name="oneForAll"></param>
        /// <returns></returns>
        public static List<int> CreateTestForEmployee(List<AccountDTO> accounts, Configuration config, DateTime startDate, DateTime endDate, bool oneForAll = false)
        {
            using (DeverateContext db = new DeverateContext())
            {
                List<QuestionDTO> questions = new List<QuestionDTO>();
                List<CatalogueInConfiguration> catalogues = config.CatalogueInConfiguration.ToList();
                for (int i = 0; i < catalogues.Count; i++)
                {
                    catalogues[i].NumberQuestion = catalogues[i].NumberQuestion > catalogues[i].Catalogue.Question.Count ? catalogues[i].Catalogue.Question.Count : catalogues[i].NumberQuestion;
                }
                List<Test> generatedTest = db.Test.Where(c => c.ConfigId == config.ConfigId).ToList();
                List<Test> tests = new List<Test>();
                if (oneForAll == true)
                {
                    for (int i = 0; i < catalogues.Count; i++)
                    {
                        catalogues[i].Catalogue.Question = ShuffleQuestion(catalogues[i].Catalogue.Question.ToList());
                        List<Question> tempQuestions = catalogues[i].Catalogue
                            .Question.Take(catalogues[i].NumberQuestion).ToList();
                        foreach (Question q in tempQuestions)
                        {
                            questions.Add(new QuestionDTO(q.QuestionId, q.QuestionText, q.Answer.Where(a => a.IsActive == true).ToList()));
                        }

                    }
                    foreach(AccountDTO acc in accounts)
                    {
                        foreach(Test tmp in generatedTest)
                        {
                            if(tmp.AccountId == acc.accountId)
                            {
                                continue;
                            }
                        }
                        questions = ShuffleQuestion(questions);
                        List<QuestionInTest> inTests = new List<QuestionInTest>();
                        foreach(QuestionDTO q in questions)
                        {
                            QuestionInTest qit = new QuestionInTest()
                            {
                                QuestionId = q.questionId,
                                IsActive = true
                            };
                            inTests.Add(qit);
                        }
                        Test t = new Test()
                        {
                            AccountId = acc.accountId,
                            QuestionInTest = inTests,
                            ConfigId = config.ConfigId,
                            CreateDate = DateTime.UtcNow,
                            Code = AppConstrain.GenerateCode(),
                            Status = AppConstrain.PENDING,
                            IsActive = true,
                            StartDate = startDate,
                            EndDate = endDate
                        };
                        tests.Add(t);
                    }
                }
                else
                {

                    foreach (AccountDTO acc in accounts)
                    {
                        foreach (Test tmp in generatedTest)
                        {
                            if (tmp.AccountId == acc.accountId)
                            {
                                continue;
                            }
                        }
                        for (int i = 0; i < catalogues.Count; i++)
                        {
                            catalogues[i].Catalogue.Question = ShuffleQuestion(catalogues[i].Catalogue.Question.ToList());
                            List<Question> tempQuestions = catalogues[i].Catalogue.Question.Take(catalogues[i].NumberQuestion).ToList();
                            foreach (Question q in tempQuestions)
                            {
                                questions.Add(new QuestionDTO(q.QuestionId, q.QuestionText, q.Answer.Where(a => a.IsActive == true).ToList()));
                            }

                        }
                        questions = ShuffleQuestion(questions);
                        List<QuestionInTest> inTests = new List<QuestionInTest>();
                        foreach (QuestionDTO q in questions)
                        {
                            QuestionInTest qit = new QuestionInTest()
                            {
                                QuestionId = q.questionId,
                                IsActive = true
                            };
                            inTests.Add(qit);
                        }

                        Test t = new Test()
                        {
                            AccountId = acc.accountId,
                            QuestionInTest = inTests,
                            ConfigId = config.ConfigId,
                            CreateDate = DateTime.UtcNow,
                            Code = AppConstrain.GenerateCode(),
                            Status = AppConstrain.PENDING,
                            IsActive = true,
                            StartDate = startDate,
                            EndDate = endDate
                        };
                        tests.Add(t);
                        questions = new List<QuestionDTO>();
                    }
                }
                db.Test.AddRange(tests);
                db.SaveChanges();
                List<int> testIds = new List<int>();
                foreach(Test t in tests)
                {
                    testIds.Add(t.TestId);
                }
                return testIds;
            }
        }

        /// <summary>
        /// Lấy danh sách câu hỏi từng catalogue
        /// </summary>
        /// <param name="db"></param>
        /// <param name="catalogueId"></param>
        /// <returns></returns>
        public static List<QuestionDTO> GetQuestionOfCatalogue(DeverateContext db, int? catalogueId)
        {
            List<QuestionDTO> questions = db.Question
                               .Include(c => c.Catalogue)
                               .Where(c => c.CatalogueId == catalogueId && c.IsActive == true)
                               .Select(c => new QuestionDTO(c.QuestionId, c.QuestionText, null)).ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                questions[i].answers = GetAnswerOfQuestion(questions[i].questionId);
            }
            return questions;
        }

        /// <summary>
        /// Lấy danh sách câu trả lời của câu hỏi
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public static List<AnswerDTO> GetAnswerOfQuestion(int? questionId)
        {
            using(DeverateContext db = new DeverateContext())
            {
                List<AnswerDTO> answerDTOs = db.Answer
                .Include(a => a.Question)
                .Where(a => a.Question.QuestionId == questionId && a.IsActive == true)
                .Select(a => new AnswerDTO(a)).ToList();
                return answerDTOs;
            }
        }

        public static List<Answer> GetAnswers(int questionId)
        {
            using(DeverateContext db = new DeverateContext())
            {
                List<Answer> answers = db.Answer
                    .Where(a => a.QuestionId == questionId && a.IsActive == true)
                    .ToList();
                return answers;
            }
        }

        /// <summary>
        /// Lấy số câu hỏi từng catalogue
        /// </summary>
        /// <param name="db"></param>
        /// <param name="totalQuestion"></param>
        /// <param name="catalogues"></param>
        /// <returns></returns>
        public static List<CatalogueDTO> GetNumberOfQuestionEachCatalogue(DeverateContext db, int? totalQuestion, List<CatalogueDTO> catalogues)
        {
            if (totalQuestion == 0)
            {
                return catalogues;
            }
            int currentQuestion = 0;
            for (int i = 0; i < catalogues.Count; i++)
            {
                double numberOfQuestion = catalogues[i].weightPoint == null || catalogues[i].weightPoint.Value <= 0 ? 0 : catalogues[i].weightPoint.Value * totalQuestion.Value / AppConstrain.SCALE_UP_NUMB;
                catalogues[i].numberOfQuestion = Convert.ToInt32(numberOfQuestion);
                if (catalogues[i].numberOfQuestion == 0)
                {
                    catalogues[i].numberOfQuestion = 1;
                }
                currentQuestion += catalogues[i].numberOfQuestion;
            }
            int dif = currentQuestion - totalQuestion.Value;
            catalogues[catalogues.Count - 1].numberOfQuestion = catalogues[catalogues.Count - 1].numberOfQuestion - dif;
            return catalogues;
        }

        /// <summary>
        /// Lấy trọng số của các catalogue
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public static List<CatalogueDTO> GetCatalogueWeights( int? configId)
        {
            using(DeverateContext db = new DeverateContext())
            {

                List<CatalogueDTO> companyCatalogues = db.CatalogueInConfiguration
                                           .Include(c => c.Config)
                                           .Where(c => c.ConfigId == configId)
                                           .Select(c => new CatalogueDTO(c.CatalogueId, c.Catalogue.Name,
                                           0, c.WeightPoint, null, c.Catalogue.IsActive))
                                           .ToList();
                if (companyCatalogues.Count == 0)
                {
                    return null;
                }
                return companyCatalogues;
            }
        }

        /// <summary>
        /// Tính toán rank mà ứng viên đạt được
        /// </summary>
        /// <param name="userTest"></param>
        /// <returns></returns>
        public static RankPoint EvaluateRank(UserTest userTest)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Test test = db.Test.Include(t => t.Account)
                    .Include(t => t.Config.Company)
                    .Include(t => t.Config)
                    .ThenInclude(t => t.CatalogueInConfiguration)
                    .SingleOrDefault(t => t.TestId == userTest.testId && t.Code == userTest.code);
                if (test == null)
                {
                    return null;
                }
                test.Status = AppConstrain.SUBMITTED;
                test.FinishTime = DateTime.UtcNow;
                db.SaveChanges();
                List<AnswerDTO> answers = new List<AnswerDTO>();
                List<int?> answerIds = new List<int?>();
                for (int i = 0; i < userTest.questionInTest.Count; i++)
                {
                    answerIds.Add(userTest.questionInTest[i].answerId);
                }
                var anss = db.Answer.Where(a => answerIds.Contains(a.AnswerId)).ToList();
                double totalPoint = 0;
                if (anss.Count != 0)
                {
                    anss.ForEach(a => answers.Add(new AnswerDTO(a)));
                    TestAnswerDTO testAnswer = new TestAnswerDTO(answers, userTest.testId);
                    totalPoint = CalculateResultPoint(testAnswer, test, userTest.testId, test.ConfigId);
                    List<int> rankIds = new List<int>();
                    List<RankInConfig> rankInConfigs = db.RankInConfig
                        .Where(r => r.ConfigId == test.ConfigId)
                        .ToList();
                    rankInConfigs.ForEach(r => rankIds.Add(r.RankId));
                    List<CatalogueInRank> catalogueInRanks = db.CatalogueInRank
                        .Where(r => rankIds.Contains(r.RankId))
                        .ToList();
                    totalPoint = AppConstrain.RoundDownNumber(totalPoint, AppConstrain.SCALE_DOWN_NUMB);
                    List<ConfigurationRankDTO> configurationRanks = GetRankPoint(test);
                    configurationRanks = configurationRanks.OrderBy(o => o.point).ToList();
                    ConfigurationRankDTO tmp = new ConfigurationRankDTO();
                    tmp.rankId = -1;
                    tmp.point = 0;
                    int potentialRankId = configurationRanks[0].rankId;
                    foreach (ConfigurationRankDTO cr in configurationRanks)
                    {
                        if (totalPoint > cr.point)
                        {
                            potentialRankId = cr.rankId;
                            bool isPass = true;
                            foreach(CatalogueInRank cir in catalogueInRanks)
                            {
                                if(cir.RankId == cr.rankId)
                                {
                                    foreach (DetailResult dr in test.DetailResult)
                                    {
                                        if (dr.CatalogueInConfig.CatalogueId == cir.CatalogueId)
                                        {
                                            if (dr.Point < cir.Point)
                                            {
                                                isPass = false;
                                                break;
                                            }
                                        }
                                    } 
                                }
                            }
                            if(isPass == true)
                            {
                                tmp = cr;
                            }
                        }
                    }
                    if(tmp.rankId == -1)
                    {
                        test.RankId = null;
                    }
                    else
                    {
                        foreach (RankInConfig r in rankInConfigs)
                        {
                            if (r.RankId == tmp.rankId)
                            {
                                if(totalPoint < r.Point)
                                {
                                    test.RankId = null;
                                }
                                else
                                {
                                    test.RankId = tmp.rankId;
                                }
                            }
                        }
                    }
                    test.PotentialRankId = potentialRankId;
                    test.Point = totalPoint;
                }
                else
                {
                    List<CatalogueInConfiguration> catalogueInConfigurations = db.CatalogueInConfiguration
                        .Where(c => c.ConfigId == test.ConfigId)
                        .ToList();
                    List<DetailResult> details = new List<DetailResult>();
                    foreach(CatalogueInConfiguration cic in catalogueInConfigurations)
                    {
                        DetailResult dr = new DetailResult
                        {
                            CatalogueInConfigId = cic.CatalogueInConfigId,
                            IsActive = true,
                            Point = 0,
                        };
                        details.Add(dr);
                    }
                    test.DetailResult = details;
                    test.Point = 0;
                }
                db.SaveChanges();
                return new RankPoint(test.RankId.ToString(), 0);
            }
        }

        /// <summary>
        /// Tự động lưu đáp án của ứng viên
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userTest"></param>
        /// <returns></returns>
        public static bool AutoSaveAnswer(DeverateContext db, UserTest userTest)
        {
            Test test = db.Test.SingleOrDefault(c => c.TestId == userTest.testId);
            if (test.Code != userTest.code)
            {
                return false;
            }
            SaveAnswer(userTest);
            return true;
        }

        /// <summary>
        /// Lưu phần đáp án của ứng viên
        /// </summary>
        /// <param name="userTest"></param>
        public static void SaveAnswer(UserTest userTest)
        {
            using (DeverateContext db = new DeverateContext())
            {
                List<int> questionIds = new List<int>();
                for (int i = 0; i < userTest.questionInTest.Count; i++)
                {
                    questionIds.Add(userTest.questionInTest[i].questionId);
                }
                var qitss = db.QuestionInTest.Where(o => questionIds.Contains(o.QuestionId) && o.TestId == userTest.testId).ToList();
                for (int i = 0; i < qitss.Count; i++)
                {
                    for (int j = 0; j < userTest.questionInTest.Count; j++)
                    {
                        if (qitss[i].QuestionId == userTest.questionInTest[j].questionId)
                            if(userTest.questionInTest[j].answerId != null)
                            {
                                qitss[i].AnswerId = userTest.questionInTest[j].answerId.Value;
                            }
                            else
                            {
                                qitss[i].AnswerId = userTest.questionInTest[j].answerId;
                            }
                    }

                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Lấy danh sách rank + điểm
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public static List<ConfigurationRankDTO> GetRankPoint(Test test)
        {
            using(DeverateContext db = new DeverateContext())
            {
                List<ConfigurationRankDTO> rankInConfigs = db.RankInConfig
                    .OrderBy(r => r.Point)
                    .Where(r => r.ConfigId == test.ConfigId)
                    .Select(r => new ConfigurationRankDTO(r.RankId, r.Rank.Name, r.Point))
                    .ToList();
                return rankInConfigs;
            }
        }

        /// <summary>
        /// Tính điểm kết quả của bài test
        /// </summary>
        /// <param name="answers"></param>
        /// <param name="test"></param>
        /// <param name="companyId"></param>
        /// <param name="testId"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public static double CalculateResultPoint(TestAnswerDTO answers, Test test, int? testId, int configId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                double totalPoint = 0;
                List<CataloguePointDTO> cataloguePoints = CalculateCataloguePoints(db, answers, configId, testId);
                List<CatalogueWeightPointDTO> catalogueWeightPoints = GetWeightPoints(answers.testId);

                List<int> catalogueIds = new List<int>();
                cataloguePoints.ForEach(c => catalogueIds.Add(c.catalogueId));
                List<CatalogueInConfiguration> catalogueInConfigurations = db.CatalogueInConfiguration.Where(c => c.ConfigId == configId).ToList();
                for(int i = 0; i < catalogueIds.Count; i++)
                {
                    for(int j = 0; j < catalogueInConfigurations.Count; j++)
                    {
                        if(catalogueIds[i] == catalogueInConfigurations[j].CatalogueId)
                        {
                            cataloguePoints[i].catalogueId = catalogueInConfigurations[j].CatalogueInConfigId;
                            
                        }
                    }
                }
                List<DetailResult> details = new List<DetailResult>();
                for (int i = 0; i < cataloguePoints.Count; i++)
                {
                    DetailResult detail = new DetailResult();
                    detail.CatalogueInConfigId = cataloguePoints[i].catalogueId;
                    if (cataloguePoints[i].cataloguePoint < 0)
                    {
                        continue;
                    }
                    double point = cataloguePoints[i].cataloguePoint * catalogueWeightPoints[i].weightPoint / AppConstrain.SCALE_UP_NUMB;
                    detail.Point = cataloguePoints[i].cataloguePoint;
                    detail.IsActive = true;
                    detail.TestId = test.TestId;
                    detail.CatalogueInConfig = db.CatalogueInConfiguration.Where(c => c.CatalogueInConfigId == detail.CatalogueInConfigId).FirstOrDefault();
                    details.Add(detail);
                    
                    totalPoint += point;
                }
                if (details.Count > 0)
                {
                    test.DetailResult = details;
                }
                return totalPoint;
            }
           
        }

        /// <summary>
        /// Lấy trọng số từng catalogue
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        public static List<CatalogueWeightPointDTO> GetWeightPoints(int? testId)
        {
            using(DeverateContext db = new DeverateContext())
            {
                var result = from t in db.Test
                             join cf in db.Configuration on t.ConfigId equals cf.ConfigId
                             join cif in db.CatalogueInConfiguration on cf.ConfigId equals cif.ConfigId
                             where t.TestId == testId
                             select new CatalogueWeightPointDTO(cif.CatalogueId, cif.WeightPoint);
                if (result == null)
                {
                    return null;
                }
                return result.ToList();
            }

        }

        /// <summary>
        /// Tính điểm từng catalogue
        /// </summary>
        /// <param name="db"></param>
        /// <param name="answers"></param>
        /// <param name="companyId"></param>
        /// <param name="testId"></param>
        /// <returns></returns>
        public static List<CataloguePointDTO> CalculateCataloguePoints(DeverateContext db, TestAnswerDTO answers, int configId, int? testId)
        {
            var cataInCompany = db.CatalogueInConfiguration.Where(c => c.ConfigId == configId).Select(c => c.Catalogue).ToList();
            List<CataloguePointDTO> cataloguePoints = new List<CataloguePointDTO>();
            List<AnswerDTO> anss = new List<AnswerDTO>(answers.answers);
            List<int?> questIds = new List<int?>();
            anss.ForEach(a => questIds.Add(a.answerId));
            var quess = db.Answer.Include(a => a.Question).Where(an => questIds.Contains(an.AnswerId)).ToList();
            var ans = db.QuestionInTest
                .Include(q => q.Question)
                .ThenInclude(q => q.Answer)
                .Where(q => q.TestId == testId)
                .ToList();
            foreach(Catalogue cata in cataInCompany)
            {
                double point = 0;
                double maxPoint = 0;
                List<int?> ids = new List<int?>();
                for (int i = 0; i < quess.Count; i++)
                {
                    if (quess[i].Question.CatalogueId == cata.CatalogueId)
                    {
                        maxPoint += quess[i].Question.Point;
                        point += quess[i].Percent * quess[i].Question.Point / AppConstrain.SCALE_UP_NUMB;
                        ids.Add(quess[i].QuestionId);
                        quess.RemoveAt(i);
                        i--;

                    }
                }
                for (int i = 0; i < ans.Count; i++)
                {
                    if (ids.Contains(ans[i].QuestionId))
                    {
                        ans.RemoveAt(i);
                        i--;
                    }
                    else if (cata.CatalogueId == ans[i].Question.CatalogueId)
                    {
                        maxPoint += ans[i].Question.Point;
                    }
                }


                double cataloguePoint = maxPoint == 0 ? 0 : (point / maxPoint) * AppConstrain.SCALE_UP_NUMB;
                cataloguePoints.Add(new CataloguePointDTO(cata.CatalogueId, cataloguePoint));
            }
            return cataloguePoints;
        }

        /// <summary>
        /// Lấy danh sách bài test của ứng viên
        /// </summary>
        /// <param name="db"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static List<TestInfoDTO> GetAllTestTodayByUsername(DeverateContext db, int accountId)
        {
            List<TestInfoDTO> tests = db.Test
                .Include(t => t.Config)
                .Include(t => t.Account)
                .Where(t => t.AccountId == accountId && t.IsActive == true)
                .Select(t => new TestInfoDTO(t.ConfigId, t.AccountId,
                t.TestId, t.Config.Title, null, t.Status, t.Config.CreateDate,
                t.Config.ExpiredDays, t.Config.IsActive))
                .ToList();
            return tests;
        }

        /// <summary>
        /// Lấy thông tin config
        /// </summary>
        /// <param name="db"></param>
        /// <param name="testId"></param>
        /// <returns></returns>
        public static ConfigurationDTO GetConfig(DeverateContext db, int testId)
        {
            var config = db.Configuration.Include(z => z.Test).Where(c => c.Test.Any(x => x.TestId == testId)).FirstOrDefault();
            var test = config.Test.SingleOrDefault(t => t.TestId == testId);
            return new ConfigurationDTO(config, test);
        }

        /// <summary>
        /// Lấy danh sách câu hỏi + câu trả lời của bài test
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public static UserTest GetQuestionInTest(TestInfoDTO testInfo, bool checkCode)
        {
            using (DeverateContext context = new DeverateContext())
            {
                UserTest testU = new UserTest();
                Test test = new Test();
                if (checkCode)
                {
                    test = context.Test.SingleOrDefault(t => t.TestId == testInfo.testId && t.Code == testInfo.code);
                    if (test == null)
                    {
                        return null;
                    }
                    if (test.StartTime == null)
                    {
                        test.StartTime = DateTime.UtcNow;
                        test.Status = AppConstrain.DOING;
                        context.SaveChanges();
                    }
                }
                else
                {
                    test = context.Test.SingleOrDefault(t => t.TestId == testInfo.testId);
                }
                if (test == null)
                {
                    return null;
                }
                var questionInTest = context.QuestionInTest
                                     .Include(x => x.Question)
                                     .ThenInclude(y => y.Answer)
                                     .Where(t => t.TestId == test.TestId);
                var result = new List<QuestionInTestDTO>();
                foreach (QuestionInTest item in questionInTest.ToList())
                {
                    result.Add(new QuestionInTestDTO(item.TestId, item.QuestionId,
                        item.AnswerId, item.Question.Answer.ToList(), item.Question.QuestionText));
                }
                testU.accountId = test.AccountId;
                testU.code = test.Code;
                testU.testId = test.TestId;
                testU.startTime = test.StartTime;
                testU.questionInTest = result;
                return testU;
            }
        }

        /// <summary>
        /// Lấy danh sách bài test của config
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<TestInfoDTO> GetTestByConfig(DeverateContext db, int id)
        {
            var results = db.Test.Where(t => t.ConfigId == id)
                .Select(t => new TestInfoDTO(t, t.Config.Title, t.Account.Username, t.Applicant.Fullname))
                .ToList();
            return results;
        }

        /// <summary>
        /// Lấy thông tin bài test
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        public static TestInfoDTO GetTestByTestId(int testId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Test.Where(t => t.TestId == testId).Select(t => new TestInfoDTO(t)).FirstOrDefault();
            }
        }

        /// <summary>
        /// Tự động submit bài test khi hết thời gian hoặc quá hạn
        /// </summary>
        /// <param name="testId"></param>
        public static void AutoSubmit(int testId)
        {
            TestInfoDTO testInfo = GetTestByTestId(testId);
            UserTest userTest = GetQuestionInTest(testInfo, false);
            EvaluateRank(userTest);
        }

        /// <summary>
        /// Gửi access code làm test đến cho ứng viên
        /// </summary>
        /// <param name="listestResendCode"></param>
        /// <param name="isEmployee"></param>
        public static void SendMailQuizCode(List<int> listestResendCode, bool isEmployee)
        {
            using (DeverateContext context = new DeverateContext())
            {
                List<TestMailDTO> list = new List<TestMailDTO>();
                if (isEmployee)
                {
                    list = context.Test
                        .Include(x => x.Account)
                        .Include(x => x.Config)
                        .Where(x => listestResendCode.Contains(x.TestId))
                         .Select(x => new TestMailDTO(
                             x.Account.Email,
                             x.Account.Fullname,
                             x.Config.Title,
                             x.StartDate,
                             x.EndDate,
                             x.Code,
                             x.TestId.ToString()
                             )
                         )
                        .ToList();
                }
                else
                {
                    list = context.Test.Include(c => c.Config).Include(a => a.Applicant).Where(x => listestResendCode.Contains(x.ApplicantId.Value))
                    .Select(x => new TestMailDTO(
                        x.Applicant.Email,
                        x.Applicant.Fullname,
                        x.Config.Title,
                        x.StartDate,
                        x.EndDate,
                        x.Code,
                        x.TestId.ToString()
                        )
                    )
                    .ToList();
                }

                Producer.PublishMessage(JsonConvert.SerializeObject(list), AppConstrain.TEST_MAIL);
            }
        }

        /// <summary>
        /// Kiểm tra access code của bài test
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int CheckCode(int testId, string code)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Test test = context.Test.SingleOrDefault(t => t.TestId == testId && t.Code == code);
                if (test != null)
                {
                    return test.ApplicantId.Value;
                }
                return 0;
            }
        }
    }
}
