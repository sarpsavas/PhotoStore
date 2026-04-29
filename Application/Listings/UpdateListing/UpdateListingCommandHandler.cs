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

namespace Application.Listings.UpdateListing
{
    public class UpdateListingCommandHandler : IRequestHandler<UpdateListingCommand,Unit>
    {
        private readonly IRepository<Listing> _repositoryLi;
        private readonly IUserRepository _userRepository;
        private readonly IListingRepository _listingRepository;
        private readonly IUnitOfWork _unit;

        public UpdateListingCommandHandler(IRepository<Listing> repositoryLi,
            IUnitOfWork unit,
            IUserRepository userRepository,
            IListingRepository listingRepository)
        {
            _repositoryLi = repositoryLi;
            _unit = unit;
            _userRepository = userRepository;
            _listingRepository = listingRepository;
        }

        public async Task<Unit> Handle(UpdateListingCommand request, CancellationToken cancellationToken)
        {
            Transaction transaction = new Transaction();
            transaction.Type = TransactionType.UpdateListing;
            transaction.UserId = request.UserId;
            try
            {
                Listing listing = new Listing();
                listing = await _listingRepository.GetListingByListingId(request.ListingId);
                listing.ListingId = request.ListingId;
                listing.UserId = request.UserId;

                var user = await _userRepository.GetUserByUserId(request.UserId);

                listing.UserName = $"{user.Name} {user.LastName}";
                listing.ListingName = request.ListingName;
                listing.ListingDescription = request.ListingDescription;
                listing.Address = request.Address;
                listing.Price = request.Price;
                listing.Contact = request.Contact;
                listing.Category = request.Category;

                await _repositoryLi.Update(listing);
                transaction.Success = TransactionSuccess.Successful;
            }
            catch (Exception)
            {

                transaction.Success = TransactionSuccess.Unsuccessful;
            }

            await _unit.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
