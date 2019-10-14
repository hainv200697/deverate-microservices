using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Models
{
    public class AnwserTestDTO
    {
        public int answerId { get; set; }
        public string answer { get; set; }

        public AnwserTestDTO(int answerId, string answer)
        {
            this.answerId = answerId;
            this.answer = answer;
        }
    }
}
