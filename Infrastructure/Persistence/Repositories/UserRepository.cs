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
    public class UserRepository : IUserRepository, IRepository<User>
    {
        private readonly ConnContext _context;

        public UserRepository(ConnContext context)
        {
            _context = context;
        }
        public async Task<Guid> GetUserIdByEMailAndPasswordHash(string eMail, string passwordHash)
        {
            
            var response = await _context.Users.
                Where(x => x.EMail == eMail && x.PasswordHash == passwordHash)
                .FirstOrDefaultAsync();
            if( response == null) { throw new Exception("Database response null[ss-01]"); }
            return response.UserId;

        }

        public async Task Add(User user)
        {

        }

        public async Task Update(User user)
        {

        }

        public async Task Delete(string UserId)
        {

        }

    }
}
