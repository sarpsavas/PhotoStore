using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UserUpdateEMail
{
    public record UserUpdateEMailCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string EMail { get; set; }

       
    }
}
