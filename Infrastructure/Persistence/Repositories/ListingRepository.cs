using Core.Abstractions.Repositories;
using Core.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ListingRepository :IListingRepository, IRepository<Listing>
    {
        private readonly ConnContext _context;

        public ListingRepository(ConnContext context)
        {
            _context = context;
        }

        public async Task<List<Listing>> GetListingsByLetters(string letters)
        {
            var response = await _context.Listings.Where(x => x.UserName.Contains(letters)).ToListAsync();
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
            return response;
        }

        public async Task<List<Listing>> GetUserListingsByUserId(Guid userId)
        {
            var response = await _context.Listings.Where(x => x.UserId == userId).ToListAsync();
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
            return response;
        }

        public async Task Add(Listing listing)
        {
            await _context.Listings.AddAsync(listing);
            
        }

        public async Task Update(Listing listing)
        {
            var response = await _context.Listings.Where(x => x.ListingId == listing.ListingId).FirstOrDefaultAsync();

            response.ListingName = listing.ListingName;
            response.ListingId = listing.ListingId;
            response.ListingDescription = listing.ListingDescription;
            response.ListingDate = listing.ListingDate;
            response.Address = listing.Address;
            response.ImageUrls = listing.ImageUrls;
            response.UserId = listing.UserId;
            response.UserName = listing.UserName;
            response.Price = listing.Price;
        }
        public async Task Delete(Guid userId)
        {
            var response = await _context.Listings.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if(response != null)
            {
                _context.Listings.Remove(response);
            }
            
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
            
        }
    }
}
