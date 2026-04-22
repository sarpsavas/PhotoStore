using Application.Encryptors;
using Core.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.LogIn
{
    public class LogInQueryHandler : IRequestHandler<LogInQuery, Guid>
    {
        private readonly IUserRepository _URepository;

        public LogInQueryHandler(IUserRepository uRepository)
        {
            _URepository = uRepository;
        }
        public async Task<Guid> Handle(LogInQuery request, CancellationToken cancellationToken)
        {
            Encryption en = new Encryption();
            string passwordHash = en.Encryptor(request.Password);
            return await _URepository.GetUserIdByEMailAndPasswordHash(request.EMail, passwordHash);
        }
    }
}
