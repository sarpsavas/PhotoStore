using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Abstractions.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetUserTransactionsByUserId(Guid userId);
    }
}
