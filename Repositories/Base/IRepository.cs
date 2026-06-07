using System.Collections.Generic;

namespace WMS.Repositories.Base;

public interface IRepository<T> where T : class, new()
{
    T?         GetById(int id);
    List<T>    GetAll();
    int        Insert(T entity);
    int        Update(T entity);
    int        Delete(int id);
}
