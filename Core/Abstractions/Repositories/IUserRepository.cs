using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<Guid> GetUserIdByEMailAndPasswordHash(string eMail, string passwordHash);
        Task<User> GetUserByUserId(Guid UserId);
    }
}
