using Application.Responses;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.ViewFilterListings
{
    public class ViewFilterListingsQuery : IRequest<ListingResponse>
    {
        public int Page { get; set; }
        public ListingCategories Category { get; set; }
        public decimal minValue { get; set; }
        public decimal maxValue { get; set; }
    }
}
