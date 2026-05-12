using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.GetListingsByLetters
{
    public record GetListingsByLettersQuery : IRequest<ListingResponse>
    {
        public string Letters { get; set; }
    }
}
