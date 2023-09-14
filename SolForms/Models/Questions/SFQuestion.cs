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
        public virtual ICollection<SFShowCondition> ShowCondition { get; set; } = new List<SFShowCondition>();
        public virtual ICollection<SFOption>? Options { get; set; }
    }
}
