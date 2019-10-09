using ResourceServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;

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

        public static string CreateQuestion(List<QuestionDTO> quest)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ques in quest)
                {
                    Question question = new Question();
                    question.CatalogueId = ques.CatalogueId;
                    question.Question1 = ques.Question1;
                    question.IsActive = ques.IsActive;
                    question.CreateBy = ques.CreateBy;
                    question.Answer = ques.Answer;
                    context.Question.Add(question);
                    context.SaveChanges();
                }
                return "Creating Question success";
            }

        }

        public static string UpdateQuestion(QuestionDTO ques)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Question question = context.Question.SingleOrDefault(c => c.QuestionId == ques.QuestionId);
                question.CatalogueId = ques.CatalogueId;
                question.Question1 = ques.Question1;
                question.IsActive = ques.IsActive;
                question.MaxPoint = ques.MaxPoint;
                question.CreateBy = ques.CreateBy;
                question.Answer = ques.Answer;
                context.SaveChanges();
                return "UPdating Question success"; 
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
                    context.SaveChanges();
                }
                return "Removing Question success";
            }
        }
    }
}
