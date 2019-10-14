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

        public static List<QuestionDTO> GetQuestionByCatalogue(int id)
        {
            using (DeverateContext context = new DeverateContext())

            {
                var question = context.Question.Where(ques => ques.CatalogueId == id).Select(ques => new QuestionDTO(ques, ques.Catalogue.Name, ques.Answer.ToList()));

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
                    question.CatalogueId = ques.CatalogueId;
                    question.Question1 = ques.Question1;
                    question.IsActive = true;
                    question.CreateBy = ques.CreateBy;
                    question.Answer = ques.Answer;
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
                    Question question = context.Question.Include(x=>x.Answer).SingleOrDefault(x=>x.QuestionId == ques.QuestionId);
                    question.Question1 = ques.Question1;
                    question.IsActive = ques.IsActive;
                    question.CreateBy = ques.CreateBy;
                    var answers = new List<Answer>();
                    foreach(var item in ques.Answer)
                    {
                        var answer = new Answer() { Answer1 = item.Answer1, Point = item.Point };
                        answers.Add(answer);
                    }
                    context.Answer.RemoveRange(question.Answer);
                    question.Answer = answers;
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
                    Question questionDb = context.Question.SingleOrDefault(c => c.QuestionId == ques.QuestionId);
                    questionDb.IsActive = false;
                    foreach(var item in questionDb.Answer.ToList())
                    {
                        item.IsActive = false;
                    }
                    context.SaveChanges();
                }
                return Message.removeQuestionSucceed;
            }
        }


    }
}
