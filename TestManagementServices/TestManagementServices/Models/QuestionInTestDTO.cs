using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Models
{
    public class QuestionInTestDTO
    {
        public int testId { get; set; }
        public int questionId { get; set; }
        public List<AnwserTestDTO> answers { get; set; } = new List<AnwserTestDTO>();
        public int? answerId { get; set; }
        public string question { get; set; }

        public QuestionInTestDTO(int testId, int questionId, int? answerId, List<Answer> answers, string question)
        {
            this.testId = testId;
            this.questionId = questionId;
            this.answerId = answerId;
            this.question = question;
            if(answers != null)
            {
                foreach (Answer item in answers)
                {
                    this.answers.Add(new AnwserTestDTO(item.AnswerId, item.AnswerText));
                }
            }
            
        }
    }
}
