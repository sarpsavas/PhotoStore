using Core.Abstractions.Repositories;
using Core.Entities;
using Core.Enums;
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
        public async Task<Listing> GetListingByListingId(Guid listingId)
        {
            var response = await _context.Listings.Where(x => x.ListingId == listingId).FirstOrDefaultAsync();
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
            return response;
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

        public async Task<List<Listing>> GetTenListing(int page)
        {

            int pages = page * 15;

            if (pages < 0 || pages > 98)
            {
                throw new Exception("istenilen sayfa sayısı geçersiz.");
            }

                var response = await _context.Listings.OrderByDescending(x => x.ListingDate).Skip(pages).Take(15).ToListAsync();
            return response;
        }
        public async Task<List<Listing>> GetFilterTenListing(int page, ListingCategories category,decimal minValue, decimal maxValue)
        {

            
            int pages = page * 15;

            if (pages < 0 || pages > 98)
            {
                throw new Exception("istenilen sayfa sayısı geçersiz.");
            }

            var response = await _context.Listings.OrderByDescending(x => x.ListingDate).Where(s => s.Category == category && s.Price > minValue && s.Price < maxValue).Skip(pages).Take(15).ToListAsync();
            return response;
        }

        public async Task Add(Listing listing)
        {
            await _context.Listings.AddAsync(listing);
            
        }

        public async Task Update(Listing listing)
        {
            var response = await _context.Listings.Where(x => x.ListingId == listing.ListingId).FirstOrDefaultAsync();

            if (response != null)
            {
                response.ListingName = listing.ListingName;
                
                response.ListingDescription = listing.ListingDescription;
                
                response.Address = listing.Address;
                
                response.UserName = listing.UserName;
                response.Price = listing.Price;
                response.Category = listing.Category;
                response.Contact = listing.Contact;
            }
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
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
