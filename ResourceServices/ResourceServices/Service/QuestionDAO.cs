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


        public static QuestionDTO GetQuestionByCatalogue(int catalogueId, int companyId, bool status)
        {
            using (DeverateContext context = new DeverateContext())

            {
                var companyCata = context.CompanyCatalogue.Include(x => x.Question)
                    .Where(x => x.CompanyId == companyId && x.CompanyCatalogueId == catalogueId)
                    .Select(x => new QuestionDTO(x.Question.Where(q => q.IsActive == status).ToList(), x.Name, x.CompanyCatalogueId))
                    .SingleOrDefault();
                return companyCata;
            }

        }

        public static void CreateQuestion(List<QuestionDTO> quest)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ques in quest)
                {
                    Question question = new Question();
                    question.CompanyCatalogueId = ques.companyCatalogueId;
                    question.Question1 = ques.question1;
                    question.IsActive = true;
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
                question.Question1 = ques.question1;
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
                var check = context.Question.Where(x => ques.Contains(x.Question1) && x.CompanyCatalogueId == companyCatalogueId).Select(x => x.Question1).ToList();
                return check;
            }

        }

        public static List<string> checkExistedDefaultQuestion(List<string> ques, int defaultCatalogueId)
        {
            using (DeverateContext context = new DeverateContext())

            {
                var check = context.DefaultQuestion.Where(x => ques.Contains(x.Question) && x.DefaultCatalogueId == defaultCatalogueId).Select(x => x.Question).ToList();
                return check;
            }

        }

        public static void CreateDefaultQuestion(List<QuestionDefaultDTO> quest)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var defaultQuestions = new List<DefaultQuestion>();
                foreach (var ques in quest)
                {
                    defaultQuestions.Add(new DefaultQuestion
                    {
                        DefaultCatalogueId = ques.catalogueDefaultId,
                        Question = ques.question,
                        IsActive = true,
                        Point = ques.point,
                        CreateDate = DateTime.UtcNow,
                        DefaultAnswer = ques.answer
                    });
                }
                context.DefaultQuestion.AddRange(defaultQuestions);
                context.SaveChanges();
            }

        }

        public static void UpdateDefaultQuestion(QuestionDefaultDTO ques)
        {
            using (DeverateContext context = new DeverateContext())
            {
                DefaultQuestion question = context.DefaultQuestion.SingleOrDefault(x => x.DefaultQuestionId == ques.questionDefaultId);
                question.Question = ques.question;
                context.DefaultQuestion.Update(question);
                context.SaveChanges();
            }
        }

    }
}
