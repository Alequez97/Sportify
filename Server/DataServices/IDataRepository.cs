using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public interface IDataRepository<T>
    {

        T Read(int id);
        
        List<T> ReadAll();
    }
}
