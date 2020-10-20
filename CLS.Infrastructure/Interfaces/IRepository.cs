using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CLS.Infrastructure.Interfaces
{
    public interface IRepository<T> : IEnumerable<T>, IRepository where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Get(int id);
        T Get(params object[] keyValues);
        T Put(T entity);
        List<dynamic> SqlQuery(string sql);
        void Delete(T entity);
        void DeleteMany(List<T> entities);
        void CascadingDelete(T entity);
    }

    public interface IRepository { }
}
