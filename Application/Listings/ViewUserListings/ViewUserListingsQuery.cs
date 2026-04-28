using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.ViewUserListings
{
    public record ViewUserListingsQuery : IRequest<List<Listing>>
    {
        public Guid UserId { get; set; }
    }
}
