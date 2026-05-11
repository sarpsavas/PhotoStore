using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Services
{
    public interface IAddListingImageService
    {
        Task SaveImage(string userId, string listingId, string url);
    }
}
