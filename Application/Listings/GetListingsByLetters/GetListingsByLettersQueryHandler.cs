using Application.Responses;
using Core.Abstractions.Repositories;
using Core.Entities;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.GetListingsByLetters
{
    public class GetListingsByLettersQueryHandler : IRequestHandler<GetListingsByLettersQuery,ListingResponse>
    {
        private IListingRepository _liRepository;
        private IQuestionRepository _quRepository;

        public GetListingsByLettersQueryHandler(IListingRepository liRepository,
            IQuestionRepository quRepository)
        {
            _liRepository = liRepository;
            _quRepository = quRepository;
        }

        public async Task<ListingResponse> Handle(GetListingsByLettersQuery request, CancellationToken cancellationToken)
        {
            ListingResponse response = new ListingResponse();
            List<Listing> listings = new List<Listing>();
            List<List<Question>> questions = new List<List<Question>>();
            listings = await _liRepository.GetListingsByLetters(request.Letters);

            foreach (var listing in listings)
            {
                questions.Add(await _quRepository.GetQuestionsByListingId(listing.ListingId));
            }
            response.Questions = questions;
            response.Listings = listings;
            return response;

        }
    }
}
