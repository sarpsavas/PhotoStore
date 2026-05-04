using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.AddListingAnswer
{
    public record AddListingAnswerCommand : IRequest<Unit>
    {
        public Guid QuestionId { get; set; }
        public string? AnswerText { get; set; }
        public DateTime? AnswerDate { get; set; }
        public Guid UserId { get; set; }
    }
}
