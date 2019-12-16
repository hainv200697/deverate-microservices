using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    public class QuestionDTO
    {
        public int questionId { get; set; }
        public string question { get; set; }
        public List<AnswerDTO> answers { get; set; }
        public int catalogueId { get; set; }
        public QuestionDTO() { }
        public QuestionDTO(int questionId, List<AnswerDTO> answers)
        {
            this.questionId = questionId;
            this.answers = answers;
        }

        public QuestionDTO(int questionId, string question, List<AnswerDTO> answers, int identify = 0)
        {
            this.questionId = questionId;
            this.answers = answers;
            this.question = question;
        }

        public QuestionDTO(int questionId, string question, List<Answer> answers)
        {
            this.questionId = questionId;
            this.answers = answers.Select(a => new AnswerDTO(a)).ToList();
            this.question = question;
        }
        public QuestionDTO(int questionId, string question, int companyCatalogueId, List<AnswerDTO> answers)
        {
            this.questionId = questionId;
            this.answers = answers;
            this.catalogueId = companyCatalogueId;
            this.question = question;
        }
    }
}
