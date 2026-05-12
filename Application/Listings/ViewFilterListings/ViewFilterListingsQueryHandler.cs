using Application.Responses;
using Core.Abstractions.Repositories;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.ViewFilterListings
{
    public class ViewFilterListingsQueryHandler : IRequestHandler<ViewFilterListingsQuery,ListingResponse>
    {
        private IListingRepository _lisRepository;
        private IQuestionRepository _qRepository;

        public ViewFilterListingsQueryHandler(IListingRepository lisRepository,
            IQuestionRepository qRepository)
        {
            _lisRepository = lisRepository;
            _qRepository = qRepository;
        }

        public async Task<ListingResponse> Handle(ViewFilterListingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                ListingResponse response = new ListingResponse();

                List<Listing> listings = new List<Listing>();
                List<List<Question>> allQuestions = new List<List<Question>>();

                listings = await _lisRepository.GetFilterTenListing(request.Page, request.Category, request.minValue, request.maxValue);

                foreach (var listing in listings)
                {
                    var responseDb = await _qRepository.GetQuestionsByListingId(listing.ListingId);
                    allQuestions.Add(responseDb);
                }
                response.Listings = listings;
                response.Questions = allQuestions;

                return response;
            }
            catch (Exception)
            {

                throw new Exception("ViewFilterListingsQueryHandler error");
            }

        }
    }
}
