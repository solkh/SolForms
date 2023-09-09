using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Models.Questions
{
    public interface IOption
    {
        Guid Id { get; set; }
        Guid QuestionId { get; set; }
        int? Order { get; set; }
        string? Text { get; set; }
    }
    public class Option : IOption
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string? Text { get; set; }
        public int? Order { get; set; }
    }
}
