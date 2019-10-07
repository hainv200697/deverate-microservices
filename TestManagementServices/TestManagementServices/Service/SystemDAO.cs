using AuthenServices.Model;
using AuthenServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Model;
using TestManagementServices.Models;

namespace TestManagementServices.Service
{
    public class SystemDAO
    {

        public static List<QuestionDTO> GenerateTest(DeverateContext db, ConfigurationDTO config)
        {
            var emps = from c in db.Configuration
                       join a in db.Account on c.TestOwnerId equals a.AccountId
                       join co in db.Company on a.CompanyId equals co.CompanyId
                       where c.ConfigId == config.configId && a.RoleId == 3
                       select new AccountDTO(a);
            List<AccountDTO> accounts = emps.ToList();
            for(int i = 0; i < accounts.Count; i++)
            {
                GenerateQuestion(db, accounts[i].AccountId, config);
            }
            return null;
        }

        /// <summary>
        /// Tạo bài kiểm tra cho người dùng dưa trên config
        /// </summary>
        /// <param name="db"></param>
        /// <param name="accountId"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<QuestionDTO> GenerateQuestion(DeverateContext db, int? accountId, ConfigurationDTO config)
        {
            Random rand = new Random();
            List<CatalogueDTO> catalogues = GetCatalogueWeights(db, config.configId);
            for (int i = 0; i < catalogues.Count; i++)
            {
                catalogues[i].questions = GetQuestionOfCatalogue(db, catalogues[i].catalogueId);
            }
            catalogues = GetNumberOfQuestionEachCatalogue(db, config.totalQuestion, catalogues);
            List<QuestionDTO> questions = new List<QuestionDTO>();
            for (int i = 0; i < catalogues.Count; i++)
            {
                List<int?> quesIds = new List<int?>();
                List<QuestionDTO> totalQues = catalogues[i].questions;
                int quesLenght = totalQues.Count;
                int numbOfQues = catalogues[i].numberOfQuestion > catalogues[i].questions.Count ? catalogues[i].questions.Count : catalogues[i].numberOfQuestion.Value;
                for (int j = 0; j < numbOfQues; j++)
                {
                    int rQues = rand.Next(0, quesLenght);
                    if (!quesIds.Contains(totalQues[rQues].questionId))
                    {
                        quesIds.Add(totalQues[rQues].questionId);
                        questions.Add(totalQues[rQues]);
                    }
                    else
                    {
                        j--;
                    }
                }
            }
            return questions;
        }

        /// <summary>
        /// Lấy câu hỏi của từng catalogue
        /// </summary>
        /// <param name="db"></param>
        /// <param name="catalogueId"></param>
        /// <returns></returns>
        public static List<QuestionDTO> GetQuestionOfCatalogue(DeverateContext db, int? catalogueId)
        {
            var ques = from ca in db.Catalogue
                       join q in db.Question on ca.CatalogueId equals q.CatalogueId
                       where ca.CatalogueId == catalogueId
                       select new QuestionDTO(q.QuestionId, null);
            List<QuestionDTO> questions = ques.ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                questions[i].answers = GetAnswerOfQuestion(db, questions[i].questionId);
            }
            return questions;
        }

        /// <summary>
        /// Lấy câu trả lời của từng đâu hỏi
        /// </summary>
        /// <param name="db"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public static List<AnswerDTO> GetAnswerOfQuestion(DeverateContext db, int? questionId)
        {
            var answers = from q in db.Question
                          join a in db.Answer on q.QuestionId equals a.QuestionId
                          where q.QuestionId == questionId
                          select new AnswerDTO(a);
            return answers.ToList();
        }

        public static List<CatalogueDTO> GetNumberOfQuestionEachCatalogue(DeverateContext db,  int? totalQuestion, List<CatalogueDTO> catalogues)
        {
            int currentQuestion = 0;
            for(int i = 0; i < catalogues.Count; i++)
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
            var result = from cf in db.Configuration
                         join cif in db.CatalogueInConfiguration on cf.ConfigId equals cif.ConfigId
                         where cf.ConfigId == configId
                         select new CatalogueDTO(cif.CatalogueId, cif.Catalogue.Name, 0, cif.WeightPoint, null, cif.Catalogue.IsActive);
            if (result == null)
            {
                return null;
            }
            return result.ToList();
        }



        /// <summary>
        /// Đánh giá rank dựa trên bài test
        /// </summary>
        /// <param name="db"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        public static RankPoint EvaluateRank(DeverateContext db, TestAnswerDTO answers)
        {

                double? totalPoint = CalculateResultPoint(db, answers);
                List<ConfigurationRankDTO> configurationRanks = GetRankPoint(db, answers);
                configurationRanks = configurationRanks.OrderBy(o => o.point).ToList();
                ConfigurationRankDTO tmp = new ConfigurationRankDTO();
                tmp.rankId = configurationRanks[0].rankId.Value;
                tmp.point = configurationRanks[0].point;
                foreach (ConfigurationRankDTO cr in configurationRanks)
                {
                    if (tmp.point > cr.point)
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
            int? testId = db.AccountInTest.SingleOrDefault(a => a.Aitid == answer.AITId).TestId;
            if (testId == null)
            {
                return null;
            }
            var result = from con in db.Configuration
                         join te in db.Test on con.ConfigId equals te.ConfigId
                         join cr in db.ConfigurationRank on con.ConfigId equals cr.ConfigId
                         where te.TestId == testId
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
            int? testId = db.AccountInTest.SingleOrDefault(a => a.Aitid == answers.AITId).TestId;
            if (testId == null)
            {
                return -1;
            }
            List<CatalogueWeightPointDTO> catalogueWeightPoints = GetWeightPoints(db, testId);
            for (int i = 0; i < cataloguePoints.Count; i++)
            {
                totalPoint += (cataloguePoints[i].cataloguePoint / catalogueWeightPoints[i].weightPoint);
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

            int? testId = db.AccountInTest.SingleOrDefault(ait => ait.Aitid == answers.AITId).TestId;
            if (testId == null)
            {
                return null;
            }

            var catas = from t in db.Test
                        join cf in db.Configuration on t.ConfigId equals cf.ConfigId
                        join cif in db.CatalogueInConfiguration on cf.ConfigId equals cif.ConfigId
                        join c in db.Catalogue on cif.CatalogueId equals c.CatalogueId
                        where t.TestId == testId
                        select new CatalogueDTO(c);


            List<CatalogueDTO> catalogues = catas.ToList();
            List<CataloguePointDTO> cataloguePoints = new List<CataloguePointDTO>();
            foreach (CatalogueDTO cata in catalogues)
            {
                float? point = 0;
                float? maxPoint = 0;
                foreach (AnswerDTO answer in answers.answers)
                {
                    Answer ans = db.Answer.SingleOrDefault(an => an.AnswerId == answer.AnswerId);
                    if (ans == null)
                    {
                        continue;
                    }
                    if (ans.Question.CatalogueId == cata.catalogueId)
                    {
                        maxPoint += ans.Question.MaxPoint;
                        point += ans.Point;
                    }
                }
                float? cataloguePoint = (point / maxPoint);
                cataloguePoints.Add(new CataloguePointDTO(cata.catalogueId, cataloguePoint));
            }
            return cataloguePoints;
        }
    }
}
