using Application.Helpers.Encryptors;
using Application.Helpers.TokenGenerator;
using Application.Responses;
using Core.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Helpers.Authentication;

namespace Application.Users.LogIn
{
    public class LogInQueryHandler : IRequestHandler<LogInQuery, LogInResponse>
    {
        private readonly IUserRepository _URepository;
        private readonly AuthService _auth;
        public LogInQueryHandler(IUserRepository uRepository,
            AuthService auth)
        {
            _URepository = uRepository;
            _auth = auth;
        }
        public async Task<LogInResponse> Handle(LogInQuery request, CancellationToken cancellationToken)
        {
            EncryptionHelper en = new EncryptionHelper();
            string passwordHash = en.Encryptor(request.Password);

            LogInResponse response = new LogInResponse();
            response.UserId = await _URepository.GetUserIdByEMailAndPasswordHash(request.EMail, passwordHash);
            response.Token = _auth.GenerateToken(request.EMail);
            return response;
        }
    }
}
