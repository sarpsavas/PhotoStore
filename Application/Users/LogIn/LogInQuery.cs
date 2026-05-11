using Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.LogIn
{
    public record LogInQuery : IRequest<LogInResponse>
    {
        
        public string EMail { get; set; }
        public string Password { get; set; }
    }
}
