using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Models
{
    public class QuestionInTestDTO
    {
        public int? qitid { get; set; }
        public int? testId { get; set; }
        public List<AnwserTestDTO> answers { get; set; } = new List<AnwserTestDTO>();
        public int? answerId { get; set; }
        public string question { get; set; }

        public QuestionInTestDTO(int? qitid, int? testId, List<Answer> answers, int? answerId, string question)
        {
            this.qitid = qitid;
            this.testId = testId;
            this.answerId = answerId;
            this.question = question;
            foreach(Answer item in answers)
            {
                this.answers.Add(new AnwserTestDTO(item.AnswerId, item.Answer1));
            } 
        }
    }
}
