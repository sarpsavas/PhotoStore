using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Repositories
{
    public interface IListingRepository
    {
        Task<List<Listing>> GetListingsByLetters(string letter);
        Task<List<Listing>> GetUserListingsByUserId(Guid userId);
    }
}
