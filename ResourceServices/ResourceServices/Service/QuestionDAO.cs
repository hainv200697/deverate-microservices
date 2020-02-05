using ResourceServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;
using ResourceServices.Models;
using Microsoft.EntityFrameworkCore;

namespace ResourceServices.Service
{
    public class QuestionDAO
    {
        public static List<QuestionDTO> GetQuestionByCatalogue(int catalogueId, int companyId, bool status)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var companyCata = context.Question.Include(x => x.Catalogue)
                    .Where(x => x.Catalogue.CompanyId == companyId &&
                    (catalogueId != 0 ? x.Catalogue.CatalogueId == catalogueId : true) 
                    && x.IsActive == status)
                    .Select(x => new QuestionDTO(x, x.Catalogue.Name, x.CatalogueId))
                    .ToList();
                return companyCata;
            }
        }

        public static void CreateQuestion(List<QuestionDTO> quest)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ques in quest)
                {
                    foreach(var ans in ques.answer)
                    {
                        ans.CreateDate = DateTime.UtcNow;
                    }
                    Question question = new Question();
                    question.CatalogueId = ques.companyCatalogueId;
                    question.QuestionText = ques.question1;
                    question.IsActive = true;
                    question.CreateDate = DateTime.UtcNow;
                    question.Point = ques.point;
                    question.Answer = ques.answer;
                    context.Question.Add(question);
                }
                context.SaveChanges();
            }
        }

        public static void UpdateQuestion(QuestionDTO ques)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Question question = context.Question.SingleOrDefault(x => x.QuestionId == ques.questionId);
                question.QuestionText = ques.question1;
                question.Point = ques.point;
                context.Question.Update(question);
                context.SaveChanges();
            }
        }

        public static void removeQuestion(List<QuestionDTO> Question)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ques in Question)
                {
                    Question questionDb = context.Question.SingleOrDefault(c => c.QuestionId == ques.questionId);
                    questionDb.IsActive = ques.isActive;
                    if (ques.isActive == false)
                    {
                        List<AnswerDTO> answers = context.Answer.Where(answer => answer.QuestionId == questionDb.QuestionId).Select(answer => new AnswerDTO(answer)).ToList();
                        foreach (var item in answers)
                        {
                            Answer AnswerDb = context.Answer.SingleOrDefault(c => c.AnswerId == item.answerId);
                            AnswerDb.IsActive = false;
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public static List<string> checkExistedQuestion(List<string> ques, int companyCatalogueId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var check = context.Question.Where(x => ques.Contains(x.QuestionText) && x.CatalogueId == companyCatalogueId).Select(x => x.QuestionText).ToList();
                return check;
            }
        }

        public static List<string> checkExistedDefaultQuestion(List<string> ques, int defaultCatalogueId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var check = context.Question.Where(x => ques.Contains(x.QuestionText) && x.CatalogueId == defaultCatalogueId).Select(x => x.QuestionText).ToList();
                return check;
            }
        }

        public static void CreateDefaultQuestion(List<QuestionDefaultDTO> quest)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var defaultQuestions = new List<Question>();
                foreach (var ques in quest)
                {
                    foreach (var ans in ques.answer)
                    {
                        ans.CreateDate = DateTime.UtcNow;
                    }
                    defaultQuestions.Add(new Question
                    {
                        CatalogueId = ques.catalogueDefaultId,
                        QuestionText = ques.question,
                        IsActive = true,
                        Point = ques.point,
                        CreateDate = DateTime.UtcNow,
                        Answer = ques.answer
                    });
                }
                context.Question.AddRange(defaultQuestions);
                context.SaveChanges();
            }
        }

        public static void UpdateDefaultQuestion(QuestionDefaultDTO ques)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Question question = context.Question.SingleOrDefault(x => x.QuestionId == ques.questionDefaultId);
                question.QuestionText = ques.question;
                question.Point = ques.point;
                context.Question.Update(question);
                context.SaveChanges();
            }
        }

        public static List<QuestionDefaultDTO> GetQuestionByDefaultCatalogue(int catalogueId, bool status)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var defaultCata = context.Question.Include(x => x.Catalogue)
                    .Where(x => x.Catalogue.IsDefault == true &&
                    (catalogueId != 0 ? x.Catalogue.CatalogueId == catalogueId : true)
                    && x.IsActive == status)
                    .Select(x => new QuestionDefaultDTO(x, x.Catalogue.Name)).ToList();
                return defaultCata;
            }
        }

        public static void removeQuestionDefault(List<QuestionDefaultDTO> Question)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ques in Question)
                {
                    Question questionDb = context.Question.SingleOrDefault(c => c.QuestionId == ques.questionDefaultId);
                    questionDb.IsActive = ques.isActive;
                    if (ques.isActive == false)
                    {
                        List<AnswerDefaultDTO> answers = context.Answer.Where(answer => answer.QuestionId == questionDb.QuestionId).Select(answer => new AnswerDefaultDTO(answer)).ToList();
                        foreach (var item in answers)
                        {
                            Answer AnswerDb = context.Answer.SingleOrDefault(c => c.AnswerId == item.answerId);
                            AnswerDb.IsActive = false;
                        }
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
