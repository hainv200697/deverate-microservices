﻿using AuthenServices.Model;
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
        public static string SendTestMail(int? configId, bool isUpdate)
        {
            using (DeverateContext db = new DeverateContext())
            {
                {
                    var result = from c in db.Configuration
                                 join t in db.Test on c.ConfigId equals t.ConfigId
                                 join a in db.Account on t.AccountId equals a.AccountId
                                 where c.ConfigId == configId && c.IsActive == true
                                 select new TestMailDTO(a.Email, a.Fullname, c.Title, c.StartDate, c.EndDate,
                                 isUpdate == false ? t.Code : null, t.TestId.ToString());
                    if (result.ToList().Count == 0)
                    {
                        return null;
                    }
                    List<TestMailDTO> mails = result.ToList();
                    string message = JsonConvert.SerializeObject(mails);
                    Producer.PublishMessage(message, AppConstrain.test_mail);
                }
                return Message.sendMailSucceed;
            }
        }

        public static void ExpireTest(int testId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var test = context.Test.FirstOrDefault(t => t.TestId == testId);
                test.Status = "Expired";
                context.SaveChanges();
            }
        }

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

        public static int GetApplicantId(int testId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var test = context.Test.FirstOrDefault(t => t.TestId == testId);
                if (test.ApplicantId == null)
                {
                    return 0;
                }
                return test.ApplicantId.Value;
            }
        }

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

        //public static SampleTestDTO GenerateQuestionsForSampleTest(DeverateContext db, SampleConfigDTO con)
        //{

        //    List<QuestionDTO> questions = new List<QuestionDTO>();
        //    List<QuestionRemain> remainQues = new List<QuestionRemain>();
        //    List<QuestionDTO> unchoosedQues = new List<QuestionDTO>();
        //    List<QuestionDTO> choosedQues = new List<QuestionDTO>();
        //    List<int?> quesIds = new List<int?>();
        //    Random rand = new Random();
        //    int totalCataQues = 0;

        //    List<int?> cIds = new List<int?>();
        //    con.catalogueInSamples.ForEach(c => cIds.Add(c.cicId));
        //    List<CatalogueInCompany> catalogueIns = db.CatalogueInCompany.Include(c => c.Catalogue).Include(c => c.Question).ThenInclude(c => c.Answer).Where(c => cIds.Contains(c.Catalogue.CatalogueId) && c.CompanyId == con.companyId).ToList();
        //    List<Question> cloneQuesList = new List<Question>();
        //    foreach (CatalogueInCompany cic in catalogueIns)
        //    {
        //        for (int i = 0; i < cic.Question.ToList().Count; i++)
        //        {
        //            if (cic.Question.ToList()[i].IsActive == true)
        //            {
        //                cloneQuesList.Add(cic.Question.Where(q => q.IsActive == true).ToList()[i]);
        //            }
        //        }
        //        cic.Question = cloneQuesList;
        //        cloneQuesList = new List<Question>();
        //    }
        //    if (catalogueIns.Count == 0)
        //    {
        //        return null;
        //    }
        //    List<CatalogueDTO> catalogues = new List<CatalogueDTO>();
        //    for (int i = 0; i < catalogueIns.Count; i++)
        //    {
        //        for (int j = 0; j < con.catalogueInSamples.Count; j++)
        //        {
        //            if (con.catalogueInSamples[j].cicId == catalogueIns[i].CatalogueId)
        //            {
        //                catalogues.Add(new CatalogueDTO(catalogueIns[i].CatalogueId, catalogueIns[i].Catalogue.Name, 0, con.catalogueInSamples[j].weightPoint, catalogueIns[i].Question.ToList()));
        //                totalCataQues += catalogueIns[i].Question.ToList().Count;
        //            }
        //        }
        //    }
        //    if (catalogues == null || catalogues.Count == 0)
        //    {
        //        return null;
        //    }

        //    if (totalCataQues == 0)
        //    {
        //        return null;

        //    }
        //    catalogues = catalogues.OrderBy(o => o.weightPoint).ToList();

        //    int totalOfQues = con.totalQuestion.Value > totalCataQues ? totalCataQues : con.totalQuestion.Value;
        //    catalogues = GetNumberOfQuestionEachCatalogue(db, totalOfQues, catalogues);


        //    for (int i = 0; i < catalogues.Count; i++)
        //    {
        //        QuestionRemain quesRe = new QuestionRemain();
        //        quesRe.catalogueId = catalogues[i].catalogueId;

        //        List<Question> totalQues = catalogues[i].questionList;
        //        if (totalQues == null || totalQues.Count == 0)
        //        {
        //            continue;
        //        }
        //        int quesLenght = totalQues.Count;
        //        int numbOfQues = catalogues[i].numberOfQuestion > quesLenght ? quesLenght : catalogues[i].numberOfQuestion.Value;
        //        totalQues = ShuffleQuestion(totalQues);
        //        for (int j = 0; j < numbOfQues; j++)
        //        {
        //            if (!quesIds.Contains(totalQues[j].QuestionId))
        //            {
        //                quesIds.Add(totalQues[j].QuestionId);
        //                List<AnswerDTO> answers = totalQues[j].Answer.Select(a => new AnswerDTO(a)).ToList();
        //                questions.Add(new QuestionDTO(totalQues[j].QuestionId, totalQues[j].Question1, totalQues[j].Cic.CatalogueId, answers));
        //                if (numbOfQues != quesLenght)
        //                {
        //                    choosedQues.Add(new QuestionDTO(totalQues[j].QuestionId, answers));
        //                }
        //            }
        //            else
        //            {
        //                j--;
        //            }
        //        }
        //        quesRe.curNumbQues = numbOfQues;
        //        quesRe.numbCataQues = catalogues[i].questionList.Count;

        //        if (numbOfQues != quesLenght)
        //        {
        //            for (int k = 0; k < quesLenght; k++)
        //            {
        //                List<AnswerDTO> answers = totalQues[k].Answer.Select(a => new AnswerDTO(a)).ToList();
        //                bool isContainQues = false;
        //                for (int m = 0; m < choosedQues.Count; m++)
        //                {
        //                    if (choosedQues[m].questionId == totalQues[k].QuestionId)
        //                    {
        //                        isContainQues = true;
        //                        break;
        //                    }
        //                }
        //                if (isContainQues == false)
        //                {
        //                    unchoosedQues.Add(new QuestionDTO(totalQues[k].QuestionId, totalQues[k].Question1, totalQues[k].Cic.CatalogueId, answers));
        //                }
        //            }
        //        }
        //        quesRe.unchoosedQues = unchoosedQues;
        //        quesRe.weightPoint = catalogues[i].weightPoint;
        //        remainQues.Add(quesRe);
        //    }
        //    questions = fillQues(remainQues, totalOfQues, questions);
        //    questions = Shuffle(questions);
        //    for (int i = 0; i < catalogues.Count; i++)
        //    {
        //        catalogues[i].questionList = null;

        //    }
        //    return new SampleTestDTO(catalogues, questions);
        //}

        public static string GenerateTestForApplicants(string configId, List<ApplicantDTO> applicants, bool oneForAll = false)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Configuration con = db.Configuration
                    .Include(c => c.CatalogueInConfiguration)
                    .ThenInclude(t => t.CompanyCatalogue)
                    .ThenInclude(t => t.Question)
                    .SingleOrDefault(o => o.ConfigId == Int32.Parse(configId));
                if (con.Duration < AppConstrain.minDuration)
                {
                    return Message.durationExceptopn;
                }
                List<CompanyCatalogueDTO> catas = GetCatalogueWeights(con.ConfigId);
                if (catas.Count == 0 || catas == null)
                {
                    return Message.noCatalogueException;
                }
                if (applicants.Count == 0)
                {
                    return Message.noApplicantException;
                }
                CreateTestForApplicant(applicants, con, oneForAll);
                List<int> applicantIds = new List<int>();
                foreach (ApplicantDTO applicant in applicants)
                {
                    applicantIds.Add(applicant.applicantId);
                }
                SendMailQuizCode(applicantIds, false);
                return null;
            }
        }

        public static int isAvailableTest(int? accountId, List<Test> tests)
        {
            for (int i = 0; i < tests.Count; i++)
            {
                if (tests[i].AccountId == accountId) return i;
            }
            return -1;
        }

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

        //public static void GenerateQuestionsForApplicants(DeverateContext db, List<ApplicantDTO> applicants, Configuration config)
        //{
        //    List<QuestionDTO> questions = new List<QuestionDTO>();
        //    List<QuestionRemain> remainQues = new List<QuestionRemain>();
        //    List<QuestionDTO> unchoosedQues = new List<QuestionDTO>();
        //    List<QuestionDTO> choosedQues = new List<QuestionDTO>();
        //    List<int?> quesIds = new List<int?>();
        //    Random rand = new Random();
        //    int totalCataQues = 0;


        //    List<CatalogueDTO> catalogues = GetCatalogueWeights(db, config.ConfigId);
        //    if (catalogues == null || catalogues.Count == 0)
        //    {
        //        return;
        //    }
        //    catalogues = catalogues.OrderBy(o => o.weightPoint).ToList();
        //    for (int i = 0; i < catalogues.Count; i++)
        //    {
        //        catalogues[i].questions = GetQuestionOfCatalogue(db, catalogues[i].catalogueId, config.Account.CompanyId);
        //        totalCataQues += catalogues[i].questions == null ? 0 : catalogues[i].questions.Count;
        //    }
        //    if (totalCataQues == 0)
        //    {
        //        return;

        //    }
        //    int totalOfQues = config.TotalQuestion > totalCataQues ? totalCataQues : config.TotalQuestion;
        //    catalogues = GetNumberOfQuestionEachCatalogue(db, totalOfQues, catalogues);

        //    foreach (ApplicantDTO app in applicants)
        //    {
        //        for (int i = 0; i < catalogues.Count; i++)
        //        {
        //            QuestionRemain quesRe = new QuestionRemain();
        //            quesRe.catalogueId = catalogues[i].catalogueId;

        //            List<QuestionDTO> totalQues = catalogues[i].questions;
        //            if (totalQues == null || totalQues.Count == 0)
        //            {
        //                continue;
        //            }
        //            int quesLenght = totalQues.Count;
        //            int numbOfQues = catalogues[i].numberOfQuestion > quesLenght ? quesLenght : catalogues[i].numberOfQuestion.Value;
        //            totalQues = Shuffle(totalQues);
        //            for (int j = 0; j < numbOfQues; j++)
        //            {
        //                if (!quesIds.Contains(totalQues[j].questionId))
        //                {
        //                    quesIds.Add(totalQues[j].questionId);
        //                    questions.Add(totalQues[j]);
        //                    if (numbOfQues != quesLenght)
        //                    {
        //                        choosedQues.Add(totalQues[j]);
        //                    }
        //                }
        //                else
        //                {
        //                    j--;
        //                }
        //            }
        //            quesRe.curNumbQues = numbOfQues;
        //            quesRe.numbCataQues = catalogues[i].questions.Count;

        //            if (numbOfQues != quesLenght)
        //            {
        //                for (int k = 0; k < quesLenght; k++)
        //                {
        //                    bool isContainQues = false;
        //                    for (int m = 0; m < choosedQues.Count; m++)
        //                    {
        //                        if (choosedQues[m].questionId == totalQues[k].questionId)
        //                        {
        //                            isContainQues = true;
        //                            break;
        //                        }
        //                    }
        //                    if (isContainQues == false)
        //                    {
        //                        unchoosedQues.Add(totalQues[k]);
        //                    }
        //                }
        //            }
        //            quesRe.unchoosedQues = unchoosedQues;
        //            quesRe.weightPoint = catalogues[i].weightPoint;
        //            remainQues.Add(quesRe);
        //        }
        //        questions = fillQues(remainQues, totalOfQues, questions);
        //        questions = Shuffle(questions);

        //        Test test = new Test();
        //        test.ConfigId = config.ConfigId;
        //        test.ApplicantId = app.applicantId;
        //        test.CreateDate = DateTime.UtcNow;
        //        test.IsActive = true;
        //        test.Status = "Pending";
        //        test.Code = GenerateCode();
        //        db.Test.Add(test);
        //        db.SaveChanges();
        //        List<QuestionInTest> questionInTests = new List<QuestionInTest>();
        //        for (int i = 0; i < questions.Count; i++)
        //        {
        //            QuestionInTest inTest = new QuestionInTest();
        //            inTest.TestId = test.TestId;
        //            inTest.QuestionId = questions[i].questionId;
        //            inTest.IsActive = true;
        //            questionInTests.Add(inTest);

        //        }
        //        db.QuestionInTest.AddRange(questionInTests);
        //        db.SaveChanges();
        //        questions = new List<QuestionDTO>();
        //        remainQues = new List<QuestionRemain>();
        //        unchoosedQues = new List<QuestionDTO>();
        //        choosedQues = new List<QuestionDTO>();
        //        quesIds = new List<int?>();
        //    }
        //}

        public static string GenerateTest(List<int> accountIds, string configId, bool oneForAll = false)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Configuration con = db.Configuration
                    .Include(c => c.CatalogueInConfiguration)
                    .ThenInclude(t => t.CompanyCatalogue)
                    .ThenInclude(t => t.Question)
                    .SingleOrDefault(o => o.ConfigId == Int32.Parse(configId));
                if (con.Duration < AppConstrain.minDuration)
                {
                    return Message.durationExceptopn;
                }
                List<CompanyCatalogueDTO> catas = GetCatalogueWeights(con.ConfigId);
                if (catas.Count == 0)
                {
                    return Message.noCatalogueException;
                }
                List<AccountDTO> accounts = db.Account.Where(a => accountIds.Contains(a.AccountId) && a.IsActive == true).Select(a => new AccountDTO(a)).ToList();
                if (accounts.Count == 0)
                {
                    return Message.noEmployeeException;
                }
                CreateTestForEmployee(accounts, con, oneForAll);
                return null;
            }
        }

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

                List<CompanyCatalogueDTO> companyCatalogues = db.CompanyCatalogue.Include(c => c.Question)
                                                            .ThenInclude(c => c.Answer)
                                                            .Where(c => catalogueIds.Contains(c.CompanyCatalogueId))
                                                            .Select(c => new CompanyCatalogueDTO(c.CompanyCatalogueId, c.Name, 0, 0, c.Question.ToList())).ToList();
                for (int i = 0; i < companyCatalogues.Count; i++)
                {
                    foreach (CatalogueInSampleTestDTO sc in sampleConfig.catalogueInSamples)
                    {
                        if(sc.companyCatalogueId == companyCatalogues[i].companyCatalogueId)
                        {
                            companyCatalogues[i].numberOfQuestion = sc.numberQuestion > companyCatalogues[i].questionList.Count ? companyCatalogues[i].questionList.Count : sc.numberQuestion;
                            companyCatalogues[i].questionList = ShuffleQuestion(companyCatalogues[i].questionList.ToList());
                            List<Question> tempQuestions = companyCatalogues[i].questionList.Take(companyCatalogues[i].numberOfQuestion.Value).ToList();
                            foreach (Question q in tempQuestions)
                            {
                                questions.Add(new QuestionDTO(q.QuestionId, q.Question1, q.Answer.ToList()));
                            }
                            companyCatalogues[i].questionList = null;
                        }
                    }

                }
                questions = ShuffleQuestion(questions);
                SampleTestDTO sampleTest = new SampleTestDTO();
                sampleTest.questions = questions;
                sampleTest.catalogues = companyCatalogues;
                return sampleTest;
            }
        }

        public static void CreateTestForApplicant(List<ApplicantDTO> applicants, Configuration config, bool oneForAll = false)
        {
            using (DeverateContext db = new DeverateContext())
            {
                List<QuestionDTO> questions = new List<QuestionDTO>();
                List<CatalogueInConfiguration> catalogues = config.CatalogueInConfiguration.ToList();
                for (int i = 0; i < catalogues.Count; i++)
                {
                    catalogues[i].NumberQuestion = catalogues[i].NumberQuestion > catalogues[i].CompanyCatalogue.Question.Count ? catalogues[i].CompanyCatalogue.Question.Count : catalogues[i].NumberQuestion;

                }
                List<Test> tests = new List<Test>();
                if (oneForAll == true)
                {
                    for (int i = 0; i < catalogues.Count; i++)
                    {
                        catalogues[i].CompanyCatalogue.Question = ShuffleQuestion(catalogues[i].CompanyCatalogue.Question.ToList());
                        List<Question> tempQuestions = catalogues[i].CompanyCatalogue.Question.Take(catalogues[i].NumberQuestion).ToList();
                        foreach (Question q in tempQuestions)
                        {
                            questions.Add(new QuestionDTO(q.QuestionId, q.Question1, q.Answer.ToList()));
                        }

                    }
                    foreach (ApplicantDTO app in applicants)
                    {
                        questions = ShuffleQuestion(questions);
                        List<QuestionInTest> inTests = new List<QuestionInTest>();
                        foreach (QuestionDTO q in questions)
                        {
                            QuestionInTest qit = new QuestionInTest();
                            qit.QuestionId = q.questionId;
                            qit.IsActive = true;
                            inTests.Add(qit);
                        }

                        Test t = new Test();
                        t.ApplicantId = app.applicantId;
                        t.QuestionInTest = inTests;
                        t.ConfigId = config.ConfigId;
                        t.CreateDate = DateTime.UtcNow;
                        t.Code = GenerateCode();
                        t.Status = "Pending";
                        tests.Add(t);
                    }
                }
                else
                {
                    
                    foreach (ApplicantDTO app in applicants)
                    {
                        for (int i = 0; i < catalogues.Count; i++)
                        {
                            catalogues[i].CompanyCatalogue.Question = ShuffleQuestion(catalogues[i].CompanyCatalogue.Question.ToList());
                            List<Question> tempQuestions = catalogues[i].CompanyCatalogue.Question.Take(catalogues[i].NumberQuestion).ToList();
                            foreach (Question q in tempQuestions)
                            {
                                questions.Add(new QuestionDTO(q.QuestionId, q.Question1, q.Answer.ToList()));
                            }

                        }
                        questions = ShuffleQuestion(questions);
                        List<QuestionInTest> inTests = new List<QuestionInTest>();
                        foreach (QuestionDTO q in questions)
                        {
                            QuestionInTest qit = new QuestionInTest();
                            qit.QuestionId = q.questionId;
                            qit.IsActive = true;
                            inTests.Add(qit);
                        }

                        Test t = new Test();
                        t.ApplicantId = app.applicantId;
                        t.QuestionInTest = inTests;
                        t.ConfigId = config.ConfigId;
                        t.CreateDate = DateTime.UtcNow;
                        t.Code = GenerateCode();
                        t.Status = "Pending";
                        tests.Add(t);
                        questions = new List<QuestionDTO>();
                    }
                }
                db.Test.AddRange(tests);
                db.SaveChanges();
            }
        }

        public static void CreateTestForEmployee(List<AccountDTO> accounts, Configuration config, bool oneForAll = false)
        {
            using (DeverateContext db = new DeverateContext())
            {
                List<QuestionDTO> questions = new List<QuestionDTO>();
                List<CatalogueInConfiguration> catalogues = config.CatalogueInConfiguration.ToList();
                for (int i = 0; i < catalogues.Count; i++)
                {
                    catalogues[i].NumberQuestion = catalogues[i].NumberQuestion > catalogues[i].CompanyCatalogue.Question.Count ? catalogues[i].CompanyCatalogue.Question.Count : catalogues[i].NumberQuestion;
                }
                List<Test> generatedTest = db.Test.Where(c => c.ConfigId == config.ConfigId).ToList();
                List<Test> tests = new List<Test>();
                if (oneForAll == true)
                {
                    for (int i = 0; i < catalogues.Count; i++)
                    {
                        catalogues[i].CompanyCatalogue.Question = ShuffleQuestion(catalogues[i].CompanyCatalogue.Question.ToList());
                        List<Question> tempQuestions = catalogues[i].CompanyCatalogue.Question.Take(catalogues[i].NumberQuestion).ToList();
                        foreach (Question q in tempQuestions)
                        {
                            questions.Add(new QuestionDTO(q.QuestionId, q.Question1, q.Answer.ToList()));
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
                            QuestionInTest qit = new QuestionInTest();
                            qit.QuestionId = q.questionId;
                            qit.IsActive = true;
                            inTests.Add(qit);
                        }

                        Test t = new Test();
                        t.AccountId = acc.accountId;
                        t.QuestionInTest = inTests;
                        t.ConfigId = config.ConfigId;
                        t.CreateDate = DateTime.UtcNow;
                        t.Code = GenerateCode();
                        t.IsActive = true;
                        t.Status = "Pending";
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
                            catalogues[i].CompanyCatalogue.Question = ShuffleQuestion(catalogues[i].CompanyCatalogue.Question.ToList());
                            List<Question> tempQuestions = catalogues[i].CompanyCatalogue.Question.Take(catalogues[i].NumberQuestion).ToList();
                            foreach (Question q in tempQuestions)
                            {
                                questions.Add(new QuestionDTO(q.QuestionId, q.Question1, q.Answer.ToList()));
                            }

                        }
                        questions = ShuffleQuestion(questions);
                        List<QuestionInTest> inTests = new List<QuestionInTest>();
                        foreach (QuestionDTO q in questions)
                        {
                            QuestionInTest qit = new QuestionInTest();
                            qit.QuestionId = q.questionId;
                            qit.IsActive = true;
                            inTests.Add(qit);
                        }

                        Test t = new Test();
                        t.AccountId = acc.accountId;
                        t.QuestionInTest = inTests;
                        t.ConfigId = config.ConfigId;
                        t.CreateDate = DateTime.UtcNow;
                        t.Code = GenerateCode();
                        t.Status = "Pending";
                        t.IsActive = true;
                        tests.Add(t);
                        questions = new List<QuestionDTO>();
                    }
                }
                db.Test.AddRange(tests);
                db.SaveChanges();
            }
        }




        //public static void GenerateQuestions(DeverateContext db, List<AccountDTO> accounts, Configuration config, int? companyId)
        //{
        //    List<QuestionDTO> questions = new List<QuestionDTO>();
        //    List<QuestionRemain> remainQues = new List<QuestionRemain>();
        //    List<QuestionDTO> unchoosedQues = new List<QuestionDTO>();
        //    List<QuestionDTO> choosedQues = new List<QuestionDTO>();
        //    List<int?> quesIds = new List<int?>();
        //    Random rand = new Random();
        //    int totalCataQues = 0;

        //    List<int?> accountIds = new List<int?>();
        //    accounts.ForEach(a => accountIds.Add(a.accountId));
        //    List<Test> tests = db.Test.Where(t => t.ConfigId == config.ConfigId).ToList();
        //    tests = removeAvailableTests(accountIds, tests);
        //    db.SaveChanges();
        //    List<CatalogueDTO> catalogues = GetCatalogueWeights(db, config.ConfigId);
        //    catalogues = catalogues.OrderBy(o => o.weightPoint).ToList();
        //    for (int i = 0; i < catalogues.Count; i++)
        //    {
        //        catalogues[i].questions = GetQuestionOfCatalogue(db, catalogues[i].catalogueId, companyId);
        //        totalCataQues += catalogues[i].questions.Count;
        //    }
        //    if (totalCataQues == 0)
        //    {
        //        return;
        //    }
        //    int totalOfQues = config.TotalQuestion > totalCataQues ? totalCataQues : config.TotalQuestion;
        //    catalogues = GetNumberOfQuestionEachCatalogue(db, totalOfQues, catalogues);


        //    foreach (AccountDTO acc in accounts)
        //    {
        //        for (int i = 0; i < catalogues.Count; i++)
        //        {
        //            QuestionRemain quesRe = new QuestionRemain();
        //            quesRe.catalogueId = catalogues[i].catalogueId;

        //            List<QuestionDTO> totalQues = catalogues[i].questions;

        //            if (totalQues == null || totalQues.Count == 0)
        //            {
        //                continue;
        //            }
        //            int quesLenght = totalQues.Count;
        //            int numbOfQues = catalogues[i].numberOfQuestion > quesLenght ? quesLenght : catalogues[i].numberOfQuestion.Value;
        //            totalQues = Shuffle(totalQues);
        //            for (int j = 0; j < numbOfQues; j++)
        //            {
        //                if (!quesIds.Contains(totalQues[j].questionId))
        //                {
        //                    quesIds.Add(totalQues[j].questionId);
        //                    questions.Add(totalQues[j]);
        //                    if (numbOfQues != quesLenght)
        //                    {
        //                        choosedQues.Add(totalQues[j]);
        //                    }
        //                }
        //                else
        //                {
        //                    j--;
        //                }
        //            }
        //            quesRe.curNumbQues = numbOfQues;
        //            quesRe.numbCataQues = catalogues[i].questions.Count;

        //            if (numbOfQues != quesLenght)
        //            {
        //                for (int k = 0; k < quesLenght; k++)
        //                {
        //                    bool isContainQues = false;
        //                    for (int m = 0; m < choosedQues.Count; m++)
        //                    {
        //                        if (choosedQues[m].questionId == totalQues[k].questionId)
        //                        {
        //                            isContainQues = true;
        //                            break;
        //                        }
        //                    }
        //                    if (isContainQues == false)
        //                    {
        //                        unchoosedQues.Add(totalQues[k]);
        //                    }
        //                }
        //            }
        //            quesRe.unchoosedQues = unchoosedQues;
        //            quesRe.weightPoint = catalogues[i].weightPoint;
        //            remainQues.Add(quesRe);
        //        }
        //        questions = fillQues(remainQues, totalOfQues, questions);
        //        questions = Shuffle(questions);

        //        Test test = new Test();
        //        test.ConfigId = config.ConfigId;
        //        test.AccountId = acc.accountId;
        //        test.CreateDate = DateTime.UtcNow;
        //        test.IsActive = true;
        //        test.Status = "Pending";
        //        test.Code = GenerateCode();
        //        db.Test.Add(test);
        //        db.SaveChanges();
        //        List<QuestionInTest> questionInTests = new List<QuestionInTest>();
        //        for (int i = 0; i < questions.Count; i++)
        //        {
        //            QuestionInTest inTest = new QuestionInTest();
        //            inTest.TestId = test.TestId;
        //            inTest.QuestionId = questions[i].questionId;
        //            inTest.IsActive = true;
        //            questionInTests.Add(inTest);

        //        }
        //        db.QuestionInTest.AddRange(questionInTests);
        //        db.SaveChanges();
        //        questions = new List<QuestionDTO>();
        //        remainQues = new List<QuestionRemain>();
        //        unchoosedQues = new List<QuestionDTO>();
        //        choosedQues = new List<QuestionDTO>();
        //        quesIds = new List<int?>();
        //    }
        //}

        //public static List<QuestionDTO> GenerateQuestion(DeverateContext db, int? accountId, Configuration config, int? companyId)
        //{
        //    List<QuestionDTO> questions = new List<QuestionDTO>();
        //    Random rand = new Random();
        //    int totalCataQues = 0;
        //    List<CatalogueDTO> catalogues = GetCatalogueWeights(db, config.ConfigId);
        //    catalogues = catalogues.OrderByDescending(o => o.weightPoint).ToList();
        //    for (int i = 0; i < catalogues.Count; i++)
        //    {
        //        catalogues[i].questions = GetQuestionOfCatalogue(db, catalogues[i].catalogueId, companyId);
        //        totalCataQues += catalogues[i].questions.Count;
        //    }
        //    int totalOfQues = config.TotalQuestion > totalCataQues ? totalCataQues : config.TotalQuestion;
        //    catalogues = GetNumberOfQuestionEachCatalogue(db, totalOfQues, catalogues);
        //    List<QuestionRemain> remainQues = new List<QuestionRemain>();
        //    for (int i = 0; i < catalogues.Count; i++)
        //    {
        //        QuestionRemain quesRe = new QuestionRemain();
        //        quesRe.catalogueId = catalogues[i].catalogueId;
        //        List<int?> quesIds = new List<int?>();
        //        List<QuestionDTO> totalQues = catalogues[i].questions;
        //        List<QuestionDTO> choosedQues = new List<QuestionDTO>();
        //        int quesLenght = totalQues.Count;
        //        int numbOfQues = catalogues[i].numberOfQuestion > quesLenght ? quesLenght : catalogues[i].numberOfQuestion.Value;
        //        totalQues = Shuffle(totalQues);
        //        for (int j = 0; j < numbOfQues; j++)
        //        {
        //            if (!quesIds.Contains(totalQues[j].questionId))
        //            {
        //                quesIds.Add(totalQues[j].questionId);
        //                questions.Add(totalQues[j]);
        //                if (numbOfQues != quesLenght)
        //                {
        //                    choosedQues.Add(totalQues[j]);
        //                }
        //            }
        //            else
        //            {
        //                j--;
        //            }
        //        }
        //        quesRe.curNumbQues = numbOfQues;
        //        quesRe.numbCataQues = catalogues[i].questions.Count;
        //        List<QuestionDTO> unchoosedQues = new List<QuestionDTO>();
        //        if (numbOfQues != quesLenght)
        //        {
        //            for (int k = 0; k < quesLenght; k++)
        //            {
        //                bool isContainQues = false;
        //                for (int m = 0; m < choosedQues.Count; m++)
        //                {
        //                    if (choosedQues[m].questionId == totalQues[k].questionId)
        //                    {
        //                        isContainQues = true;
        //                        break;
        //                    }
        //                }
        //                if (isContainQues == false)
        //                {
        //                    unchoosedQues.Add(totalQues[k]);
        //                }
        //            }
        //        }
        //        quesRe.unchoosedQues = unchoosedQues;
        //        quesRe.weightPoint = catalogues[i].weightPoint;
        //        remainQues.Add(quesRe);
        //    }
        //    questions = fillQues(remainQues, totalOfQues, questions);
        //    questions = Shuffle(questions);

        //    Test test = new Test();
        //    test.ConfigId = config.ConfigId;
        //    test.AccountId = accountId;
        //    test.CreateDate = DateTime.UtcNow;
        //    test.IsActive = true;
        //    test.Status = "Pending";
        //    db.Test.Add(test);
        //    db.SaveChanges();

        //    test.Code = GenerateCode();
        //    db.SaveChanges();

        //    for (int i = 0; i < questions.Count; i++)
        //    {
        //        QuestionInTest inTest = new QuestionInTest();
        //        inTest.TestId = test.TestId;
        //        inTest.QuestionId = questions[i].questionId;
        //        inTest.IsActive = true;
        //        db.QuestionInTest.Add(inTest);
        //        db.SaveChanges();
        //    }


        //    return questions;
        //}


        //public static List<QuestionDTO> fillQues(List<QuestionRemain> remains, int totalOfQues, List<QuestionDTO> questions)
        //{
        //    Random rand = new Random();
        //    int remainNumbQues = totalOfQues - questions.Count;
        //    if (totalOfQues <= questions.Count)
        //    {
        //        return questions;
        //    }
        //    for (int i = 0; i < remains.Count; i++)
        //    {
        //        int unfilledQues = remains[i].numbCataQues.Value - remains[i].curNumbQues.Value;
        //        if (unfilledQues == 0)
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            int difQues = remainNumbQues > unfilledQues ? unfilledQues : remainNumbQues;
        //            difQues = Convert.ToInt32(difQues * remains[i].weightPoint);
        //            if (difQues == 0)
        //            {
        //                difQues = 1;
        //            }
        //            difQues = remainNumbQues > difQues ? difQues : remainNumbQues;
        //            for (int j = 0; j < difQues; j++)
        //            {
        //                int rQues = rand.Next(0, remains[i].unchoosedQues.Count);
        //                if (!questions.Contains(remains[i].unchoosedQues[rQues]))
        //                {
        //                    questions.Add(remains[i].unchoosedQues[rQues]);
        //                    remains[i].unchoosedQues.RemoveAt(rQues);
        //                    remains[i].curNumbQues += 1;
        //                }
        //                else
        //                {
        //                    j--;
        //                }
        //            }
        //            remainNumbQues = remainNumbQues - difQues;
        //        }
        //    }

        //    return fillQues(remains, totalOfQues, questions);
        //}

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

        public static List<QuestionDTO> GetQuestionOfCatalogue(DeverateContext db, int? companyCatalogueId, int? companyId)
        {

            var ques = from ca in db.CompanyCatalogue
                       join q in db.Question on ca.CompanyCatalogueId equals q.CompanyCatalogueId
                       where ca.CompanyCatalogueId == companyCatalogueId && ca.CompanyId == companyId && q.IsActive == true
                       select new QuestionDTO(q.QuestionId, q.Question1, null);
            List<QuestionDTO> questions = ques.ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                questions[i].answers = GetAnswerOfQuestion(db, questions[i].questionId);
            }
            return questions;
        }

        public static List<AnswerDTO> GetAnswerOfQuestion(DeverateContext db, int? questionId)
        {

            var answers = from q in db.Question
                          join a in db.Answer on q.QuestionId equals a.QuestionId
                          where q.QuestionId == questionId && q.IsActive == true
                          select new AnswerDTO(a);
            return answers.ToList();

        }

        public static List<CompanyCatalogueDTO> GetNumberOfQuestionEachCatalogue(DeverateContext db, int? totalQuestion, List<CompanyCatalogueDTO> catalogues)
        {
            if (totalQuestion == 0)
            {
                return catalogues;
            }
            int currentQuestion = 0;
            for (int i = 0; i < catalogues.Count; i++)
            {
                double numberOfQuestion = catalogues[i].weightPoint == null || catalogues[i].weightPoint.Value <= 0 ? 0 : catalogues[i].weightPoint.Value * totalQuestion.Value / AppConstrain.scaleUpNumb;
                catalogues[i].numberOfQuestion = Convert.ToInt32(numberOfQuestion);
                if (catalogues[i].numberOfQuestion == 0)
                {
                    catalogues[i].numberOfQuestion = 1;
                }
                currentQuestion += catalogues[i].numberOfQuestion.Value;
            }
            int dif = currentQuestion - totalQuestion.Value;
            catalogues[catalogues.Count - 1].numberOfQuestion = catalogues[catalogues.Count - 1].numberOfQuestion.Value - dif;
            return catalogues;
        }

        public static List<CompanyCatalogueDTO> GetCatalogueWeights( int? configId)
        {
            using(DeverateContext db = new DeverateContext())
            {
                var result = from cf in db.Configuration
                             join cif in db.CatalogueInConfiguration on cf.ConfigId equals cif.ConfigId
                             where cf.ConfigId == configId
                             select new CompanyCatalogueDTO(cif.CompanyCatalogueId, cif.CompanyCatalogue.Name, 0, cif.WeightPoint, null, cif.CompanyCatalogue.IsActive);
                if (result.ToList().Count == 0)
                {
                    return null;
                }
                return result.ToList();
            }

        }



        public static RankPoint EvaluateRank(UserTest userTest)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Test test = db.Test.Include(t => t.Account).Include(t => t.Config.Account).SingleOrDefault(t => t.TestId == userTest.testId && t.Code == userTest.code);
                if (test == null)
                {
                    return null;
                }

                test.Status = "Submitted";
                test.FinishTime = DateTime.UtcNow;
                db.SaveChanges();
                List<AnswerDTO> answers = new List<AnswerDTO>();
                List<int?> answerIds = new List<int?>();
                for (int i = 0; i < userTest.questionInTest.Count; i++)
                {
                    answerIds.Add(userTest.questionInTest[i].answerId);
                }
                var anss = db.Answer.Where(a => answerIds.Contains(a.AnswerId)).ToList();
                string rank = "Dev0";
                double totalPoint = 0;

                if (anss.Count != 0)
                {
                    anss.ForEach(a => answers.Add(new AnswerDTO(a)));
                    TestAnswerDTO testAnswer = new TestAnswerDTO(answers, userTest.testId);
                    totalPoint = CalculateResultPoint(testAnswer, test, test.Config.Account.CompanyId, userTest.testId);


                    totalPoint = AppConstrain.RoundDownNumber(totalPoint, 1);
                    List<ConfigurationRankDTO> configurationRanks = GetRankPoint(testAnswer);
                    configurationRanks = configurationRanks.OrderBy(o => o.point).ToList();
                    ConfigurationRankDTO tmp = new ConfigurationRankDTO();
                    tmp.companyRankId = configurationRanks[0].companyRankId;
                    tmp.point = configurationRanks[0].point;
                    foreach (ConfigurationRankDTO cr in configurationRanks)
                    {
                        if (totalPoint > cr.point)
                        {
                            tmp = cr;
                        }
                    }
                    rank = db.CompanyRank.SingleOrDefault(r => r.CompanyRankId == tmp.companyRankId).Name;
                    if (rank == null)
                    {
                        return null;
                    }
                    test.CompanyRankId = tmp.companyRankId;
                    test.Point = totalPoint;
                }
                else
                {
                    test.CompanyRankId = 4;
                    test.Point = 0;
                }
                db.SaveChanges();

                return new RankPoint(rank, totalPoint);
            }
        }

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
                            qitss[i].AnswerId = userTest.questionInTest[j].answerId.Value;
                    }

                }
                db.SaveChanges();
            }
        }

        public static List<ConfigurationRankDTO> GetRankPoint(TestAnswerDTO answer)
        {
            using(DeverateContext db = new DeverateContext())
            {
                if (answer.testId == null)
                {
                    return null;
                }
                Test test = db.Test.Include(t => t.Config).ThenInclude(t => t.CatalogueInConfiguration).Where(t => t.TestId == answer.testId).FirstOrDefault();
                if(test == null)
                {
                    return null;
                }
                List<ConfigurationRankDTO> rankDTOs = new List<ConfigurationRankDTO>();
                int numbOfCatalogue = test.Config.CatalogueInConfiguration.Count;
                List<CatalogueInRank> catalogueInRanks = db.CatalogueInRank
                    .Where(cir => cir.CatalogueInConfig.ConfigId == test.ConfigId).ToList();
                foreach(CatalogueInRank cir in catalogueInRanks)
                {
                    if(rankDTOs.Count > 0)
                    {
                        bool isContain = false;
                        for(int i = 0; i < rankDTOs.Count; i++)
                        {
                            if(rankDTOs[i].companyRankId == cir.CompanyRankId)
                            {
                                isContain = true;
                                rankDTOs[i].point += (cir.Point / numbOfCatalogue).Value;
                                break;
                            }
                        }
                        if (isContain == false)
                        {
                            rankDTOs.Add(new ConfigurationRankDTO(cir.CompanyRankId, (cir.Point / numbOfCatalogue).Value));
                        }
                    }
                    else
                    {
                        rankDTOs.Add(new ConfigurationRankDTO(cir.CompanyRankId, (cir.Point / numbOfCatalogue).Value));
                    }
                }
                return rankDTOs;
            }

        }

        public static double CalculateResultPoint(TestAnswerDTO answers, Test test, int? companyId, int? testId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                double totalPoint = 0;
                List<CataloguePointDTO> defaultCataloguePoints = CalculateCataloguePoints(db, answers, companyId, testId);
                if (answers.testId == null)
                {
                    return -1;
                }
                List<CataloguePointDTO> cataloguePoints = new List<CataloguePointDTO>();
                List<CatalogueWeightPointDTO> catalogueWeightPoints = GetWeightPoints(answers.testId);
                for (int i = 0; i < defaultCataloguePoints.Count; i++)
                {
                    for (int j = 0; j < catalogueWeightPoints.Count; j++)
                    {
                        if (defaultCataloguePoints[i].catalogueId == catalogueWeightPoints[j].catalogueId)
                        {
                            if (!cataloguePoints.Contains(defaultCataloguePoints[i]))
                            {
                                cataloguePoints.Add(defaultCataloguePoints[i]);
                            }

                            break;
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
                    double point = cataloguePoints[i].cataloguePoint * catalogueWeightPoints[i].weightPoint;
                    detail.Point = cataloguePoints[i].cataloguePoint;
                    detail.IsActive = true;
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

        public static List<CatalogueWeightPointDTO> GetWeightPoints(int? testId)
        {
            using(DeverateContext db = new DeverateContext())
            {
                var result = from t in db.Test
                             join cf in db.Configuration on t.ConfigId equals cf.ConfigId
                             join cif in db.CatalogueInConfiguration on cf.ConfigId equals cif.ConfigId
                             where t.TestId == testId
                             select new CatalogueWeightPointDTO(cif.CompanyCatalogueId, cif.WeightPoint);
                if (result == null)
                {
                    return null;
                }
                return result.ToList();
            }

        }

        public static List<CataloguePointDTO> CalculateCataloguePoints(DeverateContext db, TestAnswerDTO answers, int? companyId, int? testId)
        {
            if (answers.testId == null)
            {
                return null;
            }
            var cataInCompany = db.CompanyCatalogue.Where(c => c.CompanyId == companyId).ToList();
            List<CataloguePointDTO> cataloguePoints = new List<CataloguePointDTO>();
            List<AnswerDTO> anss = new List<AnswerDTO>(answers.answers);
            List<int?> questIds = new List<int?>();
            anss.ForEach(a => questIds.Add(a.answerId));
            var quess = db.Answer.Include(a => a.Question).Where(an => questIds.Contains(an.AnswerId)).ToList();
            var ans = db.QuestionInTest.Include(q => q.Question).ThenInclude(q => q.Answer).Where(q => q.TestId == testId).ToList();
            foreach(CompanyCatalogue cata in cataInCompany)
            {
                double point = 0;
                double maxPoint = 0;
                List<int?> ids = new List<int?>();
                for (int i = 0; i < quess.Count; i++)
                {
                    if (quess[i].Question.CompanyCatalogueId == cata.CompanyCatalogueId)
                    {
                        maxPoint += quess[i].Question.Point;
                        point += quess[i].Percent * quess[i].Question.Point;
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
                    else if (cata.CompanyCatalogueId == ans[i].Question.CompanyCatalogueId)
                    {
                        maxPoint += ans[i].Question.Point;
                    }
                }


                double cataloguePoint = maxPoint == 0 ? 0 : (point / maxPoint);
                cataloguePoints.Add(new CataloguePointDTO(cata.CompanyCatalogueId, cataloguePoint));
            }
            return cataloguePoints;
        }

        public static List<TestInfoDTO> GetAllTestTodayByUsername(DeverateContext db, int? acccountId)
        {
            var result = from cf in db.Configuration
                         join t in db.Test on cf.ConfigId equals t.ConfigId
                         where t.AccountId == acccountId && t.IsActive == true
                         select new TestInfoDTO(cf.ConfigId, acccountId, t.TestId, cf.Title, null, t.Status, cf.StartDate, cf.EndDate);
            return result.ToList();
        }

        public static ConfigurationDTO GetConfig(DeverateContext db, int testId)
        {
            var config = db.Configuration.Include(z => z.Test).Where(c => c.Test.Any(x => x.TestId == testId)).FirstOrDefault();
            var test = config.Test.SingleOrDefault(t => t.TestId == testId);
            return new ConfigurationDTO(config, test);
        }

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
                        test.Status = "Doing";
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
                    result.Add(new QuestionInTestDTO(item.TestId, item.QuestionId, item.AnswerId, item.Question.Answer.ToList(), item.Question.Question1));
                }
                testU.accountId = test.AccountId;
                testU.code = test.Code;
                testU.testId = test.TestId;
                testU.startTime = test.StartTime;
                testU.questionInTest = result;
                return testU;
            }
        }

        public static List<TestInfoDTO> GetTestByConfig(DeverateContext db, int id)
        {
            var results = db.Test.Where(t => t.ConfigId == id).Select(t => new TestInfoDTO(t, t.Config.Title, t.Account.Username, t.Applicant.Fullname)).ToList();
            return results;
        }

        public static TestInfoDTO GetTestByTestId(int testId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                return context.Test.Where(t => t.TestId == testId).Select(t => new TestInfoDTO(t)).FirstOrDefault();
            }
        }

        public static void AutoSubmit(int testId)
        {
            TestInfoDTO testInfo = GetTestByTestId(testId);
            UserTest userTest = GetQuestionInTest(testInfo, false);
            EvaluateRank(userTest);
        }

        public static void SendMailQuizCode(List<int> listestResendCode, bool isEmployee)
        {
            using (DeverateContext context = new DeverateContext())
            {
                List<TestMailDTO> list = new List<TestMailDTO>();
                if (isEmployee)
                {
                    list = context.Test.Include(x => x.Account).Include(x => x.Config).Where(x => listestResendCode.Contains(x.TestId))
                     .Select(x => new TestMailDTO(
                         x.Account.Email,
                         x.Account.Fullname,
                         x.Config.Title,
                         x.Config.StartDate,
                         x.Config.EndDate,
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
                        x.Config.StartDate,
                        x.Config.EndDate,
                        x.Code,
                        x.TestId.ToString()
                        )
                    )
                    .ToList();
                }

                Producer.PublishMessage(JsonConvert.SerializeObject(list), AppConstrain.test_mail);
            }
        }

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
