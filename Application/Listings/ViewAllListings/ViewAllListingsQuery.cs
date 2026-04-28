using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.ViewAllListings
{
    public record ViewAllListingsQuery : IRequest<List<Listing>>
    {
        public int page { get; set; }
    }
}
