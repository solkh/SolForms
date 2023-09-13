using SolForms.Models.Enums;
using SolForms.Models.Questions;
using SolForms.Models;
using SolForms.Services;
using System.Linq.Expressions;
using SolForms.Options;
using SolForms.Helpers;
using static System.Collections.Specialized.BitVector32;

namespace SolForms.Extentions
{
    public static class SolFormsFactory
    {
        // Forms
        

        public static Guid DeleteForm(this SolForm form)
        {
            return form.Id;
        }

        // Sections
        public static List<SolFormSection> AddSections(this SolForm form, params string[] titles)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            if (form.FormSections == null) form.FormSections = new List<SolFormSection>();

            var order = form.FormSections.Count();
            foreach (var title in titles)
            {
                var sec = new SolFormSection
                {
                    Id = Guid.NewGuid(),
                    FormSectionTitle = title,
                    Order = order++,
                    Questions = new List<BaseQuestion>()
                };
                form.FormSections.Add(sec);
            }
            return form.FormSections;
        }


        // Questions
        public static IFormSection? AddQuestion(this IFormSection section, QuestionType type, string text, ShowCondition? cond = null)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var order = section?.Questions?.Count;

            var q = new BaseQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = text,
                Order = order,
                Type = type
            };
            q.ShowCondition = cond ?? q.ShowCondition;
            section?.Questions?.Add(q);
            return section;
        }
        public static IFormSection? AddQuestions(this IFormSection section, params BaseQuestion[] questions)
        {
            foreach (var q in questions)
            {
                section?.Questions?.Add(q);
            }
            return section;
        }
        public static IFormSection? AddSingleChoiceQuestion(this IFormSection section, string text, ShowCondition cond, params string[] options)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            var order = section?.Questions?.Count;
            var question = new BaseQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = text,
                Order = order,
                Type = QuestionType.SingleChoice,
                ShowCondition = cond,
            };
            question.AddOptions(options);
            section?.Questions?.Add(question);

            return section;
        }
        public static BaseQuestion AddSingleChoiceQuestion(this IFormSection section, string text, params string[] options)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            var order = section?.Questions?.Count;
            var question = new BaseQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = text,
                Order = order,
                Type = QuestionType.SingleChoice,
            };
            question.AddOptions(options);
            section?.Questions?.Add(question);

            return question;
        }
        public static IFormSection? AddMultipleChoiceQuestion(this IFormSection section, string text, ShowCondition cond, params string[] options)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var order = section?.Questions?.Count;
            var question = new BaseQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = text,
                Order = order,
                Type = QuestionType.MultipleChoice,
                ShowCondition = cond,
            };

            question.AddOptions(options);
            section?.Questions?.Add(question);
            return section;
        }
        public static IFormSection? AddMultipleChoiceQuestion(this IFormSection section, string text, params string[] options)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var order = section?.Questions?.Count;
            var question = new BaseQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = text,
                Order = order,
                Type = QuestionType.MultipleChoice,
            };

            question.AddOptions(options);
            section?.Questions?.Add(question);
            return section;
        }
        public static IFormSection? AddFreeTextQuestion(this IFormSection section, string text, ShowCondition cond)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var order = section?.Questions?.Count;
            var question = new BaseQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = text,
                Order = order,
                Type = QuestionType.FreeText,
                ShowCondition = cond,
            };

            //TODO: Add Text Area

            section?.Questions?.Add(question);

            return section;
        }
        public static IFormSection? AddImageFileUploadQuestion(this IFormSection section, string text, ShowCondition cond)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var order = section?.Questions?.Count;
            var question = new BaseQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = text,
                Order = order,
                Type = QuestionType.ImageFileUpload,
                ShowCondition = cond,
            };
            //TODO: Add Photo Option
            section?.Questions?.Add(question);

            return section;
        }

        // Options
        public static List<Option> AddOptions(this IBaseQuestion question, params string[] texts)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            if (question.Options == null) question.Options = new List<Option>();
            var order = question.Options.Count();
            foreach (var text in texts)
            {
                var op = new Option
                {
                    Id = Guid.NewGuid(),
                    Text = text,
                    Order = order++
                };
                question.Options.Add(op);
            }
            return question.Options;
        }

        // Conditions
        public static IBaseQuestion AddShowCondition(this ISolForm form, Guid basequestionId, ConditionType type, Guid questionId, Guid optionId)
        {
            var question = form.GetQuestionById(basequestionId);
            return AddShowCondition(question, type, questionId, optionId);
        }
        public static IBaseQuestion AddShowCondition(this IFormSection section, Guid basequestionId, ConditionType type, Guid questionId, Guid optionId)
        {
            var question = section.GetQuestionById(basequestionId);
            return question.AddShowCondition(type, questionId, optionId);
        }
        public static IBaseQuestion AddShowCondition(this IBaseQuestion question, ConditionType type, Guid questionId, Guid optionId)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));

            question.ShowCondition = new ShowCondition
            {
                QuestionId = questionId,
                Type = type,
                OptionId = optionId
            };
            return question;
        }

        //Answers
        public static Answer AnswerQuestion(this IBaseQuestion question, params Guid[] selectedOptions)
        {
            var answer = new Answer()
            {
                QuestionId = question.Id,
                Value = selectedOptions[0].ToString(),
            };

            answer.Value = string.Join(",", selectedOptions);
            return answer;
        }
        public static Answer AnswerQuestion(this IFormSection section, Guid questionId, params Guid[] selectedOptions) =>
            section.GetQuestionById(questionId).AnswerQuestion(selectedOptions);
        public static string SkipCommas(this string input, string seperator = ",") =>
            input.Replace(seperator, seperator + seperator);
        public static string UnSkip(this string input, string seperator = ",") =>
            input.Replace(seperator + seperator, seperator);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static SolForm CreateForm(string title, string description)
        {
            return new SolForm
            {
                Id = Guid.NewGuid(),
                FormTitle = title,
                IsActive = true,
                ShortDescrption = description,
            };
        }
        public static SolFormSection CreateSection(string title)
        {
            return new SolFormSection
            {
                Id = Guid.NewGuid(),
                FormSectionTitle = title,                                                 
            };
        }
        public static BaseQuestion CreateQuestion(string questionText, QuestionType type)
        {
            return new BaseQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = questionText,                
                Type = type,                
            };
        }
        public static Option CreateOption(string optionText)
        {
            return new Option
            {
                Id = Guid.NewGuid(),
                Text = optionText,                
            };
        }
        public static ShowCondition CreateCondition(ConditionType type)
        {
            return new ShowCondition
            {
                Id = Guid.NewGuid(),
                Type = type,                
            };
        }
        public static SolForm AddSections(this SolForm form, params SolFormSection[] sections)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            if (form.FormSections == null) form.FormSections = new List<SolFormSection>();

            var order = form.FormSections.Count();
            foreach (var section in sections)
            {
                section.Order = order++;
                section.FormId = form.Id;                
                form.FormSections.Add(section);
            }
            return form;
        }
        public static SolFormSection? AddQuestions(this SolFormSection section, params BaseQuestion[] questions)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            if(section.Questions == null) section.Questions = new List<BaseQuestion>();
            var sectionId = section.Id;
            var order = section?.Questions?.Count;
            foreach(var question in questions)
            {
                
                question.Order = order++;
                question.SectionId = sectionId;
                section?.Questions?.Add(question);
            }
            return section;
        }
        public static BaseQuestion AddOptions(this BaseQuestion question, params Option[] options)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            if (question.Options == null) question.Options = new List<Option>();

            var order = question.Options.Count();
            foreach (var option in options)
            {
                option.Order = order++;
                option.QuestionId = question.Id;                
                question.Options.Add(option);
            }
            return question;
        }
        //public static BaseQuestion AddConditions(this BaseQuestion question, params ShowCondition[] conditions)
        //{
        //    if (question == null) throw new ArgumentNullException(nameof(question));
        //    question.ShowCondition.QuestionId = question.Id;
        //    question.ShowCondition.
        //    
        //    return question;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="userName"></param>
        /// <param name="userEmail"></param>
        /// <param name="userPhone"></param>
        /// <returns></returns>
        /// 
        //public static AnsweringSession CreateAnswer(AnsweringSession answeringSession, params Option[] selectedOptions)
        //{
        //    if (selectedOptions is null) throw new ArgumentNullException(nameof(selectedOptions));            
        //    foreach (var option in selectedOptions)
        //    {
        //        var ans = CreateAnswer(option);
        //        answeringSession.Answers.Add(ans);
        //    }            
        //    return answeringSession;
        //}
        //public static Answer CreateAnswer(Option selectedOption)
        //{
        //    return new Answer()
        //    {
        //        Id = Guid.NewGuid(),
        //        QuestionId = selectedOption.QuestionId,
        //        Value = selectedOption.Text,                
        //    };
        //}
        public static AnsweringSession CreateAnsweringSession(string userName, string userEmail, string userPhone)
        {
            return new AnsweringSession() 
            {                
                Id = Guid.NewGuid(),
                UserEmail = userEmail, 
                UserPhone = userPhone,
                UserName = userName,
                Answers = new List<Answer>()
            };
        }
        public static Answer CreateAnswer(this BaseQuestion question, params Option[] options)
        {
            var answer = new Answer()
            {
                Id = Guid.NewGuid(),
                QuestionId = question.Id,
            };
            foreach(var option in options)
            {
                answer.Value += option.Text+",";
            }
            if (answer.Value.EndsWith(","))
            {
                answer.Value = answer.Value.Substring(0, answer.Value.Length - 1);
            }            
            return answer;
        }
        public static AnsweringSession AddAnswer(this AnsweringSession answeringSession, params Answer[] answers)
        {
            if (answeringSession == null) throw new ArgumentNullException(nameof(answeringSession));
            if (answeringSession.Answers == null) answeringSession.Answers = new List<Answer>();
            foreach(var answer in answers)
            {
                answer.SubmissionId = answeringSession.Id;
                answeringSession.Answers.Add(answer);
            }
            return answeringSession;
        }
        public static AnsweringSession AddAnsweringSession(this SolForm form, AnsweringSession answeringSession)
        {
            answeringSession.FormId = form.Id;
            return answeringSession;
        }
    }
}
