using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace CLS.Infrastructure.Interfaces
{
    public interface IEntities : IDisposable
    {
        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry Entry(object entity);
        int SaveChanges();
    }
}
