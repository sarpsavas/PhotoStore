using Application.Encryptors;
using Core.Abstractions.Repositories;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand,Unit>
    {
        private readonly IRepository<User> _repository;

        public RegisterCommandHandler(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            User user = new User();
            user.Name = request.Name;
            user.LastName = request.LastName;
            user.EMail = request.EMail;

            Encryption en = new Encryption();
            user.PasswordHash = en.Encryptor(request.Password);

            await _repository.Add(user);
            return Unit.Value;
        }
    }
}
