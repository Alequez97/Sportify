using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public class MsSqlDataRepository<T> : IDataRepository<T>
    {

        public MsSqlDataRepository(DbContext)
        {

        }
        public T Read(int id)
        {
            throw new NotImplementedException();
        }

        public List<T> ReadAll()
        {
            throw new NotImplementedException();
        }
    }
}
