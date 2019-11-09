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
    public class AnswerDAO
    {

        

        public static List<AnswerDTO> GetAnswerByQuestion(int? id, bool status)
        {
            using (DeverateContext context = new DeverateContext())

            {
                var answer = context.Answer.Where(ans => ans.QuestionId == id && ans.IsActive == status).Select(ans => new AnswerDTO(ans));

                return answer.ToList();
            }

        }

        
        public static string CreateAnswer(AnswerDTO ans)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Answer answer = new Answer();
                answer.Answer1 = ans.answer;
                answer.Point = ans.point;
                answer.IsActive = true;
                answer.QuestionId = ans.questionId;
                context.Answer.Add(answer);
                context.SaveChanges();
                return Message.createAnswerSucceed;
            }

        }

        public static void UpdateMaxPoint(int? id)
        {
            using (DeverateContext context = new DeverateContext())
            {
                int maxPoint = context.Answer.Where(a => a.QuestionId == id && a.IsActive == true).Max(a => a.Point);
                Question quesDb = context.Question.SingleOrDefault(ques => ques.QuestionId == id);
                quesDb.MaxPoint = maxPoint;
                context.SaveChanges();
            }
        }

        public static string UpdateAnswer(AnswerDTO ans)
        {
            try
            {
                using (DeverateContext context = new DeverateContext())
                {
                    Answer answer = new Answer();
                    answer.AnswerId = ans.answerId; 
                    answer.Answer1 = ans.answer;
                    answer.Point = ans.point;
                    answer.IsActive = true;
                    answer.QuestionId = ans.questionId;
                    context.Answer.Update(answer);
                    context.SaveChanges();
                    return Message.createAnswerSucceed;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "{\"message\" : \"Update Answer fail\"}";
            }
            

        }

        public static string removeAnswer(List<AnswerDTO> answer)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ans in answer)
                {
                    Answer AnswerDb = context.Answer.SingleOrDefault(c => c.AnswerId == ans.answerId);
                    AnswerDb.IsActive = ans.isActive;
                    context.SaveChanges();
                }
                return Message.removeAnswerSucceed;
            }
        }


    }
}
