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

        public static List<AnswerDTO> GetQuestionByStatus(bool status, int id)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var answer = from ans in context.Answer
                               where ans.IsActive == status && ans.QuestionId == id
                               select new AnswerDTO(ans);
                return answer.ToList();
            }

        }

        public static List<AnswerDTO> GetAnswerByQuestion(int id)
        {
            using (DeverateContext context = new DeverateContext())

            {
                var answer = context.Answer.Where(ans => ans.QuestionId == id && ans.IsActive == true).Select(ans => new AnswerDTO(ans));

                return answer.ToList();
            }

        }

        
        public static string CreateAnswer(AnswerDTO ans)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Answer answer = new Answer();
                answer.Answer1 = ans.Answer;
                answer.Point = ans.Point;
                answer.IsActive = true;
                answer.QuestionId = ans.QuestionId;
                context.Answer.Add(answer);
                context.SaveChanges();
                return Message.createAnswerSucceed;
            }

        }

        public static string UpdateAnswer(AnswerDTO ans)
        {
            try
            {
                using (DeverateContext context = new DeverateContext())
                {
                    Answer answer = new Answer();
                    answer.AnswerId = ans.AnswerId; 
                    answer.Answer1 = ans.Answer;
                    answer.Point = ans.Point;
                    answer.IsActive = true;
                    answer.QuestionId = ans.QuestionId;
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
                    Answer AnswerDb = context.Answer.SingleOrDefault(c => c.AnswerId == ans.AnswerId);
                    AnswerDb.IsActive = false;
                    context.SaveChanges();
                }
                return Message.removeAnswerSucceed;
            }
        }


    }
}
