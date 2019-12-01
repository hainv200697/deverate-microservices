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
        public static string SendTestMail(int? configId, bool isUpdate)
        {
            using(DeverateContext db = new DeverateContext())
            {
                {
                    var result = from c in db.Configuration
                                 join t in db.Test on c.ConfigId equals t.ConfigId
                                 join a in db.Account on t.AccountId equals a.AccountId
                                 where c.ConfigId == configId && c.IsActive == true
                                 select new TestMailDTO(a.Email, a.Fullname, c.Title, c.StartDate, c.EndDate,
                                 isUpdate == false ? t.Code: null, t.TestId.ToString());
                    if(result.ToList().Count == 0)
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
            using(DeverateContext context = new DeverateContext())
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

        public static SampleTestDTO GenerateQuestionsForSampleTest(DeverateContext db, SampleConfigDTO con)
        {

            List<QuestionDTO> questions = new List<QuestionDTO>();
            List<QuestionRemain> remainQues = new List<QuestionRemain>();
            List<QuestionDTO> unchoosedQues = new List<QuestionDTO>();
            List<QuestionDTO> choosedQues = new List<QuestionDTO>();
            List<int?> quesIds = new List<int?>();
            Random rand = new Random();
            int totalCataQues = 0;

            List<int?> cIds = new List<int?>();
            con.catalogueInSamples.ForEach(c => cIds.Add(c.cicId));
            List<CatalogueInCompany> catalogueIns = db.CatalogueInCompany.Include(c => c.Catalogue).Include(c => c.Question).ThenInclude(c => c.Answer).Where(c => cIds.Contains(c.Catalogue.CatalogueId) && c.CompanyId == con.companyId).ToList();
            List<Question> cloneQuesList = new List<Question>();
            foreach(CatalogueInCompany cic in catalogueIns)
            {
                for(int i = 0; i < cic.Question.ToList().Count; i++)
                {
                    if(cic.Question.ToList()[i].IsActive == true)
                    {
                        cloneQuesList.Add(cic.Question.Where(q => q.IsActive == true).ToList()[i]);
                    }
                }
                cic.Question = cloneQuesList;
                cloneQuesList = new List<Question>();
            }
            if(catalogueIns.Count == 0)
            {
                return null;
            }
            List<CatalogueDTO> catalogues = new List<CatalogueDTO>();
            for(int i = 0; i < catalogueIns.Count; i++)
            {
                for(int j = 0; j < con.catalogueInSamples.Count; j++)
                {
                    if(con.catalogueInSamples[j].cicId == catalogueIns[i].CatalogueId)
                    {
                        catalogues.Add(new CatalogueDTO(catalogueIns[i].CatalogueId, catalogueIns[i].Catalogue.Name, 0, con.catalogueInSamples[j].weightPoint, catalogueIns[i].Question.ToList()));
                        totalCataQues += catalogueIns[i].Question.ToList().Count;
                    }
                }
            }
            if (catalogues == null || catalogues.Count == 0)
            {
                return null;
            }

            if (totalCataQues == 0)
            {
                return null;

            }
            catalogues = catalogues.OrderBy(o => o.weightPoint).ToList();

            int totalOfQues = con.totalQuestion.Value > totalCataQues ? totalCataQues : con.totalQuestion.Value;
            catalogues = GetNumberOfQuestionEachCatalogue(db, totalOfQues, catalogues);


            for (int i = 0; i < catalogues.Count; i++)
            {
                QuestionRemain quesRe = new QuestionRemain();
                quesRe.catalogueId = catalogues[i].catalogueId;

                List<Question> totalQues = catalogues[i].questionList;
                if (totalQues == null || totalQues.Count == 0)
                {
                    continue;
                }
                int quesLenght = totalQues.Count;
                int numbOfQues = catalogues[i].numberOfQuestion > quesLenght ? quesLenght : catalogues[i].numberOfQuestion.Value;
                totalQues = ShuffleQuestion(totalQues);
                for (int j = 0; j < numbOfQues; j++)
                {
                    if (!quesIds.Contains(totalQues[j].QuestionId))
                    {
                        quesIds.Add(totalQues[j].QuestionId);
                        List<AnswerDTO> answers = totalQues[j].Answer.Select(a => new AnswerDTO(a)).ToList();
                        questions.Add(new QuestionDTO(totalQues[j].QuestionId, totalQues[j].Question1, totalQues[j].Cic.CatalogueId, answers));
                        if (numbOfQues != quesLenght)
                        {
                            choosedQues.Add(new QuestionDTO(totalQues[j].QuestionId, answers));
                        }
                    }
                    else
                    {
                        j--;
                    }
                }
                quesRe.curNumbQues = numbOfQues;
                quesRe.numbCataQues = catalogues[i].questionList.Count;

                if (numbOfQues != quesLenght)
                {
                    for (int k = 0; k < quesLenght; k++)
                    {
                        List<AnswerDTO> answers = totalQues[k].Answer.Select(a => new AnswerDTO(a)).ToList();
                        bool isContainQues = false;
                        for (int m = 0; m < choosedQues.Count; m++)
                        {
                            if (choosedQues[m].questionId == totalQues[k].QuestionId)
                            {
                                isContainQues = true;
                                break;
                            }
                        }
                        if (isContainQues == false)
                        {
                            unchoosedQues.Add(new QuestionDTO(totalQues[k].QuestionId, totalQues[k].Question1, totalQues[k].Cic.CatalogueId, answers));
                        }
                    }
                }
                quesRe.unchoosedQues = unchoosedQues;
                quesRe.weightPoint = catalogues[i].weightPoint;
                remainQues.Add(quesRe);
            }
            questions = fillQues(remainQues, totalOfQues, questions);
            questions = Shuffle(questions);
            for(int i = 0; i < catalogues.Count; i++)
            {
                catalogues[i].questionList = null;

            }
            return new SampleTestDTO(catalogues, questions);
        }

        public static string GenerateTestForApplicants(string configId, List<ApplicantDTO> applicants)
        {
            using (DeverateContext context = new DeverateContext())
            {
                    Configuration con = context.Configuration.Include(c => c.Account).SingleOrDefault(o => o.ConfigId == Int32.Parse(configId));
                    if (con.Duration < AppConstrain.minDuration)
                    {
                        return Message.durationExceptopn;
                    }
                    List<CatalogueDTO> catas = GetCatalogueWeights(context, con.ConfigId);
                    if (catas.Count == 0 || catas == null)
                    {
                        return Message.noCatalogueException;
                    }
                    if (con.TotalQuestion < catas.Count)
                    {
                        return Message.numberQuestionExceptopn;
                    }
                    if (applicants.Count == 0)
                    {
                        return Message.noApplicantException;
                    }
                    GenerateQuestionsForApplicants(context, applicants, con);
                    List<int> applicantIds = new List<int>();
                    foreach(ApplicantDTO applicant in applicants)
                    {
                        applicantIds.Add(applicant.applicantId);
                    }
                    SendMailQuizCode(applicantIds, false);
                return null;
            }
        }

        public static int isAvailableTest(int? accountId, List<Test> tests)
        {
            for(int i = 0; i < tests.Count; i++)
            {
                if (tests[i].AccountId == accountId) return i;
            }
            return -1;
        }

        public static List<Test> removeAvailableTests(List<int?> accountIds, List<Test> tests)
        {
            for (int i = 0; i < tests.Count; i++)
            {
                for(int j = 0; j < accountIds.Count; j++)
                {
                    if(accountIds[j] == tests[i].AccountId)
                    {
                        tests[i].IsActive = false;
                        break;
                    }
                }
            }
            return tests;
        }

        public static void GenerateQuestionsForApplicants(DeverateContext db, List<ApplicantDTO> applicants, Configuration config)
        {
            List<QuestionDTO> questions = new List<QuestionDTO>();
            List<QuestionRemain> remainQues = new List<QuestionRemain>();
            List<QuestionDTO> unchoosedQues = new List<QuestionDTO>();
            List<QuestionDTO> choosedQues = new List<QuestionDTO>();
            List<int?> quesIds = new List<int?>();
            Random rand = new Random();
            int totalCataQues = 0;


            List<CatalogueDTO> catalogues = GetCatalogueWeights(db, config.ConfigId);
            if(catalogues == null || catalogues.Count == 0  )
            {
                return;
            }
            catalogues = catalogues.OrderBy(o => o.weightPoint).ToList();
            for (int i = 0; i < catalogues.Count; i++)
            {
                catalogues[i].questions = GetQuestionOfCatalogue(db, catalogues[i].catalogueId, config.Account.CompanyId);
                totalCataQues += catalogues[i].questions == null ? 0: catalogues[i].questions.Count;
            }
            if(totalCataQues == 0)
            {
                return;

            }
            int totalOfQues = config.TotalQuestion > totalCataQues ? totalCataQues : config.TotalQuestion;
            catalogues = GetNumberOfQuestionEachCatalogue(db, totalOfQues, catalogues);

            foreach (ApplicantDTO app in applicants)
            {
                for (int i = 0; i < catalogues.Count; i++)
                {
                    QuestionRemain quesRe = new QuestionRemain();
                    quesRe.catalogueId = catalogues[i].catalogueId;

                    List<QuestionDTO> totalQues = catalogues[i].questions;
                    if(totalQues == null || totalQues.Count == 0)
                    {
                        continue;
                    }
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
                test.ApplicantId = app.applicantId;
                test.CreateDate = DateTime.UtcNow;
                test.IsActive = true;
                test.Status = "Pending";
                test.Code = GenerateCode();
                db.Test.Add(test);
                db.SaveChanges();
                List<QuestionInTest> questionInTests = new List<QuestionInTest>();
                for (int i = 0; i < questions.Count; i++)
                {
                    QuestionInTest inTest = new QuestionInTest();
                    inTest.TestId = test.TestId;
                    inTest.QuestionId = questions[i].questionId;
                    inTest.IsActive = true;
                    questionInTests.Add(inTest);

                }
                db.QuestionInTest.AddRange(questionInTests);
                db.SaveChanges();
                questions = new List<QuestionDTO>();
                remainQues = new List<QuestionRemain>();
                unchoosedQues = new List<QuestionDTO>();
                choosedQues = new List<QuestionDTO>();
                quesIds = new List<int?>();
            }
        }

        public static string GenerateTest(string configId)
        {
            using (DeverateContext context = new DeverateContext())
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
                Account acc = context.Account.SingleOrDefault(o => o.AccountId == con.AccountId);


                var emps = from a in context.Account
                            where a.CompanyId == acc.CompanyId && a.RoleId == AppConstrain.empRole && a.IsActive == true
                            select new AccountDTO(a);
                List<AccountDTO> accounts = emps.ToList();
                if (accounts.Count == 0)
                {
                    return Message.noEmployeeException;
                }
                GenerateQuestions(context, accounts, con, acc.CompanyId);
                return null;
            }
        }


        public static void GenerateQuestions(DeverateContext db, List<AccountDTO> accounts, Configuration config, int? companyId)
        {
            List<QuestionDTO> questions = new List<QuestionDTO>();
            List<QuestionRemain> remainQues = new List<QuestionRemain>();
            List<QuestionDTO> unchoosedQues = new List<QuestionDTO>();
            List<QuestionDTO> choosedQues = new List<QuestionDTO>();
            List<int?> quesIds = new List<int?>();
            Random rand = new Random();
            int totalCataQues = 0;

            List<int?> accountIds = new List<int?>();
            accounts.ForEach(a => accountIds.Add(a.accountId));
            List<Test> tests = db.Test.Where(t => t.ConfigId == config.ConfigId).ToList();
            tests = removeAvailableTests(accountIds, tests);
            db.SaveChanges();
            List<CatalogueDTO> catalogues = GetCatalogueWeights(db, config.ConfigId);
            catalogues = catalogues.OrderBy(o => o.weightPoint).ToList();
            for (int i = 0; i < catalogues.Count; i++)
            {
                catalogues[i].questions = GetQuestionOfCatalogue(db, catalogues[i].catalogueId, companyId);
                totalCataQues += catalogues[i].questions.Count;
            }
            if(totalCataQues == 0)
            {
                return;
            }
            int totalOfQues = config.TotalQuestion > totalCataQues ? totalCataQues : config.TotalQuestion;
            catalogues = GetNumberOfQuestionEachCatalogue(db, totalOfQues, catalogues);


            foreach (AccountDTO acc in accounts)
            {
                for (int i = 0; i < catalogues.Count; i++)
                {
                    QuestionRemain quesRe = new QuestionRemain();
                    quesRe.catalogueId = catalogues[i].catalogueId;
                        
                    List<QuestionDTO> totalQues = catalogues[i].questions;
                        
                    if(totalQues == null || totalQues.Count == 0)
                    {
                        continue;
                    }
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
                test.CreateDate = DateTime.UtcNow;
                test.IsActive = true;
                test.Status = "Pending";
                test.Code = GenerateCode();
                db.Test.Add(test);
                db.SaveChanges();
                List<QuestionInTest> questionInTests = new List<QuestionInTest>();
                for (int i = 0; i < questions.Count; i++)
                {
                    QuestionInTest inTest = new QuestionInTest();
                    inTest.TestId = test.TestId;
                    inTest.QuestionId = questions[i].questionId;
                    inTest.IsActive = true;
                    questionInTests.Add(inTest);
                        
                }
                db.QuestionInTest.AddRange(questionInTests);
                db.SaveChanges();
                questions = new List<QuestionDTO>();
                remainQues = new List<QuestionRemain>();
                unchoosedQues = new List<QuestionDTO>();
                choosedQues = new List<QuestionDTO>();
                quesIds = new List<int?>();
            }
        }

        public static List<QuestionDTO> GenerateQuestion(DeverateContext db, int? accountId, Configuration config, int? companyId)
        {
            List<QuestionDTO> questions = new List<QuestionDTO>();
            Random rand = new Random();
            int totalCataQues = 0;
            List<CatalogueDTO> catalogues = GetCatalogueWeights(db, config.ConfigId);
            catalogues = catalogues.OrderByDescending(o => o.weightPoint).ToList();
            for (int i = 0; i < catalogues.Count; i++)
            {
                catalogues[i].questions = GetQuestionOfCatalogue(db, catalogues[i].catalogueId, companyId);
                totalCataQues += catalogues[i].questions.Count;
            }
            int totalOfQues = config.TotalQuestion > totalCataQues ? totalCataQues : config.TotalQuestion;
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
            test.CreateDate = DateTime.UtcNow;
            test.IsActive = true;
            test.Status = "Pending";
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

        public static List<QuestionDTO> GetQuestionOfCatalogue(DeverateContext db, int? catalogueId, int? companyId)
        {

            var ques = from ca in db.CatalogueInCompany
                        join q in db.Question on ca.Cicid equals q.Cicid
                        where ca.CatalogueId == catalogueId && ca.CompanyId == companyId && q.IsActive == true
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

        public static List<CatalogueDTO> GetNumberOfQuestionEachCatalogue(DeverateContext db, int? totalQuestion, List<CatalogueDTO> catalogues)
        {
            if(totalQuestion == 0)
            {
                return catalogues;
            }
            int currentQuestion = 0;
            for (int i = 0; i < catalogues.Count; i++)
            {
                double numberOfQuestion = catalogues[i].weightPoint == null || catalogues[i].weightPoint.Value <= 0 ? 0: catalogues[i].weightPoint.Value * totalQuestion.Value / AppConstrain.scaleUpNumb;
                catalogues[i].numberOfQuestion = Convert.ToInt32(numberOfQuestion);
                if(catalogues[i].numberOfQuestion == 0)
                {
                    catalogues[i].numberOfQuestion = 1;
                }
                currentQuestion += catalogues[i].numberOfQuestion.Value;
            }
            int dif = currentQuestion - totalQuestion.Value;
            catalogues[catalogues.Count - 1].numberOfQuestion = catalogues[catalogues.Count - 1].numberOfQuestion.Value - dif;
            return catalogues;
        }

        public static List<CatalogueDTO> GetCatalogueWeights(DeverateContext db, int? configId)
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



        public static RankPoint EvaluateRank(UserTest userTest)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Test test = context.Test.Include(t => t.Account).Include(t => t.Config.Account).SingleOrDefault(t => t.TestId == userTest.testId && t.Code == userTest.code);
                if (test == null)
                {
                    return null;
                }

                test.Status = "Submitted";
                test.FinishTime = DateTime.UtcNow;
                context.SaveChanges();
                List<AnswerDTO> answers = new List<AnswerDTO>();
                List<int?> answerIds = new List<int?>();
                for (int i = 0; i < userTest.questionInTest.Count; i++)
                {
                    answerIds.Add(userTest.questionInTest[i].answerId);
                }
                var anss = context.Answer.Where(a => answerIds.Contains(a.AnswerId)).ToList();
                Statistic statistic = new Statistic();
                string rank = "Dev0";
                double totalPoint = 0;
                statistic.TestId = userTest.testId;
                statistic.IsActive = true;
                if (anss.Count != 0)
                {
                    anss.ForEach(a => answers.Add(new AnswerDTO(a)));
                    TestAnswerDTO testAnswer = new TestAnswerDTO(answers, userTest.testId);
                    totalPoint = CalculateResultPoint(context, testAnswer, statistic, test.Config.Account.CompanyId, userTest.testId);


                    totalPoint = AppConstrain.RoundDownNumber(totalPoint, 1);
                    List<ConfigurationRankDTO> configurationRanks = GetRankPoint(context, testAnswer);
                    configurationRanks = configurationRanks.OrderBy(o => o.point).ToList();
                    ConfigurationRankDTO tmp = new ConfigurationRankDTO();
                    tmp.rankId = configurationRanks[0].rankId;
                    tmp.point = configurationRanks[0].point;
                    foreach (ConfigurationRankDTO cr in configurationRanks)
                    {
                        if (totalPoint > cr.point)
                        {
                            tmp = cr;
                        }
                    }
                    rank = context.Rank.SingleOrDefault(r => r.RankId == tmp.rankId).Name;
                    if (rank == null)
                    {
                        return null;
                    }
                    statistic.RankId = tmp.rankId;
                    statistic.Point = totalPoint;
                }
                else
                {
                    statistic.RankId = 4;
                    statistic.Point = 0;
                }
                context.Statistic.Add(statistic);
                context.SaveChanges();

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
                List<int> qitIds = new List<int>();
                for (int i = 0; i < userTest.questionInTest.Count; i++)
                {
                    qitIds.Add(userTest.questionInTest[i].qitid.Value);
                }

                var qitss = db.QuestionInTest.Where(o => qitIds.Contains(o.Qitid)).ToList();
                for (int i = 0; i < qitss.Count; i++)
                {
                    for (int j = 0; j < userTest.questionInTest.Count; j++)
                    {
                        if (qitss[i].Qitid == userTest.questionInTest[j].qitid)
                            qitss[i].AnswerId = userTest.questionInTest[j].answerId;
                    }

                }
                db.SaveChanges();
            }
        }

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

        public static double CalculateResultPoint(DeverateContext db, TestAnswerDTO answers, Statistic statistic, int? companyId, int? testId)
        {
            double totalPoint = 0;
            List<CataloguePointDTO> defaultCataloguePoints = CalculateCataloguePoints(db, answers, companyId, testId);
            if (answers.testId == null)
            {
                return -1;
            }
            List<CataloguePointDTO> cataloguePoints = new List<CataloguePointDTO>();
            List<CatalogueWeightPointDTO> catalogueWeightPoints = GetWeightPoints(db, answers.testId);
            for(int i = 0; i < defaultCataloguePoints.Count; i++)
            {
                for(int j = 0; j < catalogueWeightPoints.Count; j++)
                {
                    if(defaultCataloguePoints[i].catalogueId == catalogueWeightPoints[j].catalogueId)
                    {
                        if (!cataloguePoints.Contains(defaultCataloguePoints[i]))
                        {
                            cataloguePoints.Add(defaultCataloguePoints[i]);
                        }
                        
                        break;
                    } 
                }
            }
            List<DetailStatistic> details = new List<DetailStatistic>();
            for (int i = 0; i < cataloguePoints.Count; i++)
            {
                DetailStatistic detail = new DetailStatistic();
                detail.CatalogueId = cataloguePoints[i].catalogueId;
                if(cataloguePoints[i].cataloguePoint < 0)
                {
                    continue;
                }
                double point = cataloguePoints[i].cataloguePoint * catalogueWeightPoints[i].weightPoint;
                detail.Point = cataloguePoints[i].cataloguePoint;
                detail.IsActive = true;
                details.Add(detail);
                totalPoint += point;
            }
            if(details.Count > 0)
            {
                statistic.DetailStatistic = details;
            }
            return totalPoint;
        }

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

        public static List<CataloguePointDTO> CalculateCataloguePoints(DeverateContext db, TestAnswerDTO answers, int? companyId, int? testId)
        {
            if (answers.testId == null)
            {
                return null;
            }
            //var catalogues = db.Catalogue.Where(c => c.CatalogueInConfiguration.Any(cif => cif.Config.Test.Any(o => o.TestId == answers.testId))).ToList();
            //var cataInCompany = db.CatalogueInCompany.Where(c => c.Catalogue.CatalogueInConfiguration.Any(cif => cif.Config.Test.Any(o => o.TestId == answers.testId))).ToList();
            var cataInCompany = db.CatalogueInCompany.Where(c => c.CompanyId == companyId).ToList();
            List<CataloguePointDTO> cataloguePoints = new List<CataloguePointDTO>();
            List<AnswerDTO> anss = new List<AnswerDTO>(answers.answers);
            List<int?> questIds = new List<int?>();
            anss.ForEach(a => questIds.Add(a.answerId));
            var quess = db.Answer.Include(a => a.Question).Where(an => questIds.Contains(an.AnswerId)).ToList();
            var ans = db.QuestionInTest.Include(q => q.Question).ThenInclude(q => q.Answer).Where(q => q.TestId == testId).ToList();
            foreach (CatalogueInCompany cata in cataInCompany)
            {
                float point = 0;
                float maxPoint = 0;
                List<int?> ids = new List<int?>();
                for(int i = 0; i < quess.Count; i++)
                {
                    if(quess[i].Question.Cicid == cata.Cicid)
                    {
                        maxPoint += quess[i].Question.MaxPoint;
                        point += quess[i].Point;
                        ids.Add(quess[i].QuestionId);
                        quess.RemoveAt(i);
                        i--;
                        
                    }
                }
                for(int i = 0; i < ans.Count; i++)
                {
                    if (ids.Contains(ans[i].QuestionId))
                    {
                        ans.RemoveAt(i);
                        i--;
                    }
                    else if (cata.Cicid == ans[i].Question.Cicid)
                    {
                        
                        point += 0;
                        maxPoint += ans[i].Question.MaxPoint;
                    }
                }
                
                
                float cataloguePoint = maxPoint == 0 ? 0: (point / maxPoint);
                cataloguePoints.Add(new CataloguePointDTO(cata.CatalogueId  , cataloguePoint));
            }
            return cataloguePoints;
        }

        public static List<TestInfoDTO> GetAllTestTodayByUsername(DeverateContext db, int? acccountId)
        {
            var result = from cf in db.Configuration
                         join t in db.Test on cf.ConfigId equals t.ConfigId
                         where t.AccountId == acccountId && t.IsActive == true
                         select new TestInfoDTO(cf.ConfigId, acccountId, t.TestId, cf.Title , null, t.Status, cf.StartDate, cf.EndDate);
            return result.ToList();
        }

        public static ConfigurationDTO GetConfig(DeverateContext db, int testId)
        {
            var config = db.Configuration.Include(z=>z.Test).Where(c => c.Test.Any(x=>x.TestId == testId)).FirstOrDefault();
            var test = config.Test.SingleOrDefault(t => t.TestId == testId);
            return new ConfigurationDTO(config, test);
        }

        public static UserTest GetQuestionInTest(TestInfoDTO testInfo, bool checkCode)
        {
            using(DeverateContext context = new DeverateContext())
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
                    result.Add(new QuestionInTestDTO(item.Qitid, item.Question.Answer.ToList(), item.AnswerId, item.Question.Question1));
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
            var results = db.Test.Where(t => t.ConfigId == id).Select(t => new TestInfoDTO(t, t.Config.Title, t.Account.Username,t.Applicant.Fullname)).ToList();
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
                } else
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
