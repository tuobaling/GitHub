using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.DAL.Models;
using Trello.DAL.Repositories;

namespace Trello.DAL.UnitOfWorks
{
    public interface IUnitOfWorks : IDisposable
    {
        /// <summary>
        /// 取得某一個Entity的Repository。
        /// 如果沒有取過，會initialise一個
        /// 如果有就取得之前initialise的那個。
        /// </summary>
        /// <typeparam name="TEntity">此Context裡面的Entity Type</typeparam>
        /// <returns>Entity的Repository</returns>
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void SaveChanges();
        void SaveChangesAsync();
    }

    public class UnitOfWorks : IUnitOfWorks
    {
        private readonly DataModel _context;
        private Hashtable _repositories;

        //private IGenericRepository<TEntity> _Repository;

        public UnitOfWorks(DataModel context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();

                var disposable = _repositories as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }

        /// <summary>
        /// 取得某一個Entity的Repository。
        /// 如果沒有取過，會initialise一個
        /// 如果有就取得之前initialise的那個。
        /// </summary>
        /// <typeparam name="TEntity">此Context裡面的Entity Type</typeparam>
        /// <returns>Entity的Repository</returns>
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async void SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
