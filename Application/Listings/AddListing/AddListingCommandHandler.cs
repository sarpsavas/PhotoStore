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

namespace Application.Listings.AddListing
{
    public class AddListingCommandHandler : IRequestHandler<AddListingCommand,Unit>
    {
        private readonly IRepository<Listing> _repositoryL;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Image> _repositoryIm;
        private readonly IRepository<Transaction> _repositoryTr;
        private readonly IUnitOfWork _unitOfWork;
        

        public AddListingCommandHandler(IRepository<Listing> repositoryL,
            IUserRepository userRepository,
            IRepository<Image> repositoryIm,
            IUnitOfWork unitOfWork,
            IRepository<Transaction> repositoryTr)
        {
            _repositoryL = repositoryL;
            _userRepository = userRepository;
            _repositoryIm = repositoryIm;
            _unitOfWork = unitOfWork;
            _repositoryTr = repositoryTr;
        }

        public async Task<Unit> Handle(AddListingCommand request, CancellationToken cancellationToken)
        {
            Transaction transaction = new Transaction();
            Listing listing = new Listing();
            

            transaction.UserId = request.UserId;
            transaction.Type = TransactionType.AddListing;
            transaction.Description = null;
            try
            {
                listing.UserId = request.UserId;
                var user = await _userRepository.GetUserByUserId(request.UserId);
                listing.UserName = $"{user.Name} {user.LastName}";
                listing.ListingName = request.ListingName;
                listing.ListingDescription = request.ListingDescription;
                listing.Address = request.Address;
                listing.ListingDate = DateTime.UtcNow;

                listing.Price = request.Price;
                listing.Contact = request.Contact;
                listing.Category = request.Category;

                await _repositoryL.Add(listing);

                transaction.Success = TransactionSuccess.Successful;
            }
            catch (Exception)
            {

                transaction.Success = TransactionSuccess.Unsuccessful;
                
            }
            
            await _repositoryTr.Add(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
