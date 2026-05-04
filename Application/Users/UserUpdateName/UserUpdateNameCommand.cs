using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UserUpdateName
{
    public record UserUpdateNameCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
