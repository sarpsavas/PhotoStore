using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UserUpdatePassword
{
    public record UserUpdatePasswordCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string OldPasswordHash { get; set; }
        public string NewPasswordHash { get; set; }
        
    }
}
