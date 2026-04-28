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
        public async Task<User> GetUserByUserId(Guid UserId)
        {
            var response = await _context.Users.Where(x => x.UserId == UserId).FirstOrDefaultAsync();
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
            User user = new User();
            user.UserId = response.UserId;
            user.setEMail(response.EMail);
            user.Name = response.Name;
            user.LastName = response.LastName;
            
            return user;

        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task Update(User user)
        {
            var response = await _context.Users.Where(x => x.UserId == user.UserId).FirstOrDefaultAsync();

            response.UserId = user.UserId;
            response.Name = user.Name;
            response.LastName = user.LastName;
            response.setEMail(user.EMail);
            response.PasswordHash = user.PasswordHash;
        }

        public async Task Delete(Guid UserId)
        {
            var response = await _context.Users.Where(x => x.UserId == UserId).FirstOrDefaultAsync();

            if(response != null)
            {
                _context.Users.Remove(response);
            }
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
        }

    }
}
