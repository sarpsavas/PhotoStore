using Core.Abstractions.Repositories;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transactions.GetAllUserTransactions
{
    public class GetAllUserTransactionsQueryHandler : IRequestHandler<GetAllUserTransactionsQuery,List<Transaction>>
    {
        private readonly ITransactionRepository _tranRepository;

        public GetAllUserTransactionsQueryHandler(ITransactionRepository tranRepository)
        {
            _tranRepository = tranRepository;
        }

        public async Task<List<Transaction>> Handle(GetAllUserTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _tranRepository.GetUserTransactionsByUserId(request.UserId);
        }
    }
}
