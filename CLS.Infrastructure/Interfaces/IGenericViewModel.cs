using System.Collections.Generic;

namespace CLS.Infrastructure.Interfaces
{
    public interface IGenericViewModel<T> where T : class
    {
        List<string> Errors { get; set; }
        List<T> Entities { get; set; }
        T Entity { get; set; }
        bool Put(IUnitOfWork uow);
    }
}
