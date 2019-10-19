﻿using AuthenServices.Model;
using AuthenServices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Model;
using TestManagementServices.Models;

namespace TestManagementServices.Service
{
    public class SystemDAO
    {
        /// <summary>
        /// Trộn danh sách câu hỏi
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
        /// Tạo bài test dựa trên file config
        /// </summary>
        /// <param name="db"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static string GenerateTest(string configId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                try
                {
                    Configuration con = context.Configuration.SingleOrDefault(o => o.ConfigId == Int32.Parse(configId));
                    if (con.Duration < AppConstrain.minDuration)
                    {
                        return Message.durationExceptopn;
                    }
                    List<CatalogueDTO> catas = GetCatalogueWeights(context, con.ConfigId);
                    if (catas.Count == 0)
                    {
                        return Message.noCatalogueException;
                    }
                    if (con.TotalQuestion < catas.Count)
                    {
                        return Message.numberQuestionExceptopn;
                    }
                    Account acc = context.Account.SingleOrDefault(o => o.AccountId == con.TestOwnerId);
                    int? companyId = acc.CompanyId;


                    var emps = from a in context.Account
                               where a.CompanyId == companyId && a.RoleId == AppConstrain.empRole && a.IsActive == true
                               select new AccountDTO(a);
                    List<AccountDTO> accounts = emps.ToList();
                    if (accounts.Count == 0)
                    {
                        return Message.noEmployeeException;
                    }
                    for (int i = 0; i < accounts.Count; i++)
                    {
                        GenerateQuestion(context, accounts[i].AccountId, con);
                    }
                }
                catch (Exception e)
                {
                    File.WriteAllText(AppConstrain.logFile, e.Message);
                }
                return null;
            }
        }

        /// <summary>
        /// Tạo bài kiểm tra cho người dùng dưa trên config
        /// </summary>
        /// <param name="db"></param>
        /// <param name="accountId"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<QuestionDTO> GenerateQuestion(DeverateContext db, int? accountId, Configuration config)
        {
            List<QuestionDTO> questions = new List<QuestionDTO>();
            try
            {
                Random rand = new Random();
                int totalCataQues = 0;
                List<CatalogueDTO> catalogues = GetCatalogueWeights(db, config.ConfigId);
                catalogues = catalogues.OrderByDescending(o => o.weightPoint).ToList();
                for (int i = 0; i < catalogues.Count; i++)
                {
                    catalogues[i].questions = GetQuestionOfCatalogue(db, catalogues[i].catalogueId);
                    totalCataQues += catalogues[i].questions.Count;
                }
                int totalOfQues = config.TotalQuestion > totalCataQues ? totalCataQues : config.TotalQuestion.Value;
                catalogues = GetNumberOfQuestionEachCatalogue(db, totalOfQues, catalogues);
                List<QuestionRemain> remainQues = new List<QuestionRemain>();
                for (int i = 0; i < catalogues.Count; i++)
                {
                    QuestionRemain quesRe = new QuestionRemain();
                    quesRe.catalogueId = catalogues[i].catalogueId;
                    List<int?> quesIds = new List<int?>();
                    List<QuestionDTO> totalQues = catalogues[i].questions;
                    List<QuestionDTO> choosedQues = new List<QuestionDTO>();
                    int quesLenght = totalQues.Count;
                    int numbOfQues = catalogues[i].numberOfQuestion > quesLenght ? quesLenght : catalogues[i].numberOfQuestion.Value;
                    for (int j = 0; j < numbOfQues; j++)
                    {
                        int rQues = rand.Next(0, quesLenght);
                        if (!quesIds.Contains(totalQues[rQues].questionId))
                        {
                            quesIds.Add(totalQues[rQues].questionId);
                            questions.Add(totalQues[rQues]);
                            if (numbOfQues != quesLenght)
                            {
                                choosedQues.Add(totalQues[rQues]);
                            }
                        }
                        else
                        {
                            j--;
                        }
                    }
                    quesRe.curNumbQues = numbOfQues;
                    quesRe.numbCataQues = catalogues[i].questions.Count;
                    List<QuestionDTO> unchoosedQues = new List<QuestionDTO>();
                    if (numbOfQues != quesLenght)
                    {
                        for (int k = 0; k < quesLenght; k++)
                        {
                            bool isContainQues = false;
                            for (int m = 0; m < choosedQues.Count; m++)
                            {
                                if (choosedQues[m].questionId == totalQues[k].questionId)
                                {
                                    isContainQues = true;
                                    break;
                                }
                            }
                            if (isContainQues == false)
                            {
                                unchoosedQues.Add(totalQues[k]);
                            }
                        }
                    }
                    quesRe.unchoosedQues = unchoosedQues;
                    remainQues.Add(quesRe);
                }
                questions = fillQues(remainQues, totalOfQues, questions);
                questions = Shuffle(questions);

                Test test = new Test();
                test.ConfigId = config.ConfigId;
                test.AccountId = accountId;
                test.CreateDate = DateTime.Now;
                test.IsActive = true;
                db.Test.Add(test);
                db.SaveChanges();

                test.Code = GenerateCode();
                db.SaveChanges();

                for (int i = 0; i < questions.Count; i++)
                {
                    QuestionInTest inTest = new QuestionInTest();
                    inTest.TestId = test.TestId;
                    inTest.QuestionId = questions[i].questionId;
                    inTest.IsActive = true;
                    db.QuestionInTest.Add(inTest);
                    db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                File.WriteAllText(AppConstrain.logFile, e.Message);
            }

            return questions;
        }


        public static List<QuestionDTO> fillQues(List<QuestionRemain> remains, int totalOfQues, List<QuestionDTO> questions)
        {
            Random rand = new Random();
            int? currentTotalQues = 0;
            for (int i = 0; i < remains.Count; i++)
            {
                currentTotalQues += remains[i].curNumbQues;
            }
            int remainNumbQues = totalOfQues - currentTotalQues.Value;
            if (totalOfQues <= currentTotalQues.Value)
            {
                return questions;
            }
            for (int i = 0; i < remains.Count; i++)
            {
                int unfilledQues = remains[i].numbCataQues.Value - remains[i].curNumbQues.Value;
                if (unfilledQues == 0)
                {
                    continue;
                }
                else
                {
                    int difQues = remainNumbQues > remains[i].curNumbQues ? remains[i].curNumbQues.Value : remainNumbQues;
                    for (int j = 0; j < difQues; j++)
                    {
                        int rQues = rand.Next(0, difQues);
                        if (!questions.Contains(remains[i].unchoosedQues[rQues]))
                        {
                            questions.Add(remains[i].unchoosedQues[rQues]);
                            remains[i].curNumbQues += 1;
                        }
                        else
                        {
                            j--;
                        }
                    }
                    remainNumbQues = remainNumbQues - difQues;
                }
            }

            return fillQues(remains, totalOfQues, questions);
        }

        /// <summary>
        /// Tạo code access vào bài test
        /// </summary>
        /// <returns></returns>
        public static string GenerateCode()
        {

            string code = PasswordGenerator.GeneratePassword(AppConstrain.includeLowercase, AppConstrain.includeUppercase,
                AppConstrain.includeNumeric, AppConstrain.includeSpecial,
                AppConstrain.includeSpaces, AppConstrain.lengthOfPassword);

            while (!PasswordGenerator.PasswordIsValid(AppConstrain.includeLowercase, AppConstrain.includeUppercase,
                AppConstrain.includeNumeric, AppConstrain.includeSpecial, AppConstrain.includeSpaces, code))
            {
                code = PasswordGenerator.GeneratePassword(AppConstrain.includeLowercase, AppConstrain.includeUppercase,
                    AppConstrain.includeNumeric, AppConstrain.includeSpecial,
                    AppConstrain.includeSpaces, AppConstrain.lengthOfPassword);
            }
            return code;
        }

        /// <summary>
        /// Lấy câu hỏi của từng catalogue
        /// </summary>
        /// <param name="db"></param>
        /// <param name="catalogueId"></param>
        /// <returns></returns>
        public static List<QuestionDTO> GetQuestionOfCatalogue(DeverateContext db, int? catalogueId)
        {
            try
            {
                var ques = from ca in db.Catalogue
                           join q in db.Question on ca.CatalogueId equals q.CatalogueId
                           where ca.CatalogueId == catalogueId
                           select new QuestionDTO(q.QuestionId, q.Question1, null);
                List<QuestionDTO> questions = ques.ToList();
                for (int i = 0; i < questions.Count; i++)
                {
                    questions[i].answers = GetAnswerOfQuestion(db, questions[i].questionId);
                }
                return questions;
            }
            catch (Exception e)
            {
                File.WriteAllText(AppConstrain.logFile, e.Message);
            }
            return null;
        }

        /// <summary>
        /// Lấy câu trả lời của từng đâu hỏi
        /// </summary>
        /// <param name="db"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public static List<AnswerDTO> GetAnswerOfQuestion(DeverateContext db, int? questionId)
        {
            try
            {
                var answers = from q in db.Question
                              join a in db.Answer on q.QuestionId equals a.QuestionId
                              where q.QuestionId == questionId
                              select new AnswerDTO(a);
                return answers.ToList();
            }
            catch (Exception e)
            {
                File.WriteAllText(AppConstrain.logFile, e.Message);
            }
            return null;

        }

        /// <summary>
        /// Lấy số lượng câu hỏi trong mỗi catalogue
        /// </summary>
        /// <param name="db"></param>
        /// <param name="totalQuestion"></param>
        /// <param name="catalogues"></param>
        /// <returns></returns>
        public static List<CatalogueDTO> GetNumberOfQuestionEachCatalogue(DeverateContext db, int? totalQuestion, List<CatalogueDTO> catalogues)
        {
            int currentQuestion = 0;
            for (int i = 0; i < catalogues.Count; i++)
            {
                double numberOfQuestion = catalogues[i].weightPoint.Value * totalQuestion.Value;
                catalogues[i].numberOfQuestion = Convert.ToInt32(numberOfQuestion);
                currentQuestion += catalogues[i].numberOfQuestion.Value;
            }
            int dif = currentQuestion - totalQuestion.Value;
            catalogues[catalogues.Count - 1].numberOfQuestion = catalogues[catalogues.Count - 1].numberOfQuestion.Value - dif;
            return catalogues;
        }

        /// <summary>
        /// Lấy list trọng số ứng với các catalogue có trong config
        /// </summary>
        /// <param name="db"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public static List<CatalogueDTO> GetCatalogueWeights(DeverateContext db, int? configId)
        {


            try
            {
                var result = from cf in db.Configuration
                             join cif in db.CatalogueInConfiguration on cf.ConfigId equals cif.ConfigId
                             where cf.ConfigId == configId
                             select new CatalogueDTO(cif.CatalogueId, cif.Catalogue.Name, 0, cif.WeightPoint, null, cif.Catalogue.IsActive);
                if (result.ToList().Count == 0)
                {
                    return null;
                }
                return result.ToList();
            }
            catch (Exception e)
            {
                File.WriteAllText(AppConstrain.logFile, e.Message);
            }
            return null;
        }



        /// <summary>
        /// Đánh giá rank dựa trên bài test
        /// </summary>
        /// <param name="db"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        public static RankPoint EvaluateRank(DeverateContext db, List<QuestionInTestDTO> questionInTestDTO)
        {
            List<AnswerDTO> answers = new List<AnswerDTO>();
            for (int i = 0; i < questionInTestDTO.Count; i++)
            {
                if (questionInTestDTO[i].answerId == null)
                {
                    continue;
                }
                var answerEn = db.Answer.SingleOrDefault(an => an.AnswerId == questionInTestDTO[i].answerId);
                answers.Add(new AnswerDTO(answerEn));
            }
            TestAnswerDTO test = new TestAnswerDTO(answers, questionInTestDTO[0].testId);
       
            double? totalPoint = CalculateResultPoint(db, test);
            List<ConfigurationRankDTO> configurationRanks = GetRankPoint(db, test);
            configurationRanks = configurationRanks.OrderBy(o => o.point).ToList();
            ConfigurationRankDTO tmp = new ConfigurationRankDTO();
            tmp.rankId = configurationRanks[0].rankId.Value;
            tmp.point = configurationRanks[0].point;
            foreach (ConfigurationRankDTO cr in configurationRanks)
            {
                if (tmp.point < cr.point)
                {
                    tmp = cr;
                }
            }
            string rank = db.Rank.SingleOrDefault(r => r.RankId == tmp.rankId).Name;
            if (rank == null)
            {
                return null;
            }
            return new RankPoint(rank, tmp.point);

        }

        /// <summary>
        /// Lấy danh sách điểm của từng rank được cấu hình 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public static List<ConfigurationRankDTO> GetRankPoint(DeverateContext db, TestAnswerDTO answer)
        {
            if (answer.testId == null)
            {
                return null;
            }
            var result = from con in db.Configuration
                         join te in db.Test on con.ConfigId equals te.ConfigId
                         join cr in db.ConfigurationRank on con.ConfigId equals cr.ConfigId
                         where te.TestId == answer.testId
                         select new ConfigurationRankDTO(cr);
            return result.ToList();
        }

        /// <summary>
        /// Tính điểm trên từng catalogue
        /// </summary>
        /// <param name="db"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        public static double? CalculateResultPoint(DeverateContext db, TestAnswerDTO answers)
        {
            double? totalPoint = 0;
            List<CataloguePointDTO> cataloguePoints = CalculateCataloguePoints(db, answers);
            if (answers.testId == null)
            {
                return -1;
            }
            List<CatalogueWeightPointDTO> catalogueWeightPoints = GetWeightPoints(db, answers.testId);
            for (int i = 0; i < cataloguePoints.Count; i++)
            {
                totalPoint += (cataloguePoints[i].cataloguePoint * catalogueWeightPoints[i].weightPoint);
            }
            return totalPoint;
        }

        /// <summary>
        /// Lấy danh sách trọng số của từng catalogue đã cấu hình
        /// </summary>
        /// <param name="db"></param>
        /// <param name="testId"></param>
        /// <returns></returns>
        public static List<CatalogueWeightPointDTO> GetWeightPoints(DeverateContext db, int? testId)
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

        /// <summary>
        /// Tính điểm trên từng catalogue
        /// </summary>
        /// <param name="db"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        public static List<CataloguePointDTO> CalculateCataloguePoints(DeverateContext db, TestAnswerDTO answers)
        {
            if (answers.testId == null)
            {
                return null;
            }

            //var catas = from t in db.Test
            //            join cf in db.Configuration on t.ConfigId equals cf.ConfigId
            //            join cif in db.CatalogueInConfiguration on cf.ConfigId equals cif.ConfigId
            //            join c in db.Catalogue on cif.CatalogueId equals c.CatalogueId
            //            where t.TestId == answers.testId
            //            select new CatalogueDTO(c);
            var test = db.Test.SingleOrDefault(t => t.TestId == answers.testId);
            var catalogues = db.Catalogue.Where(c => c.CatalogueInConfiguration.Any(cif => cif.ConfigId == test.ConfigId)).ToList();

            //List<CatalogueDTO> catalogues = catas.ToList();
            List<CataloguePointDTO> cataloguePoints = new List<CataloguePointDTO>();
            foreach (Catalogue cata in catalogues)
            {
                float? point = 0;
                float? maxPoint = 0;
                foreach (AnswerDTO answer in answers.answers)
                {
                    Answer ans = db.Answer.Include(a => a.Question).SingleOrDefault(an => an.AnswerId == answer.AnswerId);
                    if (ans == null)
                    {
                        continue;
                    }
                    if (ans.Question.CatalogueId == cata.CatalogueId)
                    {
                        maxPoint += ans.Question.MaxPoint;
                        point += ans.Point;
                    }
                }
                float? cataloguePoint = (point / maxPoint);
                cataloguePoints.Add(new CataloguePointDTO(cata.CatalogueId  , cataloguePoint));
            }
            return cataloguePoints;
        }

        /// <summary>
        /// Lấy danh sách các bài test của user trong ngày
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acccountId"></param>
        /// <returns></returns>
        public static List<TestInfoDTO> GetAllTestTodayByUsername(DeverateContext db, int? acccountId)
        {
            var result = from cf in db.Configuration
                         join t in db.Test on cf.ConfigId equals t.ConfigId
                         where t.AccountId == acccountId && t.IsActive == true && cf.StartDate <= DateTime.Now && DateTime.Now <= cf.EndDate
                         select new TestInfoDTO(cf.ConfigId, acccountId, t.TestId, cf.Title , null);
            return result.ToList();
        }

        public static ConfigurationDTO GetConfig(DeverateContext db, int testId)
        {
            var config = db.Configuration.Include(z=>z.Test).Where(c => c.Test.Any(x=>x.TestId == testId)).FirstOrDefault()   ;
            return new ConfigurationDTO(config);
        }

        public static List<QuestionInTestDTO> GetQuestionInTest(DeverateContext db, TestInfoDTO testInfo)
        {
            Test test = db.Test.SingleOrDefault(c => c.AccountId == testInfo.accountId && c.ConfigId == testInfo.configId);
            if (test.Code != testInfo.code)
            {
                return null;
            }
            var questionInTest = db.QuestionInTest
                                 .Include(x => x.Question)
                                 .ThenInclude(y => y.Answer)
                                 .Where(t => t.TestId == test.TestId);
            var result = new List<QuestionInTestDTO>();
            foreach (QuestionInTest item in questionInTest.ToList())
            {
                result.Add(new QuestionInTestDTO(item.Qitid, item.TestId, item.Question.Answer.ToList(), item.AnswerId, item.Question.Question1));
            }
            return result;
        }
    }
}
