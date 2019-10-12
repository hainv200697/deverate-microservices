using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class TestAnswerDTO
    {
        public List<AnswerDTO> answers { get; set; }
        public int? testId { get; set; }

        public TestAnswerDTO() { }

        public TestAnswerDTO(List<AnswerDTO> answers, int? testId)
        {
            this.answers = answers;
            this.testId = testId;
        }
    }
}
