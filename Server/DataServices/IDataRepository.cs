using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataServices
{
    public interface IDataRepository<T> where T : EntityBase, new()
    {
        T Read(int id);
        
        List<T> ReadAll();

        bool Create(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        List<T> FindByExpression(Expression<Func<T, bool>> expression);
    }
}
