using SolForms.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Models
{
    public class SFSubmition
    {
        public Guid? Id { get; set; }
        public Guid FromId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public virtual List<SFAnswer>? Answers { get; set; } = new List<SFAnswer>();
    }
}
