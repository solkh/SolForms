//using Microsoft.EntityFrameworkCore;
//using SolForms.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SolForms.Data.DataSourceImp
//{
//    public class MySqlDataSource : IFormsDataSource
//    {
//        private readonly DbContext _context;
//        private readonly DbSet<SolForm> _dbSet;

//        public MySqlDataSource(DbContext context)
//        {
//            _context = context;
//            _dbSet = context.Set<SolForm>();
//        }
//        public async Task<TEntity?> Get<TEntity>(string id) where TEntity : class
//        {
//            var dbSet = _context.Set<TEntity>();
//            return await dbSet.FindAsync(id);
//        }
//        public async Task<TEntity?[]> GetAll<TEntity>() where TEntity : class
//        {
//            var dbSet = _context.Set<TEntity>();
//            return await dbSet.ToArrayAsync();
//        }
//        public async Task Create<TEntity>(TEntity val) where TEntity : class
//        {
//            var dbSet = _context.Set<TEntity>();
//            dbSet.Add(val);
//            await _context.SaveChangesAsync();
//        }
//        public async Task<int> Count<TEntity>() where TEntity : class
//        {
//            var dbSet = _context.Set<TEntity>();
//            return await dbSet.CountAsync();
//        }
//        public async Task<bool> Delete<TEntity>(string id) where TEntity : class
//        {
//            var dbSet = _context.Set<TEntity>();
//            var entity = await dbSet.FindAsync(id);

//            if (entity != null)
//            {
//                dbSet.Remove(entity);
//                return await _context.SaveChangesAsync() > 0;
//            }
//            return false;
//        }
//        public async Task<bool> DeleteAll<TEntity>() where TEntity : class
//        {
//            var dbSet = _context.Set<TEntity>();
//            var entities = await dbSet.ToListAsync();

//            if (entities.Count > 0)
//            {
//                dbSet.RemoveRange(entities);
//                return await _context.SaveChangesAsync() > 0;
//            }
//            return false;
//        }

//        public async Task Update<TEntity>(string key, TEntity updatedEntity) where TEntity : class
//        {
//            var dbSet = _context.Set<TEntity>();
            

//            dbSet.Update(updatedEntity);
//            //var entry = _context.Entry(updatedEntity);
//            //if (entry.State == EntityState.Detached)
//            //{
//            //    dbSet.Attach(updatedEntity);
//            //}
//            //
//            //entry.State = EntityState.Modified;

//            await _context.SaveChangesAsync();
//        }
//    }
//}
