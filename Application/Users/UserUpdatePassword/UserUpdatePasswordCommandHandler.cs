using Application.Users.UserUpdateEMail;
using Core.Abstractions.Repositories;
using Core.Abstractions.UnitOfWork;
using Core.Entities;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UserUpdatePassword
{
    public class UserUpdatePasswordCommandHandler : IRequestHandler<UserUpdatePasswordCommand,Unit>
    {
        private readonly IRepository<Transaction> _repositoryT;
        private readonly IRepository<User> _repositoryU;
        private readonly IUserRepository _usRepository;
        private readonly IUnitOfWork _unit;

        public UserUpdatePasswordCommandHandler(IRepository<Transaction> repositoryT,
            IRepository<User> repositoryU,
            IUnitOfWork unit,
            IUserRepository usRepository
            )
        {
            _repositoryT = repositoryT;
            _repositoryU = repositoryU;
            _unit = unit;
            _usRepository = usRepository;
        }

        public async Task<Unit> Handle(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            Transaction transaction = new Transaction();
            transaction.UserId = request.UserId;
            transaction.Description = null;
            transaction.Type = TransactionType.UpdateUser;

            try
            {
                var user = await _usRepository.GetUserByUserId(request.UserId);
                if(user.PasswordHash == request.OldPasswordHash)
                {
                    throw new Exception("şifre değişimi hatası[mevcut şifre yanlış]");
                }
                user.PasswordHash = request.NewPasswordHash;

                await _repositoryU.Update(user);

                transaction.Success = TransactionSuccess.Successful;
                await _repositoryT.Add(transaction);
                await _unit.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            catch (Exception)
            {
                transaction.Success = TransactionSuccess.Unsuccessful;
                await _repositoryT.Add(transaction);
                await _unit.SaveChangesAsync(cancellationToken);
                throw new Exception("UserUpdatePassword error");
            }
        }
    }
}
