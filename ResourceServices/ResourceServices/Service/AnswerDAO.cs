using ResourceServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ResourceServices.Models;

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
        
        public static void CreateAnswer(AnswerDTO ans)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Answer answer = new Answer();
                answer.AnswerText = ans.answer;
                answer.Percent = ans.percent;
                answer.IsActive = true;
                answer.CreateDate = DateTime.UtcNow;
                answer.QuestionId = ans.questionId.Value;
                context.Answer.Add(answer);
                context.SaveChanges();
            }
        }

        public static void UpdateAnswer(AnswerDTO ans)
        {
                using (DeverateContext context = new DeverateContext())
                {
                    Answer answer = new Answer();
                    answer.AnswerId = ans.answerId; 
                    answer.AnswerText = ans.answer;
                    answer.CreateDate = DateTime.UtcNow;
                    answer.Percent = ans.percent;
                    answer.IsActive = true;
                    answer.QuestionId = ans.questionId.Value;
                    context.Answer.Update(answer);
                    context.SaveChanges();
                }
        }

        public static void removeAnswer(List<AnswerDTO> answer)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ans in answer)
                {
                    Answer AnswerDb = context.Answer.SingleOrDefault(c => c.AnswerId == ans.answerId);
                    AnswerDb.IsActive = ans.isActive;
                }
                context.SaveChanges();
            }
        }
    }
}
