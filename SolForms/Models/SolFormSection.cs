using SolForms.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Models
{
    public interface IFormSection
    {
        Guid Id { get; set; }
        Guid FormId { get; set; }
        string FormSectionTitle { get; set; }
        int? Order { get; set; }
        List<BaseQuestion>? Questions { get; set; }

    }
    public record SolFormSection : IFormSection
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public string FormSectionTitle { get; set; }
        public int? Order { get; set; }
        public List<BaseQuestion>? Questions { get; set; } = new List<BaseQuestion>();

    }
}
