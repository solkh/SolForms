using SolForms.Data;
using SolForms.Data.DataSourceImp;
using SolForms.Models;
using SolForms.Models.Enums;
using SolForms.Models.Questions;
using System.Data.Common;

namespace SolForms.Services
{    
    public class SolFormsService : ISolFormsService
    {        
        private readonly IFormsDataSource _dataSource;               
        public SolFormsService(IFormsDataSource dataSource)
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
            if(form == null)            
                return false;            
            form.IsActive = active;
            await _dataSource.Update(id, form);
            return true;
        }
        #endregion

        #region Sections
        public async Task<SolFormSection?> GetSection(Guid sectionId) => 
            await _dataSource.Get<SolFormSection>(sectionId);
        public async Task<SolFormSection?[]> GetSections(Guid FormId) => 
            await _dataSource.GetAll<SolFormSection>(FormId) ?? Array.Empty<SolFormSection>();
        public async Task CreateSection(SolFormSection section) => 
            await _dataSource.Create(section);
        public async Task UpdateSection(SolFormSection section) => 
            await _dataSource.Update(section.Id, section);
        public async Task UpdateSection(Guid id, SolFormSection section) => 
            await _dataSource.Update(id, section);
        public async Task<bool> DeleteSection(Guid sectionId) =>
            await _dataSource.Delete<SolFormSection>(sectionId);        
        #endregion

        #region Questions
        public async Task<BaseQuestion?> GetQuestion(Guid questionId)
            => await _dataSource.Get<BaseQuestion>(questionId);
        public async Task<BaseQuestion?[]> GetQuestions(Guid sectionId) => 
            await _dataSource.GetAll<BaseQuestion>(sectionId) ?? Array.Empty<BaseQuestion>();
        public async Task CreateQuestion(BaseQuestion question) => 
            await _dataSource.Create(question);
        public async Task UpdateQuestion(BaseQuestion question) => 
            await _dataSource.Update(question.Id, question);
        public async Task UpdateQuestion(Guid id, BaseQuestion question) => 
            await _dataSource.Update(id, question);
        public async Task<bool> DeleteQuestion(Guid questionId) =>
            await _dataSource.Delete<BaseQuestion>(questionId);        
        #endregion

        #region Conditions
        public async Task<ShowCondition?> GetCondtion(Guid conditionId) =>
            await _dataSource.Get<ShowCondition>(conditionId);
        public async Task CreateCondition(ShowCondition condition) => 
            await _dataSource.Create(condition);
        public async Task UpdateCondtion(ShowCondition condition) => 
            await _dataSource.Update(condition.Id, condition);
        public async Task UpdateCondtion(Guid id, ShowCondition condition) => 
            await _dataSource.Update(id, condition);
        public async Task<bool> DeleteCondtion(Guid conditionId) => 
            await _dataSource.Delete<ShowCondition>(conditionId);        
        #endregion

        #region Options
        public async Task<Option?> GetOption(Guid OptionId) => 
            await _dataSource.Get<Option>(OptionId);
        public async Task<Option?[]> GetOptions(Guid questionId) =>
            await _dataSource.GetAll<Option>() ?? Array.Empty<Option>();
        public async Task CreateOption(Option option) => 
            await _dataSource.Create(option);
        public async Task UpdateOption(Option data) => 
            await _dataSource.Update(data.Id, data);
        public async Task UpdateOption(Guid id, Option data) => 
            await _dataSource.Update(id, data);
        public async Task<bool> DeleteOption(Guid OptionId) =>
            await _dataSource.Delete<Option>(OptionId);
        public async Task<bool> IsRedFlag(Guid optionId) => 
            await _dataSource.IsRedFlag<Option>(optionId);
        #endregion

        #region Submittions
        public async Task<AnsweringSession?> GetSubmission(Guid sessionId) => 
            await _dataSource.Get<AnsweringSession>(sessionId);
        public async Task<AnsweringSession?[]> GetSubmissions(Guid formId) =>
            await _dataSource.GetAll<AnsweringSession>(formId) ?? Array.Empty<AnsweringSession>();
        public async Task<int> CountSubmittions(Guid formID) =>
            await _dataSource.Count<AnsweringSession>();
        public async Task SubmitForm(AnsweringSession answeringSession) => 
            await _dataSource.Create(answeringSession);
        public async Task UpdateSubmission(AnsweringSession answeringSession) => 
            await _dataSource.Update(answeringSession.Id ?? Guid.Empty, answeringSession);
        public async Task UpdateSubmission(Guid id, AnsweringSession answeringSession) => 
            await _dataSource.Update(id, answeringSession);
        public async Task<bool> DeleteSubmittion(Guid sessionId) => 
            await _dataSource.Delete<AnsweringSession>(sessionId);
        #endregion

        #region Answers
        public async Task<Answer?> GetAnswer(Guid sessionId) => 
            await _dataSource.Get<Answer>(sessionId);
        public async Task<Answer?[]> GetAnswers(Guid sesstionId) => 
            await _dataSource.GetAll<Answer>(sesstionId) ?? Array.Empty<Answer>();
        public async Task CreateAnswer(Answer answer) => 
            await _dataSource.Create(answer);
        public async Task CreateAnswers(Answer[] answers)
        {
            foreach (var Answer in answers)
                await _dataSource.Create(Answer);            
        }
        public async Task UpdateAnswer(Answer answer) => 
            await _dataSource.Update(answer.Id.Value, answer);
        public async Task UpdateAnswer(Guid id, Answer answer) => 
            await _dataSource.Update(id, answer);
        public async Task<bool> DeleteAnswer(Guid answerId) => 
            await _dataSource.Delete<Answer>(answerId);
        #endregion                              
    }
}