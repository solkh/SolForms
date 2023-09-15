﻿using SolForms.Models.Enums;
using System.Text;
using System.Text.Json.Serialization;

namespace SolForms.Models.Questions
{
    public record SFAnswer
    {
        [JsonIgnore]
        public Guid? Id { get; set; }
        public Guid? SubmissionId { get; set; }
        public Guid QuestionId { get; set; }        
        public string Value { get; set; } = "";
        public QuestionType Type { get; set; }
        public string[] Values =>
            Type != QuestionType.MultipleChoice && Type != QuestionType.SingleChoice ?
            new string[] { Value } :
            Value.Split(",", StringSplitOptions.RemoveEmptyEntries);
    }

    public record SFAnswerDto
    {
        public Guid? QuestionId { get; set; }
        public string Value { get; set; } = "";
    }
}
