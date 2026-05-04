using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public Guid? UserId { get; set; }
        public string QuestionText { get; set; }
        public Guid ListingId { get; set; }
        public string? AnswerText { get; set; }
        public DateTime QuestionDate { get; set; }
        public DateTime? AnswerDate { get; set; }

        public Question()
        {
            QuestionId = Guid.NewGuid();
            QuestionDate = DateTime.UtcNow;
        }
    }
}
