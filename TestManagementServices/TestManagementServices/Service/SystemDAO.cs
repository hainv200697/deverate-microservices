﻿using AuthenServices.Model;
using AuthenServices.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Model;
using TestManagementServices.Models;
using TestManagementServices.RabbitMQ;

namespace TestManagementServices.Service
{
    public class SystemDAO
    {

        /// <summary>
        /// Gửi mail thông tin bài test đến người dùng
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="isUpdate"> true: chỉ cập nhật lại config -> gửi mail thay đổi ngày giờ
        /// false: tạo mới config gửi full thông tin
        /// </param>
        /// <returns></returns>
        public static string SendTestMail(int? configId, bool isUpdate)
        {
            using(DeverateContext db = new DeverateContext())
            {
                try
                {
                    var result = from c in db.Configuration
                                 join t in db.Test on c.ConfigId equals t.ConfigId
                                 join a in db.Account on t.AccountId equals a.AccountId
                                 where c.ConfigId == configId && c.IsActive == true
                                 select new TestMailDTO(a.Email, a.Fullname, null, c.StartDate, c.EndDate,
                                 isUpdate == false ? t.Code: null);
                    if(result.ToList().Count == 0)
                    {
                        return null;
                    }
                    List<TestMailDTO> mails = result.ToList();
                    string message = JsonConvert.SerializeObject(mails);
                    Producer.PublishMessage(message, AppConstrain.test_mail);
                }
                catch(Exception e)
                {
                    File.WriteAllText(AppConstrain.logFile, e.Message);
                }
                return Message.sendMailSucceed;
            }
        }

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
                    GenerateQuestions(context, accounts, con);
                    //for (int i = 0; i < accounts.Count; i++)
                    //{
                    //    GenerateQuestion(context, accounts[i].accountId, con);
                    //}
                }
                catch (Exception e)
                {
                    File.WriteAllText(AppConstrain.logFile, e.Message);
                }
                return null;
            }
        }


        public static void GenerateQuestions(DeverateContext db, List<AccountDTO> accounts, Configuration config)
        {
            List<QuestionDTO> questions = new List<QuestionDTO>();
            List<QuestionRemain> remainQues = new List<QuestionRemain>();
            List<QuestionDTO> unchoosedQues = new List<QuestionDTO>();
            List<QuestionDTO> choosedQues = new List<QuestionDTO>();
            List<int?> quesIds = new List<int?>();
            Random rand = new Random();
            int totalCataQues = 0;

            
            try
            {
                List<CatalogueDTO> catalogues = GetCatalogueWeights(db, config.ConfigId);
                catalogues = catalogues.OrderByDescending(o => o.weightPoint).ToList();
                for (int i = 0; i < catalogues.Count; i++)
                {
                    catalogues[i].questions = GetQuestionOfCatalogue(db, catalogues[i].catalogueId);
                    totalCataQues += catalogues[i].questions.Count;
                }
                int totalOfQues = config.TotalQuestion > totalCataQues ? totalCataQues : config.TotalQuestion.Value;
                catalogues = GetNumberOfQuestionEachCatalogue(db, totalOfQues, catalogues);


                foreach (AccountDTO acc in accounts)
                {
                    for (int i = 0; i < catalogues.Count; i++)
                    {
                        QuestionRemain quesRe = new QuestionRemain();
                        quesRe.catalogueId = catalogues[i].catalogueId;
                        
                        List<QuestionDTO> totalQues = catalogues[i].questions;
                        int quesLenght = totalQues.Count;
                        int numbOfQues = catalogues[i].numberOfQuestion > quesLenght ? quesLenght : catalogues[i].numberOfQuestion.Value;
                        totalQues = Shuffle(totalQues);
                        for (int j = 0; j < numbOfQues; j++)
                        {
                            if (!quesIds.Contains(totalQues[j].questionId))
                            {
                                quesIds.Add(totalQues[j].questionId);
                                questions.Add(totalQues[j]);
                                if (numbOfQues != quesLenght)
                                {
                                    choosedQues.Add(totalQues[j]);
                                }
                            }
                            else
                            {
                                j--;
                            }
                        }
                        quesRe.curNumbQues = numbOfQues;
                        quesRe.numbCataQues = catalogues[i].questions.Count;
                        
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
                        quesRe.weightPoint = catalogues[i].weightPoint;
                        remainQues.Add(quesRe);
                    }
                    questions = fillQues(remainQues, totalOfQues, questions);
                    questions = Shuffle(questions);

                    Test test = new Test();
                    test.ConfigId = config.ConfigId;
                    test.AccountId = acc.accountId;
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

                    questions = new List<QuestionDTO>();
                    remainQues = new List<QuestionRemain>();
                    unchoosedQues = new List<QuestionDTO>();
                    choosedQues = new List<QuestionDTO>();
                    quesIds = new List<int?>();
                }
            }
            catch (Exception e)
            {
                File.WriteAllText(AppConstrain.logFile, e.Message);
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
                    totalQues = Shuffle(totalQues);
                    for (int j = 0; j < numbOfQues; j++)
                    {
                        if (!quesIds.Contains(totalQues[j].questionId))
                        {
                            quesIds.Add(totalQues[j].questionId);
                            questions.Add(totalQues[j]);
                            if (numbOfQues != quesLenght)
                            {
                                choosedQues.Add(totalQues[j]);
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
                    quesRe.weightPoint = catalogues[i].weightPoint;
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
            int remainNumbQues = totalOfQues - questions.Count;
            if (totalOfQues <= questions.Count)
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
                    int difQues = remainNumbQues > unfilledQues ? unfilledQues : remainNumbQues;
                    difQues = Convert.ToInt32(difQues * remains[i].weightPoint);
                    if(difQues == 0)
                    {
                        difQues = 1;
                    }
                    difQues = remainNumbQues > difQues ? difQues : remainNumbQues;
                    for (int j = 0; j < difQues; j++)
                    {
                        int rQues = rand.Next(0, remains[i].unchoosedQues.Count);
                        if (!questions.Contains(remains[i].unchoosedQues[rQues]))
                        {
                            questions.Add(remains[i].unchoosedQues[rQues]);
                            remains[i].unchoosedQues.RemoveAt(rQues);
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
        /// <param name="userTest"></param>
        /// <returns></returns>
        public static RankPoint EvaluateRank(DeverateContext db, UserTest userTest)
        {
            Test test = db.Test.SingleOrDefault(t => t.TestId == userTest.testId && t.Code == userTest.code);
            if (test == null)
            {
                return null;
            }

            List<AnswerDTO> answers = new List<AnswerDTO>();
            List<int> answerIds = new List<int>();
            List<int> qitIds = new List<int>();
            for(int i = 0; i < userTest.questionInTest.Count; i++)
            {
                answerIds.Add(userTest.questionInTest[i].answerId.Value);
                qitIds.Add(userTest.questionInTest[i].qitid.Value);
            }
            var anss = db.Answer.Where(a => answerIds.Contains(a.AnswerId)).ToList();
            anss.ForEach(a => answers.Add(new AnswerDTO(a)));
            //for (int i = 0; i < userTest.questionInTest.Count; i++)
            //{
            //    if (userTest.questionInTest[i].answerId == null)
            //    {
            //        continue;
            //    }
            //    var answerEn = db.Answer.Include(o => o.QuestionInTest).SingleOrDefault(an => an.AnswerId == userTest.questionInTest[i].answerId);
            //    answers.Add(new AnswerDTO(answerEn));
            //}

            var qitss = db.QuestionInTest.Where(o => qitIds.Contains(o.Qitid)).ToList();
            for(int i = 0; i < qitss.Count; i++)
            {
                qitss[i].AnswerId = answerIds[i];
            }
            db.SaveChanges();
           TestAnswerDTO testAnswer = new TestAnswerDTO(answers, userTest.testId);
            Statistic statistic = new Statistic();
            statistic.TestId = userTest.testId;
            statistic.IsActive = true;
            db.Statistic.Add(statistic);
            db.SaveChanges();
            double? totalPoint = CalculateResultPoint(db, testAnswer, statistic.StatisticId);
            List<ConfigurationRankDTO> configurationRanks = GetRankPoint(db, testAnswer);
            configurationRanks = configurationRanks.OrderBy(o => o.point).ToList();
            ConfigurationRankDTO tmp = new ConfigurationRankDTO();
            tmp.rankId = configurationRanks[0].rankId.Value;
            tmp.point = configurationRanks[0].point;
            foreach (ConfigurationRankDTO cr in configurationRanks)
            {
                if (totalPoint > cr.point)
                {
                    tmp = cr;
                }
            }
            string rank = db.Rank.SingleOrDefault(r => r.RankId == tmp.rankId).Name;
            if (rank == null)
            {
                return null;
            }
            statistic.RankId = tmp.rankId;
            statistic.Point = totalPoint;
            db.SaveChanges();
            return new RankPoint(rank, totalPoint);

        }

        public static bool AutoSaveAnswer(DeverateContext db, UserTest userTest)
        {
            Test test = db.Test.SingleOrDefault(c => c.TestId == userTest.testId);
            if (test.Code != userTest.code)
            {
                return false;
            }
            for (int i = 0; i < userTest.questionInTest.Count; i++)
            {
                if (userTest.questionInTest[i].answerId == null)
                {
                    continue;
                }
                SaveAnswer(userTest.questionInTest[i].qitid, userTest.questionInTest[i].answerId);
            }
            return true;
        }

        public static void SaveAnswer(int? QITId, int? answerId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var question = db.QuestionInTest.SingleOrDefault(o => o.Qitid == QITId);
                question.AnswerId = answerId;
                db.SaveChanges();
            }
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
        public static double? CalculateResultPoint(DeverateContext db, TestAnswerDTO answers, int? statisticId)
        {
            double? totalPoint = 0;
            List<CataloguePointDTO> cataloguePoints = CalculateCataloguePoints(db, answers);
            if (answers.testId == null)
            {
                return -1;
            }
            List<CatalogueWeightPointDTO> catalogueWeightPoints = GetWeightPoints(db, answers.testId);
            List<DetailStatistic> details = new List<DetailStatistic>();
            for (int i = 0; i < cataloguePoints.Count; i++)
            {
                DetailStatistic detail = new DetailStatistic();
                detail.StatisticId = statisticId;
                detail.CatalogueId = cataloguePoints[i].catalogueId;
                double? point = cataloguePoints[i].cataloguePoint * catalogueWeightPoints[i].weightPoint;
                detail.Point = cataloguePoints[i].cataloguePoint;
                details.Add(detail);
                totalPoint += point;
            }
            if(details.Count > 0)
            {
                db.DetailStatistic.AddRange(details);
                db.SaveChanges();
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
            var catalogues = db.Catalogue.Where(c => c.CatalogueInConfiguration.Any(cif => cif.Config.Test.Any(o => o.TestId == answers.testId))).ToList();
            List<CataloguePointDTO> cataloguePoints = new List<CataloguePointDTO>();
            List<AnswerDTO> anss = new List<AnswerDTO>(answers.answers);
            List<int?> questIds = new List<int?>();
            anss.ForEach(a => questIds.Add(a.AnswerId));
            var quess = db.Answer.Include(a => a.Question).Where(an => questIds.Contains(an.AnswerId)).ToList();

            foreach (Catalogue cata in catalogues)
            {
                float? point = 0;
                float? maxPoint = 0;
                
                for(int i = 0; i < quess.Count; i++)
                {
                    if(quess[i].Question.CatalogueId == cata.CatalogueId)
                    {
                        maxPoint += quess[i].Question.MaxPoint;
                        point += quess[i].Point;
                        quess.RemoveAt(i);
                        i--;
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

        public static List<QuestionInTestDTO> GetQuestionInTest(DeverateContext db, TestInfoDTO testInfo, bool checkCode)
        {
            Test test = new Test();
            if (checkCode)
            {
                test = db.Test.SingleOrDefault(t => t.TestId == testInfo.testId);
            }
            else
            {
                test = db.Test.SingleOrDefault(t => t.AccountId == testInfo.accountId && t.ConfigId == testInfo.configId && t.Code == testInfo.code);
            }
            if (test == null)
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
                result.Add(new QuestionInTestDTO(item.Qitid, item.Question.Answer.ToList(), item.AnswerId, item.Question.Question1));
            }
            return result;
        }

        public static List<TestInfoDTO> GetTestByConfig(DeverateContext db, int id)
        {
            var results = db.Test.Where(t => t.ConfigId == id).Select(t => new TestInfoDTO(t, t.Config.Title, t.Account.Username)).ToList();
            return results;
        }
    }
}
