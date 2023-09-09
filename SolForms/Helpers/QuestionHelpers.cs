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

        public static IBaseQuestion GetQuestionById(this ISolForm form, Guid basequestionId)
        {
            foreach(var section in form.FormSections)
            {
                var question = section.Questions.FirstOrDefault(x=>x.Id == basequestionId);
                if(question != null)
                {
                    return question;
                }
            }
            return new BaseQuestion();            
        }

        public static IBaseQuestion GetQuestionById(this IFormSection section, Guid basequestionId)
        {
            return section.Questions.FirstOrDefault(x => x.Id == basequestionId);
        }
        public static IBaseQuestion GetQuestionAt(this ISolForm form, int index)
        {
            return default;
        }

        public static IBaseQuestion GetQuestionAt(this IFormSection form, int index)
        {
            return default;
        }
    }
}
