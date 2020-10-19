using System;
using System.Data.Entity;

namespace CLS.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;
        DateTime GetDatabaseTime();
        DbContext Db { get; }
        void Commit();
    }
}
