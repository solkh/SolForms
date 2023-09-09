using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Options
{
    public class SolFormOptions
    {
        public string Forms { get; set; } = "Forms";
        public string Submissions { get; set; } = "FormSubmissions";
        public Type? DbContextType { get; set; }
    }
}
