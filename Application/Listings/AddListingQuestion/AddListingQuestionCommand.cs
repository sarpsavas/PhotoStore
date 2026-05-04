using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.AddListingQuestion
{
    public record AddListingQuestionCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string QuestionText { get; set; }
        public Guid ListingId { get; set; }


    }
}
