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
                var companyCata = context.CatalogueInCompany.Include(x => x.Catalogue).Include(x => x.Question)
                    .Where(x => x.CompanyId == companyId && x.CatalogueId == catalogueId)
                    .Select(x => new QuestionDTO(x.Question.Where(q => q.IsActive == status).ToList(), x.Catalogue.Name, x.Cicid))
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
                    question.Cicid = ques.cicid;
                    question.Question1 = ques.question1;
                    question.IsActive = true;
                    question.MaxPoint = ques.maxPoint;
                    question.CreateBy = ques.createBy;
                    question.Answer = ques.answer;
                    question.CreateBy = ques.createBy;
                    question.Answer = ques.answer;
                    context.Question.Add(question);
                    context.SaveChanges();
                }
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
                            context.SaveChanges();
                        }
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
