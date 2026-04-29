using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.DeleteListing
{
    public record DeleteListingCommand : IRequest<Unit>
    {
        public Guid ListingId { get; set; }
    }
}
