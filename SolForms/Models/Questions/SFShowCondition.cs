using SolForms.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Models.Questions
{
    public record SFShowCondition
    {
        public Guid Id { get; set; }
        public Guid ParentQuestionId { get; set; }
        public Guid? QuestionId { get; set; }
        public ConditionType Type { get; set; }
        public Guid? OptionId { get; set; }
    }
}
