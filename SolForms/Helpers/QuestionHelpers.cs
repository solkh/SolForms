using SolForms.Models.Questions;
using SolForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Helpers
{
    public static class QuestionHelpers
    {

        public static SFQuestion GetQuestionById(this SolForm form, Guid basequestionId)
        {
            foreach(var section in form.FormSections)
            {
                var question = section.Questions.FirstOrDefault(x=>x.Id == basequestionId);
                if(question != null)
                {
                    return question;
                }
            }
            return new SFQuestion();            
        }

        public static SFQuestion GetQuestionById(this SFSection section, Guid basequestionId)
        {
            return section.Questions.FirstOrDefault(x => x.Id == basequestionId);
        }
        public static SFQuestion GetQuestionAt(this SolForm form, int index)
        {
            return default;
        }

        public static SFQuestion GetQuestionAt(this SFSection form, int index)
        {
            return default;
        }
    }
}
