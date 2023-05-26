using System.Collections.Generic;

namespace DemoWebApp.Services.Repositories
{
    public interface IRepository<T>
    {
        T Create(T entity);
        List<T> Read(T filterBy, string orderBy, string order, int page, int perPage);
        T Read(int id);
        void Update(T entity);
        void Delete(int id);
        int Count(T filterBy);
    }
}
