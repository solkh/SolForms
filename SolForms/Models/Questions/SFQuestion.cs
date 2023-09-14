using SolForms.Models.Enums;

namespace SolForms.Models.Questions
{
    public record SFQuestion
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public int? Order { get; set; }
        public string? QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public virtual SFShowCondition ShowCondition { get; set; } = new SFShowCondition();
        public virtual List<SFOption>? Options { get; set; }
    }
}
