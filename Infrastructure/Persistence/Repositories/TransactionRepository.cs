using Core.Abstractions.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository, IRepository<Transaction>
    {
        private readonly ConnContext _context;

        public TransactionRepository(ConnContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetUserTransactionsByUserId(Guid userId)
        {
            var response = await _context.Transactions.Where(x => x.UserId == userId).ToListAsync();
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
            return response;
        }
        public async Task Add(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }
        public async Task Update(Transaction transaction)
        {
            var response = await _context.Transactions.Where(x => x.TransactionId == transaction.TransactionId).FirstOrDefaultAsync();
            response.TransactionId = transaction.TransactionId;
            response.UserId = transaction.UserId;
            response.TransactionTime = transaction.TransactionTime;
            response.Type = transaction.Type;
            response.Description = transaction.Description;
        }
        public async Task Delete(Guid transactionId)
        {
            var response = await _context.Transactions.Where(x => x.TransactionId == transactionId).FirstOrDefaultAsync();
            
            if (response == null) { throw new Exception("Database response null[ss-01]"); }

            if (response != null)
            {
                _context.Transactions.Remove(response);
            }
        }
    }
}
