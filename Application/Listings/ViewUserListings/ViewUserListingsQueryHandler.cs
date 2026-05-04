using Application.Responses;
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
    public class ViewUserListingsQueryHandler : IRequestHandler<ViewUserListingsQuery,ListingResponse>
    {
        private readonly IListingRepository _lRepository;
        private readonly IQuestionRepository _qRepository;

        public ViewUserListingsQueryHandler(IListingRepository lRepository,
            IQuestionRepository qRepository)
        {
            _lRepository = lRepository;
            _qRepository = qRepository;
        }

        public async Task<ListingResponse> Handle(ViewUserListingsQuery request, CancellationToken cancellationToken)
        {
            var listings = await _lRepository.GetUserListingsByUserId(request.UserId);

            List<List<Question>> questions = new List<List<Question>>();

            foreach (var listing in listings)
            {
                questions.Add(await _qRepository.GetQuestionsByListingId(listing.ListingId));
            }
            ListingResponse response = new ListingResponse();
            response.Listings = listings;
            response.Questions = questions;
            return response;
        }
    }
}
