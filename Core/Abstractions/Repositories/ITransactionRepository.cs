using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Abstractions.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactionsByUserId(Guid userId);
    }
}
