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
        public static List<SFSection> AddSections(this SolForm form, params string[] titles)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            if (form.FormSections == null) form.FormSections = new List<SFSection>();

            var order = form.FormSections.Count();
            foreach (var title in titles)
            {
                var sec = new SFSection
                {
                    Id = Guid.NewGuid(),
                    FormSectionTitle = title,
                    Order = order++,
                    Questions = new List<SFQuestion>()
                };
                form.FormSections.Add(sec);
            }
            return form.FormSections;
        }


        // Questions
        public static SFSection? AddQuestion(this SFSection section, QuestionType type, string text, SFShowCondition? cond = null)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var order = section?.Questions?.Count;

            var q = new SFQuestion
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
        public static SFSection? AddSingleChoiceQuestion(this SFSection section, string text, SFShowCondition cond, params string[] options)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            var order = section?.Questions?.Count;
            var question = new SFQuestion
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
        public static SFQuestion AddSingleChoiceQuestion(this SFSection section, string text, params string[] options)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            var order = section?.Questions?.Count;
            var question = new SFQuestion
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
        public static SFSection? AddMultipleChoiceQuestion(this SFSection section, string text, SFShowCondition cond, params string[] options)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var order = section?.Questions?.Count;
            var question = new SFQuestion
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
        public static SFSection? AddMultipleChoiceQuestion(this SFSection section, string text, params string[] options)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var order = section?.Questions?.Count;
            var question = new SFQuestion
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
        public static SFSection? AddFreeTextQuestion(this SFSection section, string text, SFShowCondition cond)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var order = section?.Questions?.Count;
            var question = new SFQuestion
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
        public static SFSection? AddImageFileUploadQuestion(this SFSection section, string text, SFShowCondition cond)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));

            var order = section?.Questions?.Count;
            var question = new SFQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = text,
                Order = order,
                Type = QuestionType.ImageFileUpload,
                ShowCondition = cond,
            };
            //TODO: Add Photo SFOption
            section?.Questions?.Add(question);

            return section;
        }

        // Options
        public static List<SFOption> AddOptions(this SFQuestion question, params string[] texts)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            if (question.Options == null) question.Options = new List<SFOption>();
            var order = question.Options.Count();
            foreach (var text in texts)
            {
                var op = new SFOption
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
        public static SFQuestion AddShowCondition(this SolForm form, Guid basequestionId, ConditionType type, Guid questionId, Guid optionId)
        {
            var question = form.GetQuestionById(basequestionId);
            return AddShowCondition(question, type, questionId, optionId);
        }
        public static SFQuestion AddShowCondition(this SFSection section, Guid basequestionId, ConditionType type, Guid questionId, Guid optionId)
        {
            var question = section.GetQuestionById(basequestionId);
            return question.AddShowCondition(type, questionId, optionId);
        }
        public static SFQuestion AddShowCondition(this SFQuestion question, ConditionType type, Guid questionId, Guid optionId)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));

            question.ShowCondition = new SFShowCondition
            {
                QuestionId = questionId,
                Type = type,
                OptionId = optionId
            };
            return question;
        }

        //Answers
        public static SFAnswer AnswerQuestion(this SFQuestion question, params Guid[] selectedOptions)
        {
            var answer = new SFAnswer()
            {
                QuestionId = question.Id,
                Value = selectedOptions[0].ToString(),
            };

            answer.Value = string.Join(",", selectedOptions);
            return answer;
        }
        public static SFAnswer AnswerQuestion(this SFSection section, Guid questionId, params Guid[] selectedOptions) =>
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
        public static SFSection CreateSection(string title)
        {
            return new SFSection
            {
                Id = Guid.NewGuid(),
                FormSectionTitle = title,
            };
        }
        public static SFQuestion CreateQuestion(string questionText, QuestionType type)
        {
            return new SFQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = questionText,
                Type = type,
            };
        }
        public static SFOption CreateOption(string optionText)
        {
            return new SFOption
            {
                Id = Guid.NewGuid(),
                Text = optionText,
            };
        }
        public static SFShowCondition CreateCondition(ConditionType type)
        {
            return new SFShowCondition
            {
                Id = Guid.NewGuid(),
                Type = type,
            };
        }
        public static SolForm AddSections(this SolForm form, params SFSection[] sections)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            if (form.FormSections == null) form.FormSections = new List<SFSection>();

            var order = form.FormSections.Count();
            foreach (var section in sections)
            {
                section.Order = order++;
                section.FormId = form.Id;
                form.FormSections.Add(section);
            }
            return form;
        }
        public static SFSection? AddQuestions(this SFSection section, params SFQuestion[] questions)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            if (section.Questions == null) section.Questions = new List<SFQuestion>();
            var sectionId = section.Id;
            var order = section?.Questions?.Count;
            foreach (var question in questions)
            {

                question.Order = order++;
                question.SectionId = sectionId;
                section?.Questions?.Add(question);
            }
            return section;
        }
        public static SFQuestion AddOptions(this SFQuestion question, params SFOption[] options)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            if (question.Options == null) question.Options = new List<SFOption>();

            var order = question.Options.Count();
            foreach (var option in options)
            {
                option.Order = order++;
                option.QuestionId = question.Id;
                question.Options.Add(option);
            }
            return question;
        }
        //public static SFQuestion AddConditions(this SFQuestion question, params SFShowCondition[] conditions)
        //{
        //    if (question == null) throw new ArgumentNullException(nameof(question));
        //    question.SFShowCondition.QuestionId = question.Id;
        //    question.SFShowCondition.
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
        //public static SFSubmition CreateAnswer(SFSubmition answeringSession, params SFOption[] selectedOptions)
        //{
        //    if (selectedOptions is null) throw new ArgumentNullException(nameof(selectedOptions));            
        //    foreach (var option in selectedOptions)
        //    {
        //        var ans = CreateAnswer(option);
        //        answeringSession.Answers.Add(ans);
        //    }            
        //    return answeringSession;
        //}
        //public static SFAnswer CreateAnswer(SFOption selectedOption)
        //{
        //    return new SFAnswer()
        //    {
        //        Id = Guid.NewGuid(),
        //        QuestionId = selectedOption.QuestionId,
        //        Value = selectedOption.Text,                
        //    };
        //}
        public static SFSubmition CreateAnsweringSession(string userName, string userEmail, string userPhone)
        {
            return new SFSubmition()
            {
                Id = Guid.NewGuid(),
                UserEmail = userEmail,
                UserPhone = userPhone,
                UserName = userName,
                Answers = new List<SFAnswer>()
            };
        }
        public static SFAnswer CreateAnswer(this SFQuestion question, params SFOption[] options)
        {
            var answer = new SFAnswer()
            {
                Id = Guid.NewGuid(),
                QuestionId = question.Id,
            };
            foreach (var option in options)
            {
                answer.Value += option.Text + ",";
            }
            if (answer.Value.EndsWith(","))
            {
                answer.Value = answer.Value.Substring(0, answer.Value.Length - 1);
            }
            return answer;
        }
        public static SFSubmition AddAnswer(this SFSubmition answeringSession, params SFAnswer[] answers)
        {
            if (answeringSession == null) throw new ArgumentNullException(nameof(answeringSession));
            if (answeringSession.Answers == null) answeringSession.Answers = new List<SFAnswer>();
            foreach (var answer in answers)
            {
                answer.SubmissionId = answeringSession.Id;
                answeringSession.Answers.Add(answer);
            }
            return answeringSession;
        }
        public static SFSubmition AddAnsweringSession(this SolForm form, SFSubmition answeringSession)
        {
            answeringSession.FormId = form.Id;
            return answeringSession;
        }
    }
}
