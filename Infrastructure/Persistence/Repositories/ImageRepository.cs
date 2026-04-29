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
    public class ImageRepository : IImageRepository, IRepository<Image>
    {
        private readonly ConnContext _context;
        public ImageRepository(ConnContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetImagesUrlByListingIdAsync(Guid ListingId)
        {
            var response = _context.Images.Where(x => x.ListingId == ListingId).Select(u => u.Url).ToList();
            if ( response == null) { throw new Exception("repository error"); }
            return response;
        }

        public async Task Add(Image image)
        {
            await _context.Images.AddAsync(image);
        }

        
        public async Task Update(Image image)
        {
            throw new Exception("image delete repository error[update close]");
        }
        public async Task Delete(Guid wrongMethod)
        {
            throw new Exception("image delete repository error[wrong parameters]");
        }
        public async Task Delete(string url)
        {
            var response = await _context.Images.Where(x => x.Url == url).FirstOrDefaultAsync();
            if (response == null) { throw new Exception("repository error"); }
            _context.Images.Remove(response);
        }
    }
}
