using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Repositories
{
    public interface IRepository<T>
    {
        Task Add(T obj);

        Task Update(T obj);

        Task Delete(T obj);

    }
}
