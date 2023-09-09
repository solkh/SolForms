using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
using SolForms.Models;
using SolForms.Models.Questions;

namespace SolForms.Data.DataSourceImp
{
    public class RelationalDataSource : IFormsDataSource
    {
        private readonly DbContext _context;
        private readonly DbSet<SolForm> _dbSet;

        public RelationalDataSource(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<SolForm>();
        }
        public async Task<TEntity?> Get<TEntity>(Guid id) where TEntity : class
        {
            var entityType = GetEntityType<TEntity>(); return entityType switch
            {
                EntityType.SolForm => await GetFormAsync(id) as TEntity,
                EntityType.SolFormSection => await GetSectionAsync(id) as TEntity,
                EntityType.BaseQuestion => await GetQuestionAsync(id) as TEntity,
                EntityType.Option => await GetOptionAsync(id) as TEntity,
                EntityType.AnsweringSession => await GetAnsweringSessionAsync(id) as TEntity,
                EntityType.Answer => await GetAnswerAsync(id) as TEntity,
                EntityType.ShowCondition => await GetConditionAsync(id) as TEntity,
                _ => throw new NotImplementedException($"Get method not implemented for {entityType}")
            };
        }
        public async Task<bool> IsRedFlag<TEntity>(Guid id) where TEntity : class
        {
            var dbSet = _context.Set<Option>();
            return await dbSet.Where(x => x.Id == id).Select(y => y.IsRedFlag).FirstOrDefaultAsync() ?? false;
        }
        public async Task<TEntity?[]?> GetAll<TEntity>(Guid? parentId = null) where TEntity : class
        {
            var entityType = GetEntityType<TEntity>();
            return entityType switch
            {
                EntityType.SolForm => await GetAllFormsAsync() as TEntity[],
                EntityType.SolFormSection => await GetAllSectionsAsync(parentId) as TEntity[],
                EntityType.BaseQuestion => await GetAllQuestionsAsync(parentId) as TEntity[],
                EntityType.Option => await GetAllOptionsAsync(parentId) as TEntity[],                
                EntityType.AnsweringSession => await GetAllAnsweringSessionsAsync() as TEntity[],
                EntityType.Answer => await GetAllAnswersAsync(parentId) as TEntity[],
                _ => throw new NotImplementedException($"GetAll method not implemented for {entityType}")
            };
        }        
        public async Task Create<TEntity>(TEntity val, Guid? parentId = null) where TEntity : class
        {
            var entityType = GetEntityType<TEntity>();
            await (entityType switch
            {
                EntityType.SolForm => CreateFormAsync(val as SolForm ?? new SolForm()),
                EntityType.SolFormSection => CreateSectionAsync(parentId.Value, val as SolFormSection ?? new SolFormSection()),
                EntityType.BaseQuestion => CreateQuestionAsync(parentId.Value, val as BaseQuestion ?? new BaseQuestion()),
                EntityType.Option => CreateOptionAsync(parentId.Value, val as Option ?? new Option()),
                EntityType.ShowCondition => CreateConditionAsync(parentId.Value, val as ShowCondition ?? new ShowCondition()),
                EntityType.AnsweringSession => CreateAnsweringSessionAsync(val as AnsweringSession ?? new AnsweringSession()),
                EntityType.Answer => CreateAnswerAsync(parentId.Value, val as Answer ?? new Answer()),
                _ => throw new NotImplementedException($"Create method not implemented for {entityType}")
            });
        }
        public async Task<int> Count<TEntity>() where TEntity : class
        {
            var dbSet = _context.Set<TEntity>();
            return await dbSet.CountAsync();
        }
        public async Task<bool> Delete<TEntity>(Guid id) where TEntity : class
        {
            var dbSet = _context.Set<TEntity>();
            var entity = await dbSet.FindAsync(id);

            if (entity != null)
            {
                dbSet.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> DeleteAll<TEntity>(Guid? parentId = null) where TEntity : class
        {
            var entityType = GetEntityType<TEntity>();
            return entityType switch
            {
                EntityType.SolForm => await DeleteAllFormsAsync(),
                EntityType.SolFormSection => await DeleteAllSectionsAsync(parentId),
                EntityType.BaseQuestion => await DeleteAllQuestionsAsync(parentId),
                EntityType.Option => await DeleteAllOptionsAsync(parentId),                
                EntityType.AnsweringSession => await DeleteAllAnsweringSessionsAsync(),
                EntityType.Answer => await DeleteAllAnswersAsync(parentId),
                _ => throw new NotImplementedException($"DeleteAll method not implemented for {entityType}"),
            };
        }
        public async Task Update<TEntity>(Guid id, TEntity updatedEntity) where TEntity : class
        {
            var dbSet = _context.Set<TEntity>().Update(updatedEntity);
            await _context.SaveChangesAsync();
        }
        private static EntityType GetEntityType<T>()
        {
            if (Enum.TryParse(typeof(T).Name, out EntityType result))
                return result;
            return EntityType.SolForm;
        }
        #region Get
        private async Task<SolForm?> GetFormAsync(Guid id)
        {
            var dbSet = _context.Set<SolForm>();
            return await dbSet.Where(x => x.Id == id)
                .Include(x => x.FormSections)
                    .ThenInclude(y => y.Questions)
                        .ThenInclude(z => z.Options)
            .FirstOrDefaultAsync();
        }
        private async Task<SolFormSection?> GetSectionAsync(Guid? id)
        {
            var dbSet = _context.Set<SolFormSection>();
            return await dbSet.Where(s => s.Id == id)
                .Include(q => q.Questions)
                    .ThenInclude(o => o.Options)
                .FirstOrDefaultAsync();
        }
        private async Task<BaseQuestion?> GetQuestionAsync(Guid id)
        {
            var dbSet = _context.Set<BaseQuestion>();
            return await dbSet.Where(x => x.Id == id)
                .Include(o => o.Options)
                .FirstOrDefaultAsync();
        }
        private async Task<Option?> GetOptionAsync(Guid id)
        {
            var dbSet = _context.Set<Option>();
            return await dbSet.FirstOrDefaultAsync(x=>x.Id == id);
        }
        private async Task<AnsweringSession?> GetAnsweringSessionAsync(Guid id)
        {
            var dbSet = _context.Set<AnsweringSession>();
            return await dbSet.Where(x => x.Id == id)
                .Include(a => a.Answers)
                .FirstOrDefaultAsync();
        }
        private async Task<Answer?> GetAnswerAsync(Guid id)
        {
            var dbSet = _context.Set<Answer>();
            return await dbSet.FindAsync(id);
        }
        private async Task<ShowCondition?> GetConditionAsync(Guid id)
        {
            var dbSet = _context.Set<ShowCondition>();
            return await dbSet.FindAsync(id);            
        }        
        #endregion

        #region GetAll
        private async Task<SolForm[]?> GetAllFormsAsync()
        {
            var dbSet = _context.Set<SolForm>();
            return await dbSet.Include(x => x.FormSections)
                .ThenInclude(y => y.Questions)
                    .ThenInclude(z => z.Options)
            .ToArrayAsync();            
        }
        private async Task<SolFormSection[]?> GetAllSectionsAsync(Guid? formId)
        {
            var dbSet = _context.Set<SolFormSection>();
            return await dbSet.Where(s=>s.FormId == formId)
                .Include(q => q.Questions)
                    .ThenInclude(o => o.Options)
                .ToArrayAsync();                      
        }
        private async Task<BaseQuestion[]?> GetAllQuestionsAsync(Guid? sectionId)
        {
            var dbSet = _context.Set<BaseQuestion>();
            return await dbSet.Where(x => x.SectionId == sectionId)
                .Include(o=>o.Options)
                .ToArrayAsync();
        }
        private async Task<Option[]?> GetAllOptionsAsync(Guid? questionId)
        {
            var dbSet = _context.Set<Option>();
            return await dbSet.Where(x => x.QuestionId == questionId).ToArrayAsync();
        }
        private async Task<AnsweringSession[]?> GetAllAnsweringSessionsAsync()
        {
            var dbSet = _context.Set<AnsweringSession>();
            return await dbSet.Include(a=>a.Answers).ToArrayAsync();
        }
        private async Task<Answer[]?> GetAllAnswersAsync(Guid? sessionId)
        {
            var dbSet = _context.Set<Answer>();
            return await dbSet.Where(x => x.SessionId == sessionId).ToArrayAsync();
        }
        #endregion

        #region Create
        private async Task CreateFormAsync(SolForm form)
        {
            var dbSet = _context.Set<SolForm>();
            await _dbSet.AddAsync(form);
            await _context.SaveChangesAsync();
        }
        private async Task CreateSectionAsync(Guid formId, SolFormSection section)
        {
            var dbSet = _context.Set<SolFormSection>();
            section.FormId = formId;
            await dbSet.AddAsync(section);
            await _context.SaveChangesAsync();
        }
        private async Task CreateQuestionAsync(Guid sectionId, BaseQuestion question)
        {
            var dbSet = _context.Set<BaseQuestion>();
            question.SectionId = sectionId;
            await dbSet.AddAsync(question);
            await _context.SaveChangesAsync();
        }
        private async Task CreateOptionAsync(Guid questionId, Option option)
        {
            var dbSet = _context.Set<Option>();
            option.QuestionId = questionId;
            await dbSet.AddAsync(option);
            await _context.SaveChangesAsync();
        }
        private async Task CreateConditionAsync(Guid questionId, ShowCondition condition)
        {
            //TODO: Fix
            var dbSet = _context.Set<ShowCondition>();
            condition.QuestionId = questionId;
            await dbSet.AddAsync(condition);
            await _context.SaveChangesAsync();
        }
        private async Task CreateAnsweringSessionAsync(AnsweringSession session)
        {
            var dbSet = _context.Set<AnsweringSession>();
            await dbSet.AddAsync(session);
            await _context.SaveChangesAsync();
        }
        private async Task CreateAnswerAsync(Guid sessionId, Answer answer)
        {
            var dbSet = _context.Set<Answer>();
            answer.SessionId = sessionId;
            await dbSet.AddAsync(answer);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region DeleteAll
        private async Task<bool> DeleteAllFormsAsync()
        {
            var dbSet = _context.Set<SolForm>();
            var forms = await dbSet.ToListAsync();
            dbSet.RemoveRange(forms);
            return await _context.SaveChangesAsync() > 1;
        }
        private async Task<bool> DeleteAllSectionsAsync(Guid? formId)
        {
            var dbSet = _context.Set<SolFormSection>();
            var sections = await dbSet.Where(x=>x.FormId == formId).ToListAsync();
            dbSet.RemoveRange(sections);
            return await _context.SaveChangesAsync() > 1;
        }
        private async Task<bool> DeleteAllQuestionsAsync(Guid? sectionId)
        {
            var dbSet = _context.Set<BaseQuestion>();
            var questions = await dbSet.Where(x=>x.SectionId == sectionId).ToListAsync();
            dbSet.RemoveRange(questions);
            return await _context.SaveChangesAsync() > 1;
        }
        private async Task<bool> DeleteAllOptionsAsync(Guid? questionId)
        {
            var dbSet = _context.Set<Option>();
            var options = await dbSet.Where(x => x.QuestionId == questionId).ToListAsync();
            dbSet.RemoveRange(options);
            return await _context.SaveChangesAsync() > 1;
        }
        private async Task<bool> DeleteAllAnsweringSessionsAsync()
        {
            var dbSet = _context.Set<AnsweringSession>();
            var sessions = await dbSet.ToListAsync();
            dbSet.RemoveRange(sessions);
            return await _context.SaveChangesAsync() > 1;
        }
        private async Task<bool> DeleteAllAnswersAsync(Guid? sessionId)
        {
            var dbSet = _context.Set<Answer>();
            var answers = await dbSet.Where(x=>x.SessionId == sessionId).ToListAsync();
            dbSet.RemoveRange(answers);
            return await _context.SaveChangesAsync() > 1;
        }
        #endregion
    }
}