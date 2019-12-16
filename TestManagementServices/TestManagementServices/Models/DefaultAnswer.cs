using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class DefaultAnswer
    {
        public int DefaultAnswerId { get; set; }
        public int DefaultQuestionId { get; set; }
        public string Answer { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }

        public virtual DefaultQuestion DefaultQuestion { get; set; }
    }
}
