using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public record ListingResponse
    {
        public List<Listing> Listings { get; set; }
        public List<List<Question>> Questions { get; set; }
    }
}
