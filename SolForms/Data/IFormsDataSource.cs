using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Data
{
    public interface IFormsDataSource
    {
        Task<TEntity?> Get<TEntity>(Guid id) where TEntity : class;
        Task<TEntity?[]?> GetAll<TEntity>(Guid? id = null) where TEntity : class;
        Task Create<TEntity>(TEntity val, Guid? parentId = null) where TEntity : class;
        Task Update<TEntity>(Guid id, TEntity val) where TEntity : class;
        Task<int> Count<TEntity>() where TEntity : class;
        Task<bool> Delete<TEntity>(Guid id) where TEntity : class;
        Task<bool> DeleteAll<TEntity>(Guid? id = null) where TEntity : class;
    }
}