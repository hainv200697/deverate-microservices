using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class QuestionDTO
    {
        public int? questionId { get; set; }
        public string question { get; set; }
        public List<AnswerDTO> answers { get; set; }
        public QuestionDTO() { }
        public QuestionDTO(int? questionId, List<AnswerDTO> answers)
        {
            this.questionId = questionId;
            this.answers = answers;
        }

        public QuestionDTO(int? questionId, string question, List<AnswerDTO> answers)
        {
            this.questionId = questionId;
            this.answers = answers;
            this.question = question;
        }
    }
}
