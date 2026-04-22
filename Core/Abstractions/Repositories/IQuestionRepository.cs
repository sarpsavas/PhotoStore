using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestionByListingId(Guid listingId);
    }
}
