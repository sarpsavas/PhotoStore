using Application.Helpers.Encryptors;
using Core.Abstractions.Repositories;
using Core.Abstractions.UnitOfWork;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(IRepository<User> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            User user = new User();
            user.Name = request.Name;
            user.LastName = request.LastName;
            user.setEMail(request.EMail);

            EncryptionHelper en = new EncryptionHelper();
            user.PasswordHash = en.Encryptor(request.Password);
 
            await _repository.Add(user);

            
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(Path.GetFullPath("app.db----"));
                throw new Exception(ex.InnerException?.Message);
            }
            
            return Unit.Value;
        }
    }
}
