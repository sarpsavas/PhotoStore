using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.GetListingImage
{
    public record GetListingImageQuery : IRequest<List<string>>
    {
        public Guid ListingId { get; set; }
    }
}
