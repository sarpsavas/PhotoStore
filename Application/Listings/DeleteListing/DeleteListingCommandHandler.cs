using Core.Abstractions.Repositories;
using Core.Abstractions.UnitOfWork;
using Core.Entities;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.DeleteListing
{
    public class DeleteListingCommandHandler : IRequestHandler<DeleteListingCommand,Unit>
    {
        private readonly IRepository<Listing> _repositoryLi;
        private readonly IRepository<Image> _repositoryIm;
        private readonly IListingRepository _lisRepository;
        private readonly IRepository<Transaction> _repositoryTr;
        private readonly IUnitOfWork _unit;

        public DeleteListingCommandHandler(IRepository<Listing> repositoryLi, 
            IRepository<Image> repositoryIm,
            IUnitOfWork unit,
            IListingRepository lisRepository,
            IRepository<Transaction> repositoryTr)
        {
            _repositoryIm = repositoryIm;
            _repositoryLi = repositoryLi;
            _unit = unit;
            _lisRepository = lisRepository;
            _repositoryTr = repositoryTr;
        }

        public async Task<Unit> Handle(DeleteListingCommand request, CancellationToken cancellationToken)
        {
            Transaction transaction = new Transaction();
            transaction.Description = null;
            
            transaction.Type = TransactionType.DeleteListing;

            try
            {
                var listing = await _lisRepository.GetListingByListingId(request.ListingId);
                await _repositoryLi.Delete(request.ListingId);
                transaction.Success = TransactionSuccess.Successful;
                transaction.UserId = listing.UserId;
            }
            catch (Exception)
            {

                transaction.Success = TransactionSuccess.Unsuccessful;
            }
            await _repositoryTr.Add(transaction);
            await _unit.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
