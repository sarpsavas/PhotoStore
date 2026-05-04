using Application.Responses;
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
    public class ViewAllListingsQueryHandler : IRequestHandler<ViewAllListingsQuery, ListingResponse>
    {
        private readonly IListingRepository _lRepository;
        private readonly IQuestionRepository _qRepository;

        public ViewAllListingsQueryHandler(IListingRepository lRepository,
            IQuestionRepository qRepository)
        {
            _lRepository = lRepository;
            _qRepository = qRepository;
        }

        public async Task<ListingResponse> Handle(ViewAllListingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                ListingResponse response = new ListingResponse();

                List<Listing> listings = new List<Listing>();
                List<List<Question>> allQuestions = new List<List<Question>>();

                listings = await _lRepository.GetTenListing(request.page);

                foreach (var listing in listings)
                {
                    var responseDb = await _qRepository.GetQuestionsByListingId(listing.ListingId);
                    allQuestions.Add(responseDb);
                }
                response.Listings = listings;
                response.Questions = allQuestions;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("ViewAllListingsQuery error");
            }
            
        }
    }
}
