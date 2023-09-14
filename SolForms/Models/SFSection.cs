using SolForms.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Models
{
    public record SFSection
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public string FormSectionTitle { get; set; }
        public int? Order { get; set; }
        public virtual ICollection<SFQuestion>? Questions { get; set; } = new List<SFQuestion>();
    }
}
