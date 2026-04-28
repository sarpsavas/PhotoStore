using Core.Abstractions.Repositories;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.ViewUserListings
{
    public class ViewUserListingsQueryHandler : IRequestHandler<ViewUserListingsQuery,List<Listing>>
    {
        private readonly IListingRepository _lRepository;

        public ViewUserListingsQueryHandler(IListingRepository lRepository)
        {
            _lRepository = lRepository;
        }

        public async Task<List<Listing>> Handle(Guid userId, CancellationToken cancellationToken)
        {

        }
    }
}
