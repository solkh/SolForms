using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MongoDB.Driver;
using SolForms.Models;
using SolForms.Models.Questions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Collections.Specialized.BitVector32;

namespace SolForms.Data.DataSourceImp
{
    public class KeyTypeValueDataSource : IFormsDataSource
    {
        private readonly DbContext _context;
        private readonly DbSet<FormsKeyTypeValue> _dbSet;

        public KeyTypeValueDataSource(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<FormsKeyTypeValue>();
        }
        public async Task<TEntity?> Get<TEntity>(Guid id) where TEntity : class
        {
            var entityType = GetEntityType<TEntity>();
            return entityType switch
            {
                EntityType.SolForm => await GetFormAsync(id) as TEntity,
                EntityType.SolFormSection => await GetSectionAsync(id) as TEntity,
                EntityType.BaseQuestion => await GetQuestionAsync(id) as TEntity,
                EntityType.Option => await GetOptionAsync(id) as TEntity,
                EntityType.ShowCondition => await GetConditionAsync(id) as TEntity,
                EntityType.AnsweringSession => await GetAnsweringSessionAsync(id) as TEntity,
                EntityType.Answer => await GetAnswerAsync(id) as TEntity,
                _ => default,
            };
        }
        public async Task<TEntity?[]?> GetAll<TEntity>(Guid? id = null) where TEntity : class
        {
            var entityType = GetEntityType<TEntity>();
            return entityType switch
            {
                EntityType.SolForm => await GetAllFormsAsync() as TEntity[],
                EntityType.SolFormSection => await GetAllSectionsAsync(id) as TEntity[],
                EntityType.BaseQuestion => await GetAllQuestionsAsync(id) as TEntity[],
                EntityType.Option => await GetAllOptionsAsync(id) as TEntity[],
                EntityType.ShowCondition => await GetAllConditionsAsync(id) as TEntity[],
                EntityType.AnsweringSession => await GetAllAnsweringSessionsAsync() as TEntity[],
                EntityType.Answer => await GetAllAnswersAsync(id) as TEntity[],
                _ => Array.Empty<TEntity>(),
            };
        }
        public async Task<int> Count<TEntity>() where TEntity : class
        {
            var type = GetEntityType<TEntity>();
            if (type == EntityType.SolForm || type == EntityType.AnsweringSession)            
                return await _dbSet.CountAsync(x => x.EntityType == type);            
            else           
                throw new NotImplementedException($"Create method not implemented for {type}");            
        }
        public async Task Create<TEntity>(TEntity val, Guid? parentId = null) where TEntity : class
        {
            var entityType = GetEntityType<TEntity>();
            await (entityType switch
            {
                EntityType.SolForm => CreateFormAsync(val as SolForm ?? new SolForm()),
                EntityType.SolFormSection => CreateSectionAsync(val as SolFormSection ?? new SolFormSection(), parentId.Value),
                EntityType.BaseQuestion => CreateQuestionAsync(val as BaseQuestion ?? new BaseQuestion(), parentId.Value),
                EntityType.Option => CreateOptionAsync(val as Option ?? new Option(), parentId.Value),
                EntityType.ShowCondition => CreateConditionAsync(val as ShowCondition ?? new ShowCondition(), parentId.Value),
                EntityType.AnsweringSession => CreateAnsweringSessionAsync(val as AnsweringSession ?? new AnsweringSession()),
                EntityType.Answer => CreateAnswerAsync(val as Answer ?? new Answer(), parentId.Value),
                _ => throw new NotImplementedException($"Create method not implemented for {entityType}")
            });
        }
        public async Task Update<TEntity>(Guid id, TEntity val) where TEntity : class
        {
            var entityType = GetEntityType<TEntity>();
            switch (entityType)
            {
                case EntityType.SolForm:
                    await UpdateFormAsync(id, val as SolForm ?? new SolForm());
                    break;
                case EntityType.SolFormSection:
                    await UpdateSectionAsync(id, val as SolFormSection ?? new SolFormSection());
                    break;
                case EntityType.BaseQuestion:
                    await UpdateQuestionAsync(id, val as BaseQuestion ?? new BaseQuestion());
                    break;
                case EntityType.Option:
                    await UpdateOptionAsync(id, val as Option ?? new Option());
                    break;
                case EntityType.ShowCondition:
                    await UpdateConditionAsync(id, val as ShowCondition ?? new ShowCondition());
                    break;
                case EntityType.AnsweringSession:
                    await UpdateAnsweringSessionAsync(id, val as AnsweringSession ?? new AnsweringSession());
                    break;
                case EntityType.Answer:
                    await UpdateAnswerAsync(id, val as Answer ?? new Answer());
                    break;
                default:
                    break;
            };
        }
        public async Task<bool> Delete<TEntity>(Guid id) where TEntity : class
        {
            var entityType = GetEntityType<TEntity>();
            return entityType switch
            {
                EntityType.SolForm => await DeleteFormAsync(id),
                EntityType.SolFormSection => await DeleteSectionAsync(id),
                EntityType.BaseQuestion => await DeleteQuestionAsync(id),
                EntityType.Option => await DeleteOptionAsync(id),
                EntityType.ShowCondition => await DeleteConditionAsync(id),
                EntityType.AnsweringSession => await DeleteAnsweringSessionAsync(id),
                EntityType.Answer => await DeleteAnswerAsync(id),
                _ => false,
            };
        }
        public async Task<bool> DeleteAll<TEntity>(Guid? id = null) where TEntity : class
        {
            var entityType = GetEntityType<TEntity>();
            return entityType switch
            {
                EntityType.SolForm => await DeleteAllFormsAsync(),
                EntityType.SolFormSection => await DeleteAllSectionsAsync(id),
                EntityType.BaseQuestion => await DeleteAllQuestionsAsync(id),
                EntityType.Option => await DeleteAllOptionsAsync(id),
                EntityType.ShowCondition => await DeleteAllConditionsAsync(id),
                EntityType.AnsweringSession => await DeleteAllAnsweringSessionsAsync(),
                EntityType.Answer => await DeleteAllAnswersAsync(id),
                _ => false,
            };
        }
        private static EntityType GetEntityType<T>()
        {
            if (Enum.TryParse(typeof(T).Name, out EntityType result))
                return result;
            return EntityType.SolForm;
        }

