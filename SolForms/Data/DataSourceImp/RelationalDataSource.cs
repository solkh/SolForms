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
                EntityType.SolFormSection => CreateSectionAsync(parentId.Value, val as SFSection ?? new SFSection()),
                EntityType.BaseQuestion => CreateQuestionAsync(parentId.Value, val as SFQuestion ?? new SFQuestion()),
                EntityType.Option => CreateOptionAsync(parentId.Value, val as SFOption ?? new SFOption()),
                EntityType.ShowCondition => CreateConditionAsync(parentId.Value, val as SFShowCondition ?? new SFShowCondition()),
                EntityType.AnsweringSession => CreateAnsweringSessionAsync(val as SFSubmition ?? new SFSubmition()),
                EntityType.Answer => CreateAnswerAsync(parentId.Value, val as SFAnswer ?? new SFAnswer()),
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
        private async Task<SolForm?> GetFormAsync(Guid? id) =>
            await _context.Set<SolForm>()
                          .AsQueryable()
                          .AsNoTracking()
                          .Include(x => x.FormSections)
                          .ThenInclude(y => y.Questions)
                          .ThenInclude(z => z.Options)
                          .Include(x => x.FormSections)
                          .ThenInclude(y => y.Questions)
                          .ThenInclude(z => z.ShowCondition)
                          .FirstOrDefaultAsync(x => x.Id == id);

        private async Task<SFSection?> GetSectionAsync(Guid? id) =>
            await _context.Set<SFSection>().Where(s => s.Id == id)
                .Include(q => q.Questions)
                    .ThenInclude(o => o.Options)
                .FirstOrDefaultAsync();
        private async Task<SFQuestion?> GetQuestionAsync(Guid? id) =>
            await _context.Set<SFQuestion>()
                          .AsQueryable()
                          .AsNoTracking()
            .Where(x => x.Id == id)
                .Include(o => o.Options)
                .FirstOrDefaultAsync();
        private async Task<SFOption?> GetOptionAsync(Guid? id) =>
            await _context.Set<SFOption>().FirstOrDefaultAsync(x => x.Id == id);
        private async Task<SFSubmition?> GetAnsweringSessionAsync(Guid? id) =>
            await _context.Set<SFSubmition>()
                          .AsQueryable()
                          .AsNoTracking()
            .Where(x => x.Id == id)
                .Include(a => a.Answers)
                .FirstOrDefaultAsync();
        private async Task<SFAnswer?> GetAnswerAsync(Guid? id) =>
            await _context.Set<SFAnswer>().FindAsync(id);
        private async Task<SFShowCondition?> GetConditionAsync(Guid? id) => 
            await _context.Set<SFShowCondition>()
                          .FindAsync(id);
        #endregion

        #region GetAll
        private async Task<SolForm[]?> GetAllFormsAsync() =>
            await _context.Set<SolForm>()
                          .AsQueryable()
                          .AsNoTracking()
                          .Include(x => x.FormSections)
                          .ThenInclude(y => y.Questions)
                          .ThenInclude(z => z.Options)
                          .Include(x => x.FormSections)
                          .ThenInclude(y => y.Questions)
                          .ThenInclude(z => z.ShowCondition)
                          .ToArrayAsync();

        private async Task<SFSection[]?> GetAllSectionsAsync(Guid? formId) =>
            await _context.Set<SFSection>()
                          .AsQueryable()
                          .AsNoTracking()
                          .Where(s => s.FormId == formId)
                          .Include(q => q.Questions)
                          .ThenInclude(o => o.Options)
                          .ToArrayAsync();

        private async Task<SFQuestion[]?> GetAllQuestionsAsync(Guid? sectionId) =>
            await _context.Set<SFQuestion>()
                          .AsQueryable()
                          .AsNoTracking()
                          .Where(x => x.SectionId == sectionId)
                          .Include(o => o.Options)
                          .ToArrayAsync();

        private async Task<SFOption[]?> GetAllOptionsAsync(Guid? questionId) =>
            await _context.Set<SFOption>()
                          .AsQueryable()
                          .AsNoTracking()
                          .Where(x => x.QuestionId == questionId)
                          .ToArrayAsync();

        private async Task<SFSubmition[]?> GetAllAnsweringSessionsAsync() =>
            await _context.Set<SFSubmition>()
                          .Include(a => a.Answers)
                          .ToArrayAsync();

        private async Task<SFAnswer[]?> GetAllAnswersAsync(Guid? sessionId) =>
            await _context.Set<SFAnswer>()
                          .AsQueryable()
                          .AsNoTracking()
                          .Where(x => x.SessionId == sessionId)
                          .ToArrayAsync();

        #endregion

        #region Create
        private async Task CreateFormAsync(SolForm form)
        {
            var dbSet = _context.Set<SolForm>();
            await _dbSet.AddAsync(form);
            await _context.SaveChangesAsync();
        }
        private async Task CreateSectionAsync(Guid formId, SFSection section)
        {
            var dbSet = _context.Set<SFSection>();
            section.FormId = formId;
            await dbSet.AddAsync(section);
            await _context.SaveChangesAsync();
        }
        private async Task CreateQuestionAsync(Guid sectionId, SFQuestion question)
        {
            var dbSet = _context.Set<SFQuestion>();
            question.SectionId = sectionId;
            await dbSet.AddAsync(question);
            await _context.SaveChangesAsync();
        }
        private async Task CreateOptionAsync(Guid questionId, SFOption option)
        {
            var dbSet = _context.Set<SFOption>();
            option.QuestionId = questionId;
            await dbSet.AddAsync(option);
            await _context.SaveChangesAsync();
        }
        private async Task CreateConditionAsync(Guid questionId, SFShowCondition condition)
        {
            //TODO: Fix
            var dbSet = _context.Set<SFShowCondition>();
            condition.QuestionId = questionId;
            await dbSet.AddAsync(condition);
            await _context.SaveChangesAsync();
        }
        private async Task CreateAnsweringSessionAsync(SFSubmition session)
        {
            var dbSet = _context.Set<SFSubmition>();
            await dbSet.AddAsync(session);
            await _context.SaveChangesAsync();
        }
        private async Task CreateAnswerAsync(Guid sessionId, SFAnswer answer)
        {
            var dbSet = _context.Set<SFAnswer>();
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
            var dbSet = _context.Set<SFSection>();
            var sections = await dbSet.Where(x => x.FormId == formId).ToListAsync();
            dbSet.RemoveRange(sections);
            return await _context.SaveChangesAsync() > 1;
        }
        private async Task<bool> DeleteAllQuestionsAsync(Guid? sectionId)
        {
            var dbSet = _context.Set<SFQuestion>();
            var questions = await dbSet.Where(x => x.SectionId == sectionId).ToListAsync();
            dbSet.RemoveRange(questions);
            return await _context.SaveChangesAsync() > 1;
        }
        private async Task<bool> DeleteAllOptionsAsync(Guid? questionId)
        {
            var dbSet = _context.Set<SFOption>();
            var options = await dbSet.Where(x => x.QuestionId == questionId).ToListAsync();
            dbSet.RemoveRange(options);
            return await _context.SaveChangesAsync() > 1;
        }
        private async Task<bool> DeleteAllAnsweringSessionsAsync()
        {
            var dbSet = _context.Set<SFSubmition>();
            var sessions = await dbSet.ToListAsync();
            dbSet.RemoveRange(sessions);
            return await _context.SaveChangesAsync() > 1;
        }
        private async Task<bool> DeleteAllAnswersAsync(Guid? sessionId)
        {
            var dbSet = _context.Set<SFAnswer>();
            var answers = await dbSet.Where(x => x.SessionId == sessionId).ToListAsync();
            dbSet.RemoveRange(answers);
            return await _context.SaveChangesAsync() > 1;
        }
        #endregion
    }
}