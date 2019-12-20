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

        
        public static void CreateAnswer(AnswerDTO ans)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Answer answer = new Answer();
                answer.Answer1 = ans.answer;
                answer.Percent = ans.percent;
                answer.IsActive = true;
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
                    answer.Answer1 = ans.answer;
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

        public static List<AnswerDefaultDTO> GetDefaultAnswerByQuestion(int? id, bool status)
        {
            using (DeverateContext context = new DeverateContext())

            {
                var answer = context.DefaultAnswer.Where(ans => ans.DefaultQuestionId == id && ans.IsActive == status).Select(ans => new AnswerDefaultDTO(ans));

                return answer.ToList();
            }

        }


        public static void CreateDefaultAnswer(AnswerDefaultDTO ans)
        {
            using (DeverateContext context = new DeverateContext())
            {
                DefaultAnswer answer = new DefaultAnswer();
                answer.Answer = ans.answer;
                answer.Percent = ans.percent;
                answer.IsActive = true;
                answer.DefaultQuestionId = ans.questionId;
                context.DefaultAnswer.Add(answer);
                context.SaveChanges();
            }

        }


        public static void UpdateDefaultAnswer(AnswerDefaultDTO ans)
        {
            using (DeverateContext context = new DeverateContext())
            {
                DefaultAnswer answer = new DefaultAnswer();
                answer.DefaultAnswerId = ans.answerId;
                answer.Answer = ans.answer;
                answer.Percent = ans.percent;
                answer.IsActive = true;
                answer.DefaultQuestionId = ans.questionId;
                context.DefaultAnswer.Update(answer);
                context.SaveChanges();
            }


        }

        public static void removeDefaultAnswer(List<AnswerDefaultDTO> answer)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ans in answer)
                {
                    DefaultAnswer AnswerDb = context.DefaultAnswer.SingleOrDefault(c => c.DefaultAnswerId == ans.answerId);
                    AnswerDb.IsActive = ans.isActive;
                }
                context.SaveChanges();
            }
        }

    }
}
