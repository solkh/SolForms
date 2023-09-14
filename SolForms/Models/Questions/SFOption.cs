using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Models.Questions
{
    public class SFOption
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string? Text { get; set; }
        public int? Order { get; set; }
        public bool IsRedFlag{ get; set; }
    }
}
