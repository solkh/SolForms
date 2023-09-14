using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SolForms.Models;
using SolForms.Models.Questions;
using SolForms.Options;
using System.Text.Json;

namespace SolForms.Data.DataSourceImp
{
    public class KeyValueDataSource : IFormsDataSource
    {
        private readonly DbContext _context;
        private readonly SolFormOptions _options;
        private readonly DbSet<FormsKeyValue> _dbSet;
        public KeyValueDataSource(IOptions<SolFormOptions> option, DbContext context)
        {
            _context = context;
            _options = option.Value;
            _dbSet = context.Set<FormsKeyValue>();
        }
        public async Task<int> Count<TEntity>() where TEntity : class
        {
            var prefix = GetPrefixKey<TEntity>();
            return await _dbSet.Where(x => x.Key.StartsWith(prefix)).CountAsync();
        }
        public async Task Create<TEntity>(TEntity val, Guid? parentId = null) where TEntity : class
        {
            var id = Guid.NewGuid();
            var prefix = GetPrefixKey<TEntity>(id.ToString());            
            var data = new FormsKeyValue()
            {
                Key = prefix,
                Value = JsonSerializer.Serialize(val)
            };
            await _dbSet.AddAsync(data);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> Delete<TEntity>(Guid key) where TEntity : class
        {            
            var data = await _dbSet.Where(x=>x.Key == GetPrefixKey<TEntity>(key.ToString())).FirstOrDefaultAsync();
            if(data != null)            
                _dbSet.Remove(data);            
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAll<TEntity>(Guid? id = null) where TEntity : class
        {
            var prefix = GetPrefixKey<TEntity>();
            var data = await _dbSet.Where(x => x.Key.StartsWith(prefix)).ToArrayAsync(); 
            if(data != null)            
                _dbSet.RemoveRange(data);            
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<TEntity?> Get<TEntity>(Guid key) where TEntity : class
        {            
            var data = await _dbSet.Where(x => x.Key == GetPrefixKey<TEntity>(key.ToString())).FirstOrDefaultAsync();
            if (data != null)            
                return JsonSerializer.Deserialize<TEntity>(data.Value);            
            return default;            
        }
        public async Task<TEntity?[]?> GetAll<TEntity>(Guid? id = null) where TEntity : class
        {
            var prefix = GetPrefixKey<TEntity>();
            var entries = await _dbSet.Where(x => x.Key.StartsWith(prefix)).ToArrayAsync();
            var values = entries.Select(x => x.Value).ToArray();
            var result = new List<TEntity?>();
            foreach (var value in values)
            {
                var item = JsonSerializer.Deserialize<TEntity>(value);
                if (item != null)
                    result.Add(item);
            }
            return result.ToArray();
        }
        public async Task Update<TEntity>(Guid key, TEntity val) where TEntity : class
        {                        
            var data = _dbSet.Where(x => x.Key == GetPrefixKey<TEntity>(key.ToString())).FirstOrDefault();
            if (data != null)
            {
                data.Value = JsonSerializer.Serialize(val);
                _dbSet.Update(data);
            }
            await _context.SaveChangesAsync();
        }
        private string GetPrefixKey<TEntity>(string? key = null)
        {
            key = string.IsNullOrEmpty(key) ? "" : $"_{key}";
            if (typeof(TEntity) == typeof(SolForm))            
                return _options.Forms + key;            
            else if (typeof(TEntity) == typeof(SFSubmission))            
                return _options.Submissions + key;            
            else if (typeof(TEntity) == typeof(SFAnswer))            
                return _options.Submissions + key;            
            return string.Empty;
        }

        //TODO : Complete this
        public Task<bool> IsRedFlag<TEntity>(Guid id) where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}