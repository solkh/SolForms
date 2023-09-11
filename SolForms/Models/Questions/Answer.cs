using SolForms.Models.Enums;
using System.Text;

namespace SolForms.Models.Questions
{
    public record Answer
    {
        public Guid Id { get; set; }        
        public Guid SessionId { get; set; }        
        public Guid QuestionId { get; set; }        
        public string Value { get; set; } = "";

        QuestionType Type { get; set; }
        public string[] Values =>
            Type != QuestionType.MultipleChoice && Type != QuestionType.SingleChoice ?
            new string[] { Value } :
            Value.Split(",", StringSplitOptions.RemoveEmptyEntries);
    }
}
