using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Trello.DAL.Models;

namespace Trello.DAL.Repositories
{
    #region 泛型Repository Interface
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Insert(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);
        int Count();
        Task<int> CountAsync();
        void Delete(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        void Dispose();
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(int id);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetAll();
        Task<ICollection<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetAsync(int id);
        void Save();
        Task<int> SaveAsync();
        TEntity Update(TEntity entity, object key);
        Task<TEntity> UpdateAsync(TEntity entity, object key);
    }
    #endregion

    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DataModel _context;

        public GenericRepository(DataModel context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {

            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual TEntity Get(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = GetAll();

            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
                queryable = queryable.Include<TEntity, object>(includeProperty);

            if (predicate != null)
                queryable = queryable.Where(predicate);

            if (orderBy != null)
                queryable = orderBy(queryable);

            return queryable.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = GetAll();

            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
                queryable = queryable.Include<TEntity, object>(includeProperty);

            if (predicate != null)
                queryable = queryable.Where(predicate);

            if (orderBy != null)
                queryable = orderBy(queryable);

            return await queryable.ToListAsync();
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {

            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).ToList();
        }

        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public virtual TEntity Update(TEntity entity, object key)
        {
            if (entity == null)
                return null;
            TEntity exist = _context.Set<TEntity>().Find(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
            return exist;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, object key)
        {
            if (entity == null)
                return null;
            TEntity exist = await _context.Set<TEntity>().FindAsync(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
            return exist;
        }

        public int Count()
        {
            return _context.Set<TEntity>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        public virtual void Save()
        {

            _context.SaveChanges();
        }

        public async virtual Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(predicate);
            return query;
        }

        public virtual async Task<ICollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {

            IQueryable<TEntity> queryable = GetAll();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<TEntity, object>(includeProperty);
            }

            return queryable;
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //private DataModel context;
        //private IDbSet<TEntity> dbSet;

        //public GenericRepository(DataModel context)
        //{
        //    this.context = context;
        //    this.dbSet = context.Set<TEntity>();
        //}

        //public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        //{
        //    IQueryable<TEntity> query = dbSet;

        //    foreach (Expression<Func<TEntity, object>> include in includes)
        //        query = query.Include(include);

        //    if (filter != null)
        //        query = query.Where(filter);

        //    if (orderBy != null)
        //        query = orderBy(query);

        //    return query.ToList();
        //}

        //public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        //{
        //    IQueryable<TEntity> query = dbSet;

        //    if (filter != null)
        //        query = query.Where(filter);

        //    if (orderBy != null)
        //        query = orderBy(query);

        //    return query;
        //}

        //public virtual TEntity GetById(object id)
        //{
        //    return dbSet.Find(id);
        //}

        //public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        //{
        //    IQueryable<TEntity> query = dbSet;

        //    foreach (Expression<Func<TEntity, object>> include in includes)
        //        query = query.Include(include);

        //    return query.FirstOrDefault(filter);
        //}

        //public virtual void Insert(TEntity entity)
        //{
        //    dbSet.Add(entity);
        //}

        //public virtual void Update(TEntity entity)
        //{
        //    dbSet.Attach(entity);
        //    context.Entry(entity).State = EntityState.Modified;
        //}

        //public virtual void Delete(object id)
        //{
        //    TEntity entityToDelete = dbSet.Find(id);
        //    if (context.Entry(entityToDelete).State == EntityState.Detached)
        //    {
        //        dbSet.Attach(entityToDelete);
        //    }
        //    dbSet.Remove(entityToDelete);
        //}

        //public virtual void Delete(TEntity entity)
        //{
        //    if (context.Entry(entity).State == EntityState.Detached)
        //    {
        //        dbSet.Attach(entity);
        //    }
        //    dbSet.Remove(entity);
        //}
    }
}
