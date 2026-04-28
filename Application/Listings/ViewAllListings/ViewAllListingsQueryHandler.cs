using Core.Abstractions.Repositories;
using Core.Abstractions.UnitOfWork;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.ViewAllListings
{
    public class ViewAllListingsQueryHandler : IRequestHandler<ViewAllListingsQuery, List<Listing>>
    {
        private readonly IListingRepository _lRepository;

        public ViewAllListingsQueryHandler(IListingRepository lRepository)
        {
            _lRepository = lRepository;
            
            
        }

        public async Task<List<Listing>> Handle(ViewAllListingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _lRepository.GetTenListing(request.page);

                
                return response;
            }
            catch (Exception ex)
            {
                throw;
                
            }
            
        }
    }
}
