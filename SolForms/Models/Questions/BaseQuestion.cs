using SolForms.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Models.Questions
{
    public interface IBaseQuestion
    {
        Guid Id { get; set; }
        Guid SectionId { get; set; }
        int? Order { get; set; }
        string? QuestionText { get; set; }
        QuestionType Type { get; set; }
        ShowCondition ShowCondition { get; set; }
        List<Option>? Options { get; set; }
    }
    public record BaseQuestion : IBaseQuestion
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public int? Order { get; set; }
        public string? QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public ShowCondition ShowCondition { get; set; } = new ShowCondition();
        public List<Option>? Options { get; set; }
    }
}
