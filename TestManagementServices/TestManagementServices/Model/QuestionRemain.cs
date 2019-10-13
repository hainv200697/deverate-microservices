using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class QuestionRemain
    {
        public int? catalogueId { get; set; }
        public int? curNumbQues { get; set; }
        public int? numbCataQues { get; set; }
        public List<QuestionDTO> unchoosedQues { get; set; }
    }
}
