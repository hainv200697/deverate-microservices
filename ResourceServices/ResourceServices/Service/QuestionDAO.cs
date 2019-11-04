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
        public static List<QuestionDTO> GetAllQuestion()
        {
            using (DeverateContext context = new DeverateContext())
            {
                var question = from ques in context.Question
                               where ques.IsActive == true
                                select new QuestionDTO(ques,ques.Catalogue.Name ,ques.Answer.ToList());
                return question.ToList();
            }

        }

        public static List<QuestionDTO> GetQuestionByStatus(bool status,int id)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var question = from ques in context.Question
                               where ques.IsActive == status && ques.CatalogueId == id
                               select new QuestionDTO(ques, ques.Catalogue.Name, ques.Answer.ToList());
                return question.ToList();
            }

        }

        public static List<QuestionDTO> GetQuestionByCatalogue(int catalogueId,int companyId, bool status)
        {
            using (DeverateContext context = new DeverateContext())

            {
                var cata = context.Catalogue.SingleOrDefault(x => x.CatalogueId == catalogueId);
                if (cata.Type == true)
                {
                    var companyCata = context.CompanyCatalogue.SingleOrDefault(x => x.CompanyId == companyId && x.CatalogueId == catalogueId);
                    if(companyCata == null)
                    {
                        return null;
                    }
                }
                var question = context.Question.Where(ques => ques.CatalogueId == catalogueId && ques.IsActive == status)
                        .Select(ques => new QuestionDTO(ques, ques.Catalogue.Name, ques.Answer.Where(ans=> ans.IsActive == true).ToList()));


                return question.ToList();
            }

        }

        public static string CreateQuestionExcel(List<QuestionDTO> quest)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ques in quest)
                {
                    Question question = new Question();
                    question.CatalogueId = ques.catalogueId;
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
                return Message.createQuestionSucceed;
            }

        }
        public static string CreateQuestion(QuestionDTO ques)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Question question = new Question();
                question.CatalogueId = ques.catalogueId;
                question.Question1 = ques.question1;
                question.IsActive = ques.isActive;
                question.MaxPoint = ques.maxPoint;
                question.CreateBy = ques.createBy;
                question.Answer = ques.answer;
                context.Question.Add(question);
                context.SaveChanges();
                return Message.createQuestionSucceed;
            }

        }

        public static string UpdateQuestion(QuestionDTO ques)
        {
            try
            {
                using (DeverateContext context = new DeverateContext())
                {
                    Question question = context.Question.SingleOrDefault(x=>x.QuestionId == ques.questionId);
                    question.Question1 = ques.question1;
                    int maxPoint = context.Answer.Where(ans => ans.QuestionId == question.QuestionId).Max(ans => ans.Point);
                    question.MaxPoint = maxPoint;
                    context.Question.Update(question);
                    context.SaveChanges();
                    return Message.updateQuestionSucceed; 
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "{\"message\" : \"Update Question fail\"}";
            }
            

        }

        public static string removeQuestion(List<QuestionDTO> Question)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ques in Question)
                {
                    Question questionDb = context.Question.SingleOrDefault(c => c.QuestionId == ques.questionId);
                    questionDb.IsActive = ques.isActive;
                    if(ques.isActive == false) { 
                        List<AnswerDTO> answers = context.Answer.Where(answer => answer.QuestionId == questionDb.QuestionId).Select(answer => new AnswerDTO(answer)).ToList();
                        foreach(var item in answers)
                        {
                            Answer AnswerDb = context.Answer.SingleOrDefault(c => c.AnswerId == item.answerId);
                            AnswerDb.IsActive = false;
                            context.SaveChanges();
                        }
                    }
                    context.SaveChanges();
                }
                return Message.removeQuestionSucceed;
            }
        }
    }
}
