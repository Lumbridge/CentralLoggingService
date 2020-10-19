using CLS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace CLS.Infrastructure.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Set of database entities being interacted with.
        /// </summary>
        private IEntities _db;

        /// <summary>
        /// Used to store a collection of DB entities with Repository functionality.
        /// </summary>
        private Dictionary<Type, IRepository> _repositories;

        /// <summary>
        /// Constructor to instantiate db entities and a repository to prevent null references.
        /// </summary>
        /// <param name="db"></param>
        public UnitOfWork(IEntities db)
        {
            _db = db;
            _repositories = new Dictionary<Type, IRepository>();
        }

        /// <summary>
        /// Public getter for the DB context.
        /// </summary>
        public DbContext Db => _db as DbContext;

        /// <summary>
        /// Gets a collection of db entities matching type T.
        /// </summary>
        /// <typeparam name="T">The type of db entities to add to the collection.</typeparam>
        /// <returns>Collection of db entities matching the type T.</returns>
        public IRepository<T> Repository<T>() where T : class
        {
            var key = typeof(T);
            if (!_repositories.ContainsKey(key))
            {
                IRepository<T> repos = new Repository<T>(_db);
                _repositories.Add(key, repos);
            }
            return _repositories[key] as IRepository<T>;
        }

        /// <summary>
        /// Saves all changes make to the unit of work context in a single transaction.
        /// </summary>
        public void Commit()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }

        public DateTime GetDatabaseTime()
        {
            return ((IObjectContextAdapter)_db).ObjectContext.CreateQuery<DateTime>("CurrentDateTime() ").AsEnumerable().Single();
        }
    }
}
