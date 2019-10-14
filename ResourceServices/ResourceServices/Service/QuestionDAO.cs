using ResourceServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;
using ResourceServices.Models;

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
                                select new QuestionDTO(ques ,ques.Answer.ToList());
                return question.ToList();
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
            using (DeverateContext context = new DeverateContext())
            {
                Question question = context.Question.SingleOrDefault(c => c.QuestionId == ques.questionId);
                question.CatalogueId = ques.catalogueId;
                question.Question1 = ques.question1;
                question.IsActive = ques.isActive;
                question.MaxPoint = ques.maxPoint;
                //question.CreateBy = ques.CreateBy;
                //question.Answer = ques.Answer;
                //context.SaveChanges();
                return Message.updateQuestionSucceed; 
            }

        }

        public static string removeQuestion(List<QuestionDTO> Question)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ques in Question)
                {
                    Question questionDb = context.Question.SingleOrDefault(c => c.QuestionId == ques.questionId);
                    questionDb.IsActive = false;
                    context.SaveChanges();
                }
                return Message.removeQuestionSucceed;
            }
        }
    }
}
