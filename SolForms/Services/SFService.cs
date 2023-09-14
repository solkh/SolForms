using SolForms.Data;
using SolForms.Data.DataSourceImp;
using SolForms.Models;
using SolForms.Models.Enums;
using SolForms.Models.Questions;
using System.Data.Common;

namespace SolForms.Services
{
    public class SFService : ISFService
    {
        private readonly IFormsDataSource _dataSource;
        public SFService(IFormsDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        #region Forms
        public async Task<SolForm?> GetForm(Guid formId) =>
            await _dataSource.Get<SolForm>(formId);
        public async Task<SolForm?[]> GetForms() =>
            await _dataSource.GetAll<SolForm>()
                ?? Array.Empty<SolForm>();

        public async Task<int> CountForms() =>
            await _dataSource.Count<SolForm>();
        public async Task CreateForm(SolForm form) =>
            await _dataSource.Create(form);
        public async Task<bool> UpdateForm(SolForm form)
        {
            if (form == null) return false;

            var oldForm = await _dataSource.Get<SolForm>(form.Id);
            if (oldForm == null)
            {
                await CreateForm(form);
                return true;
            }
            await _dataSource.Update(form.Id, form);
            return true;
        }
        public async Task<bool> UpdateForm(Guid id, SolForm form)
        {
            if (form == null) return false;

            var oldForm = await GetForm(id);
            if (oldForm == null)
            {
                await CreateForm(form);
                return true;
            }
            await _dataSource.Update(form.Id, form);
            return true;
        }
        public async Task<bool> DeleteForm(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString())) return false;
            var oldForm = _dataSource.Get<SolForm>(id);
            if (oldForm != null)
                return await _dataSource.Delete<SolForm>(id);
            return false;
        }
        public async Task<bool> SetActive(Guid id, bool active)
        {
            var form = await _dataSource.Get<SolForm>(id);
            if (form == null)
                return false;
            form.IsActive = active;
            await _dataSource.Update(id, form);
            return true;
        }
        #endregion

        #region Sections
        public async Task<SFSection?> GetSection(Guid sectionId) =>
            await _dataSource.Get<SFSection>(sectionId);
        public async Task<SFSection?[]> GetSections(Guid FormId) =>
            await _dataSource.GetAll<SFSection>(FormId) ?? Array.Empty<SFSection>();
        public async Task CreateSection(SFSection section) =>
            await _dataSource.Create(section);
        public async Task UpdateSection(SFSection section) =>
            await _dataSource.Update(section.Id, section);
        public async Task UpdateSection(Guid id, SFSection section) =>
            await _dataSource.Update(id, section);
        public async Task<bool> DeleteSection(Guid sectionId) =>
            await _dataSource.Delete<SFSection>(sectionId);
        #endregion

        #region Questions
        public async Task<SFQuestion?> GetQuestion(Guid questionId)
            => await _dataSource.Get<SFQuestion>(questionId);
        public async Task<SFQuestion?[]> GetQuestions(Guid sectionId) =>
            await _dataSource.GetAll<SFQuestion>(sectionId) ?? Array.Empty<SFQuestion>();
        public async Task CreateQuestion(SFQuestion question) =>
            await _dataSource.Create(question);
        public async Task UpdateQuestion(SFQuestion question) =>
            await _dataSource.Update(question.Id, question);
        public async Task UpdateQuestion(Guid id, SFQuestion question) =>
            await _dataSource.Update(id, question);
        public async Task<bool> DeleteQuestion(Guid questionId) =>
            await _dataSource.Delete<SFQuestion>(questionId);

        public async Task<QuestionType> GetQuestionTypeById(Guid questionId)    => 
            (await _dataSource.Get<SFQuestion>(questionId))?.Type ?? QuestionType.FreeText;
        #endregion

        #region Conditions
        public async Task<SFShowCondition?> GetCondtion(Guid conditionId) =>
            await _dataSource.Get<SFShowCondition>(conditionId);
        public async Task CreateCondition(SFShowCondition condition) =>
            await _dataSource.Create(condition);
        public async Task UpdateCondtion(SFShowCondition condition) =>
            await _dataSource.Update(condition.Id, condition);
        public async Task UpdateCondtion(Guid id, SFShowCondition condition) =>
            await _dataSource.Update(id, condition);
        public async Task<bool> DeleteCondtion(Guid conditionId) =>
            await _dataSource.Delete<SFShowCondition>(conditionId);
        #endregion

        #region Options
        public async Task<SFOption?> GetOption(Guid OptionId) =>
            await _dataSource.Get<SFOption>(OptionId);
        public async Task<SFOption?[]> GetOptions(Guid questionId) =>
            await _dataSource.GetAll<SFOption>() ?? Array.Empty<SFOption>();
        public async Task CreateOption(SFOption option) =>
            await _dataSource.Create(option);
        public async Task UpdateOption(SFOption data) =>
            await _dataSource.Update(data.Id, data);
        public async Task UpdateOption(Guid id, SFOption data) =>
            await _dataSource.Update(id, data);
        public async Task<bool> DeleteOption(Guid OptionId) =>
            await _dataSource.Delete<SFOption>(OptionId);
        #endregion

        #region Submissions
        public virtual async Task<SFSubmission?> GetSubmission(Guid sessionId) =>
            await _dataSource.Get<SFSubmission>(sessionId);
        public virtual async Task<SFSubmission?[]> GetSubmissions(Guid formId) =>
            await _dataSource.GetAll<SFSubmission>(formId) ?? Array.Empty<SFSubmission>();
        public virtual async Task<int> CountSubmissions(Guid formID) =>
            await _dataSource.Count<SFSubmission>();
        public virtual async Task SubmitForm(SFSubmission answeringSession) =>
            await _dataSource.Create(answeringSession);
        public virtual async Task UpdateSubmission(SFSubmission answeringSession) =>
            await _dataSource.Update(answeringSession.Id ?? Guid.Empty, answeringSession);
        public virtual async Task UpdateSubmission(Guid id, SFSubmission answeringSession) =>
            await _dataSource.Update(id, answeringSession);
        public virtual async Task<bool> DeleteSubmission(Guid sessionId) =>
            await _dataSource.Delete<SFSubmission>(sessionId);
        #endregion

        #region Answers
        public async Task<SFAnswer?> GetAnswer(Guid sessionId) =>
            await _dataSource.Get<SFAnswer>(sessionId);
        public async Task<SFAnswer?[]> GetAnswers(Guid sesstionId) =>
            await _dataSource.GetAll<SFAnswer>(sesstionId) ?? Array.Empty<SFAnswer>();
        public async Task CreateAnswer(SFAnswer answer) =>
            await _dataSource.Create(answer);
        public async Task CreateAnswers(SFAnswer[] answers)
        {
            foreach (var Answer in answers)
                await _dataSource.Create(Answer);
        }
        public async Task UpdateAnswer(SFAnswer answer) =>
            await _dataSource.Update(answer.Id.Value, answer);
        public async Task UpdateAnswer(Guid id, SFAnswer answer) =>
            await _dataSource.Update(id, answer);
        public async Task<bool> DeleteAnswer(Guid answerId) =>
            await _dataSource.Delete<SFAnswer>(answerId);
        #endregion
    }
}