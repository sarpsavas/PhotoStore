using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Repositories
{
    public interface IListingRepository
    {
        Task<Listing> GetListingByListingId(Guid listingId);
        Task<List<Listing>> GetListingsByLetters(string letter);
        Task<List<Listing>> GetUserListingsByUserId(Guid userId);
        Task<List<Listing>> GetFilterTenListing(int page, ListingCategories category, decimal minValue, decimal maxValue);
        Task<List<Listing>> GetTenListing(int page);
    }
}
