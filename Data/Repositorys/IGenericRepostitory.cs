using System.Collections.Generic;

namespace Data.Repositorys
{
    public interface IGenericRepostitory<T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
    }
}
