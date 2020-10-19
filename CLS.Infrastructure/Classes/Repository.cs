using CLS.Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace CLS.Infrastructure.Classes
{
    /// <summary>
    /// Generic class used to encapsulate interactions with a set of database entities.
    /// </summary>
    /// <typeparam name="T">Type of database entity e.g. TableName.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Set of database entities being interacted with.
        /// </summary>
        private IEntities _db;

        /// <summary>
        /// Constructor to instantiate a list of db entities.
        /// </summary>
        /// <param name="db">Entities to use.</param>
        public Repository(IEntities db)
        {
            _db = db;
        }

        /// <summary>
        /// Public getter for the DB context.
        /// </summary>
        public DbContext DB => _db as DbContext;

        /// <summary>
        /// Method which flags an entity as deleted.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public void Delete(T entity)
        {
            var entry = _db.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        /// <summary>
        /// Flags many entities as deleted.
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteMany(List<T> entities)
        {
            foreach (var entity in entities)
            {
                var entry = _db.Entry(entity);
                entry.State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// Returns an IQueryable matching the search criteria passed in.
        /// </summary>
        /// <param name="predicate">Expression used to search for matching entities.</param>
        /// <returns>IQueryable matching the search criteria.</returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().Where(predicate);
        }

        /// <summary>
        /// Returns the entity with an id matching the one passed in.
        /// </summary>
        /// <param name="id">Id of the entity to get.</param>
        /// <returns>Entity of type T matching the Id being searched for.</returns>
        public T Get(int id)
        {
            return _db.Set<T>().Find(id);
        }

        /// <summary>
        /// Gets an entity matching key value pairs passed in.
        /// </summary>
        /// <param name="keyValues">A set of key value pairs used to find a matching entity.</param>
        /// <returns>An entity matching key value pairs passed in.</returns>
        public T Get(params object[] keyValues)
        {
            return _db.Set<T>().Find(keyValues);
        }

        /// <summary>
        /// Returns an IQueryable for the entire set.
        /// </summary>
        /// <returns>Whole set of entities of type T.</returns>
        public IQueryable<T> GetAll()
        {
            return _db.Set<T>();
        }

        /// <summary>
        /// Used to add or update an entity into the db entities set.
        /// </summary>
        /// <param name="entity">The entity to add or update into the set.</param>
        /// <returns>The entity passed into this method.</returns>
        public T Put(T entity)
        {
            _db.Set<T>().AddOrUpdate(entity);
            return entity;
        }

        /// <summary>
        /// Used to query the database directly and return a list of results.
        /// </summary>
        /// <param name="sql">SQL query code.</param>
        /// <returns>A list of dynamic objects as a result of the SQL query.</returns>
        public List<dynamic> SqlQuery(string sql)
        {
            if (_db is DbContext)
            {
                var result = ((DbContext)_db).Database.SqlQuery<dynamic>(sql);
                return result.ToList();
            }
            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (_db.Set<T>().AsEnumerable()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
