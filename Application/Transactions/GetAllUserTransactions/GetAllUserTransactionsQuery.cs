using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transactions.GetAllUserTransactions
{
    public record GetAllUserTransactionsQuery : IRequest<List<Transaction>>
    {
        public Guid UserId { get; set; }
    }
}