        #region Get
        private async Task<SolForm?> GetFormAsync(Guid key)
        {
            var form = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.SolForm && x.Id == key);
            if (form != null)
                return JsonSerializer.Deserialize<SolForm>(form.Value);
            return default;
        }
        private async Task<SolFormSection?> GetSectionAsync(Guid key)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value);
                if (data != null)
                    foreach (var section in data.FormSections)
                        if (section.Id == key)
                            return section;
            }
            return default;
        }
        private async Task<BaseQuestion?> GetQuestionAsync(Guid key)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value);
                if (data != null)
                    foreach (var section in data.FormSections)
                        foreach (var question in section.Questions ?? new List<BaseQuestion>())
                            if (question.Id == key)
                                return question;
            }
            return default;
        }
        private async Task<Option?> GetOptionAsync(Guid key)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm?>(form.Value);
                if (data != null)
                    foreach (var section in data.FormSections)
                        foreach (var question in section.Questions ?? new List<BaseQuestion>())
                            foreach (var option in question.Options ?? new List<Option>())
                                if (option.Id == key)
                                    return option;
            }
            return default;
        }
        private async Task<ShowCondition?> GetConditionAsync(Guid key)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm?>(form.Value);
                if (data != null)
                    foreach (var section in data.FormSections)
                        foreach (var question in section.Questions ?? new List<BaseQuestion>())
                            if (question.ShowCondition.Id == key)
                                return question.ShowCondition;
            }
            return default;
        }
        private async Task<AnsweringSession?> GetAnsweringSessionAsync(Guid key)
        {
            var submission = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.AnsweringSession && x.Id == key);
            if (submission != null)
                return JsonSerializer.Deserialize<AnsweringSession>(submission.Value);
            return default;
        }
        private async Task<Answer?> GetAnswerAsync(Guid key)
        {
            var submissions = await _dbSet.Where(x => x.EntityType == EntityType.AnsweringSession).ToListAsync();
            foreach (var submission in submissions)
            {
                var data = JsonSerializer.Deserialize<AnsweringSession>(submission.Value);
                if (data != null)
                    foreach (var answer in data.Answers ?? new List<Answer>())
                        if (answer.Id == key)
                            return answer;
            }
            return default;
        }
        #endregion

        #region GetAll
        private async Task<SolForm[]?> GetAllFormsAsync()
        {
            var result = new List<SolForm>();
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            if (forms != null)
                foreach (var form in forms)
                {
                    var data = JsonSerializer.Deserialize<SolForm>(form.Value);
                    if (data != null)
                        result.Add(data);
                }
            return result.ToArray();
        }
        private async Task<SolFormSection[]?> GetAllSectionsAsync(Guid? formId)
        {
            var form = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.SolForm && x.Id == formId);
            if (form != null)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value);
                if (data != null)
                    return data.FormSections.ToArray() ?? Array.Empty<SolFormSection>();
            }
            return Array.Empty<SolFormSection>();
        }
        private async Task<BaseQuestion[]?> GetAllQuestionsAsync(Guid? sectionId)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value);
                var section = data?.FormSections.FirstOrDefault(x => x.Id == sectionId);
                return section?.Questions?.ToArray() ?? Array.Empty<BaseQuestion>();
            }
            return Array.Empty<BaseQuestion>();
        }
        private async Task<Option[]?> GetAllOptionsAsync(Guid? questionId)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value) ?? new SolForm();
                foreach (var section in data.FormSections)
                {
                    var question = section?.Questions?.FirstOrDefault(x => x.Id == questionId);
                    return question?.Options?.ToArray();
                }
            }
            return Array.Empty<Option>();
        }
        private async Task<ShowCondition[]?> GetAllConditionsAsync(Guid? sectionId)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value);
                var section = data?.FormSections.FirstOrDefault(x => x.Id == sectionId);
                return section?.Questions?.Select(x => x.ShowCondition).ToArray();
            }
            return Array.Empty<ShowCondition>();
        }
        private async Task<AnsweringSession[]?> GetAllAnsweringSessionsAsync()
        {
            var result = new List<AnsweringSession>();
            var submissions = await _dbSet.Where(x => x.EntityType == EntityType.AnsweringSession).ToArrayAsync();
            foreach (var submission in submissions)
            {
                var data = JsonSerializer.Deserialize<AnsweringSession>(submission.Value);
                if (data != null)
                    result.Add(data);
            }
            return result.ToArray();
        }
        private async Task<Answer[]?> GetAllAnswersAsync(Guid? sessionId)
        {
            var submission = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.AnsweringSession && x.Id == sessionId);
            if (submission != null)
            {
                return JsonSerializer.Deserialize<AnsweringSession>(submission.Value)?.Answers?.ToArray();
            }

            return Array.Empty<Answer>();
        }
        #endregion

        #region Create
        private async Task CreateFormAsync(SolForm form)
        {
            var result = JsonSerializer.Serialize(form);
            var newForm = new FormsKeyTypeValue 
            {
                Id=Guid.NewGuid(), 
                EntityType = EntityType.SolForm, 
                Value = result 
            };
            await _dbSet.AddAsync(newForm);
            await _context.SaveChangesAsync();
        }
        private async Task CreateSectionAsync(SolFormSection section, Guid formId)
        {
            var form = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.SolForm && x.Id == formId);
            if (form == null) throw new Exception("Form not found.");

            var formData = JsonSerializer.Deserialize<SolForm>(form.Value);
            if (formData?.FormSections == null) formData.FormSections = new List<SolFormSection>();
            formData?.FormSections.Add(section);

            form.Value = JsonSerializer.Serialize(formData);
            _dbSet.Update(form);
            await _context.SaveChangesAsync();
        }
        private async Task CreateQuestionAsync(BaseQuestion question, Guid sectionId)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var formData = JsonSerializer.Deserialize<SolForm>(form.Value);
                var section = formData?.FormSections.FirstOrDefault(s => s.Id == sectionId);
                if (section != null)
                {
                    if (section.Questions == null) section.Questions = new List<BaseQuestion>();
                    section?.Questions?.Add(question);
                    form.Value = JsonSerializer.Serialize(formData);
                    _dbSet.Update(form);
                    await _context.SaveChangesAsync();
                    return;
                }
            }
        }
        private async Task CreateOptionAsync(Option option, Guid questionId)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var formData = JsonSerializer.Deserialize<SolForm>(form.Value);
                foreach (var section in formData?.FormSections ?? new List<SolFormSection>())
                {
                    var question = section?.Questions?.FirstOrDefault(q => q.Id == questionId);
                    if (question != null)
                    {
                        if (question.Options == null) question.Options = new List<Option>();
                        question.Options.Add(option);
                        form.Value = JsonSerializer.Serialize(formData);
                        _dbSet.Update(form);
                        await _context.SaveChangesAsync();
                        return;
                    }
                }
            }
        }
        private async Task CreateConditionAsync(ShowCondition condition, Guid questionId)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var formData = JsonSerializer.Deserialize<SolForm>(form.Value);
                foreach (var section in formData.FormSections)
                {
                    var question = section?.Questions?.FirstOrDefault(q => q.Id == questionId);
                    if (question != null)
                    {
                        question.ShowCondition = condition;
                        form.Value = JsonSerializer.Serialize(formData);
                        _dbSet.Update(form);
                        await _context.SaveChangesAsync();
                        return;
                    }
                }
            }
        }
        private async Task CreateAnsweringSessionAsync(AnsweringSession answeringSession)
        {
            var result = JsonSerializer.Serialize(answeringSession);
            var newAnsweringSession = new FormsKeyTypeValue
            {
                Id = Guid.NewGuid(),
                EntityType = EntityType.AnsweringSession,
                Value = result
            };
            await _dbSet.AddAsync(newAnsweringSession);
            await _context.SaveChangesAsync();
        }
        private async Task CreateAnswerAsync(Answer answer, Guid answeringSessionId)
        {
            var session = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.AnsweringSession && x.Id == answeringSessionId);
            if (session == null) throw new Exception("Answering Session not found.");

            var sessionData = JsonSerializer.Deserialize<AnsweringSession>(session.Value);
            if (sessionData?.Answers == null) sessionData.Answers = new List<Answer>();
            sessionData?.Answers?.Add(answer);

            session.Value = JsonSerializer.Serialize(sessionData);
            _dbSet.Update(session);
            await _context.SaveChangesAsync();            
        }
        #endregion

        #region Update
        private async Task UpdateFormAsync(Guid id, SolForm form)
        {
            var result = JsonSerializer.Serialize(form);
            var data = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.SolForm && x.Id == id);
            if (data != null)
            {
                data.Value = result;
                _dbSet.Update(data);
                await _context.SaveChangesAsync();
            }
        }
        private async Task UpdateSectionAsync(Guid id, SolFormSection section)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var formData = JsonSerializer.Deserialize<SolForm>(form.Value);
                if (formData != null)
                {
                    for (int i = 0; i < formData.FormSections.Count; i++)
                    {
                        if (formData.FormSections[i].Id == id)
                        {
                            formData.FormSections[i] = section;
                            await _context.SaveChangesAsync();
                            return;
                        }
                    }
                }
            }
        }
        private async Task UpdateQuestionAsync(Guid id, BaseQuestion question)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var formData = JsonSerializer.Deserialize<SolForm>(form.Value);
                if (formData != null)
                    for (int i = 0; i < formData?.FormSections.Count; i++)
                    {
                        for (int j = 0; j < formData?.FormSections?[i].Questions?.Count; j++)
                        {
                            if (formData?.FormSections?[i].Questions?[j].Id == id)
                            {
                                formData.FormSections[i].Questions[j] = question;
                                await _context.SaveChangesAsync();
                                return;
                            }
                        }
                    }
            }
        }
        private async Task UpdateOptionAsync(Guid id, Option option)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var formData = JsonSerializer.Deserialize<SolForm?>(form.Value);
                if (formData != null)
                    for (int i = 0; i < formData?.FormSections.Count; i++)
                    {
                        for (int j = 0; j < formData?.FormSections?[i].Questions?.Count; j++)
                        {
                            for (int k = 0; k < formData?.FormSections?[i].Questions?[j].Options?.Count; k++)
                            {
                                if (formData?.FormSections?[i].Questions?[j].Options?[k].Id == id)
                                {
                                    formData.FormSections[i].Questions[j].Options[k] = option;
                                    await _context.SaveChangesAsync();
                                    return;
                                }
                            }
                        }
                    }
            }

        }
        private async Task UpdateConditionAsync(Guid id, ShowCondition condition)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var formData = JsonSerializer.Deserialize<SolForm?>(form.Value);
                if (formData != null)
                    for (int i = 0; i < formData.FormSections.Count; i++)
                    {
                        for (int j = 0; j < formData?.FormSections?[i].Questions?.Count; j++)
                        {
                            if (formData?.FormSections?[i].Questions?[j].ShowCondition.Id == id)
                            {
                                formData.FormSections[i].Questions[j].ShowCondition = condition;
                                await _context.SaveChangesAsync();
                                return;
                            }
                        }
                    }
            }
        }
        private async Task UpdateAnsweringSessionAsync(Guid id, AnsweringSession answeringSession)
        {
            var result = JsonSerializer.Serialize(answeringSession);
            var data = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.AnsweringSession && x.Id == id);
            if (data != null)
            {
                data.Value = result;
                _dbSet.Update(data);
                await _context.SaveChangesAsync();
            }
        }
        private async Task UpdateAnswerAsync(Guid id, Answer answer)
        {
            var sessions = await _dbSet.Where(x => x.EntityType == EntityType.AnsweringSession).ToListAsync();
            foreach (var session in sessions)
            {
                var sessionData = JsonSerializer.Deserialize<AnsweringSession>(session.Value);
                if (sessionData != null)
                {
                    for (int i = 0; i < sessionData?.Answers?.Count; i++)
                    {
                        if (sessionData.Answers[i].Id == id)
                        {
                            sessionData.Answers[i] = answer;
                            await _context.SaveChangesAsync();
                            return;
                        }
                    }
                }
            }
        }
        #endregion

        #region Delete
        private async Task<bool> DeleteFormAsync(Guid? key)
        {
            var form = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.SolForm && x.Id == key);
            if (form != null)
            {
                _dbSet.Remove(form);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        private async Task<bool> DeleteSectionAsync(Guid? key)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value) ?? new SolForm();
                foreach (var section in data.FormSections)
                {
                    if (section.Id == key)
                    {
                        data.FormSections.Remove(section);
                        return await _context.SaveChangesAsync() > 1;
                    }
                }
            }
            return false;
        }
        private async Task<bool> DeleteQuestionAsync(Guid? key)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value) ?? new SolForm();
                foreach (var section in data.FormSections)
                {
                    foreach (var question in section.Questions ?? new List<BaseQuestion>())
                    {
                        if (question.Id == key)
                        {
                            section?.Questions?.Remove(question);
                            return await _context.SaveChangesAsync() > 1;
                        }
                    }
                }
            }
            return false;
        }
        private async Task<bool> DeleteOptionAsync(Guid? key)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm?>(form.Value) ?? new SolForm();
                foreach (var section in data.FormSections)
                {
                    foreach (var question in section.Questions ?? new List<BaseQuestion>())
                    {
                        foreach (var option in question.Options ?? new List<Option>())
                        {
                            if (option.Id == key)
                            {
                                question.Options?.Remove(option);
                                return await _context.SaveChangesAsync() > 1;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private async Task<bool> DeleteConditionAsync(Guid? key)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm?>(form.Value) ?? new SolForm();
                foreach (var section in data.FormSections)
                {
                    foreach (var question in section.Questions ?? new List<BaseQuestion>())
                    {
                        if (question.ShowCondition.Id == key)
                        {
                            question.ShowCondition = new ShowCondition();
                            return await _context.SaveChangesAsync() > 1;
                        }
                    }
                }
            }
            return false;
        }
        private async Task<bool> DeleteAnsweringSessionAsync(Guid? key)
        {
            var submission = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.AnsweringSession && x.Id == key);
            if (submission != null)
            {
                _dbSet.Remove(submission);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        private async Task<bool> DeleteAnswerAsync(Guid? key)
        {
            var submissions = await _dbSet.Where(x => x.EntityType == EntityType.AnsweringSession).ToListAsync();
            foreach (var submission in submissions)
            {
                var data = JsonSerializer.Deserialize<AnsweringSession>(submission.Value);
                if (data != null)
                {
                    foreach (var answer in data.Answers ?? new List<Answer>())
                    {
                        if (answer.Id == key)
                        {
                            data?.Answers?.Remove(answer);
                            return await _context.SaveChangesAsync() > 0;
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region DeleteAll
        private async Task<bool> DeleteAllFormsAsync()
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            if (forms != null)
            {
                _dbSet.RemoveRange(forms);
                return await _context.SaveChangesAsync() > 1;
            }
            return false;
        }
        private async Task<bool> DeleteAllSectionsAsync(Guid? formId)
        {
            var form = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.SolForm && x.Id == formId);
            if (form != null)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value);
                if (data != null)
                {
                    data.FormSections.RemoveAll(x => x.FormId == formId);
                    return await _context.SaveChangesAsync() > 1;
                }
            }
            return false;
        }
        private async Task<bool> DeleteAllQuestionsAsync(Guid? sectionId)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value);
                var section = data?.FormSections.FirstOrDefault(x => x.Id == sectionId);
                section?.Questions?.RemoveAll(x => x.SectionId == sectionId);
                return await _context.SaveChangesAsync() > 1;
            }
            return false;
        }
        private async Task<bool> DeleteAllOptionsAsync(Guid? questionId)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value) ?? new SolForm();
                foreach (var section in data.FormSections)
                {
                    var question = section?.Questions?.FirstOrDefault(x => x.Id == questionId);
                    question?.Options?.RemoveAll(x => x.QuestionId == questionId);
                    return await _context.SaveChangesAsync() > 1;
                }
            }
            return false;
        }
        private async Task<bool> DeleteAllConditionsAsync(Guid? sectionId)
        {
            var forms = await _dbSet.Where(x => x.EntityType == EntityType.SolForm).ToListAsync();
            foreach (var form in forms)
            {
                var data = JsonSerializer.Deserialize<SolForm>(form.Value);
                var section = data?.FormSections.FirstOrDefault(x => x.Id == sectionId);
                foreach (var question in section?.Questions ?? new List<BaseQuestion>())
                {
                    question.ShowCondition = new ShowCondition();
                }
            }
            return await _context.SaveChangesAsync() > 1;
        }
        private async Task<bool> DeleteAllAnsweringSessionsAsync()
        {
            var submissions = await _dbSet.Where(x => x.EntityType == EntityType.AnsweringSession).ToListAsync();
            if (submissions != null)
            {
                _dbSet.RemoveRange(submissions);
                return await _context.SaveChangesAsync() > 1;
            }
            return false;
        }
        private async Task<bool> DeleteAllAnswersAsync(Guid? sessionId)
        {
            var submission = await _dbSet.FirstOrDefaultAsync(x => x.EntityType == EntityType.AnsweringSession && x.Id == sessionId);
            if (submission != null)
            {
                var data = JsonSerializer.Deserialize<AnsweringSession>(submission.Value);
                if (data != null)
                {
                    data?.Answers?.RemoveAll(x => x.SubmissionId == sessionId);
                    return await _context.SaveChangesAsync() > 1;
                }
            }
            return false;
        }
        #endregion

        //TODO : Complete this
        public Task<bool> IsRedFlag<TEntity>(Guid id) where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}
