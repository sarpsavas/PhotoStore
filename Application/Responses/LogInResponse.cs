using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class LogInResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
