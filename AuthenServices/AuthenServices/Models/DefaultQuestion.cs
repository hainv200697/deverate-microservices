using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class DefaultQuestion
    {
        public DefaultQuestion()
        {
            DefaultAnswer = new HashSet<DefaultAnswer>();
        }

        public int DefaultQuestionId { get; set; }
        public int DefaultCatalogueId { get; set; }
        public string Question { get; set; }
        public double Point { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public virtual DefaultCatalogue DefaultCatalogue { get; set; }
        public virtual ICollection<DefaultAnswer> DefaultAnswer { get; set; }
    }
}
