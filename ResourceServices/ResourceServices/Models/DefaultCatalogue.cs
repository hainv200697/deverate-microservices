using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class DefaultCatalogue
    {
        public DefaultCatalogue()
        {
            DefaultQuestion = new HashSet<DefaultQuestion>();
        }

        public int DefaultCatalogueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<DefaultQuestion> DefaultQuestion { get; set; }
    }
}
