using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public class MockSqlDataRepository<T> : IDataRepository<T> where T : EntityBase, new()
    {
        private readonly List<T> entites;

        public MockSqlDataRepository(List<T> entites)
        {
            this.entites = entites;
        }

        public bool Create(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public List<T> FindByExpression(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] properties)
        {
            throw new NotImplementedException();
        }

        public T Read(int id)
        {
            return entites.Where(entity => entity.Id == id).FirstOrDefault();
        }

        public List<T> ReadAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
