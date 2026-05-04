using Core.Abstractions.Repositories;
using Core.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class QuestionRepository : IQuestionRepository, IRepository<Question>
    {
        public readonly ConnContext _context;

        public QuestionRepository(ConnContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetQuestionsByListingId(Guid listingId)
        {
            var response = await _context.Questions.Where(x => x.ListingId == listingId).ToListAsync();
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
            return response;
        }
        public async Task<Question> GetQuestionByQuestionId(Guid questionId)
        {
            var response = await _context.Questions.Where(x => x.QuestionId == questionId).FirstOrDefaultAsync();
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
            return response;
        }

        public async Task Add(Question question)
        {
            await _context.Questions.AddAsync(question);
        }
        public async Task Update(Question question)
        {
            var response = await _context.Questions.Where(x => x.QuestionId == question.QuestionId).FirstOrDefaultAsync();
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
            if(response != null)
            {
                response.QuestionId = question.QuestionId;
                response.UserId = question.UserId;
                response.QuestionText = question.QuestionText;
                response.QuestionDate = question.QuestionDate;
                response.AnswerDate = question.AnswerDate;
                response.AnswerText = question.AnswerText;
                response.ListingId = question.ListingId;
            }
        }
        public async Task Delete(Guid questionId)
        {
            var response = await _context.Questions.Where(x => x.QuestionId == questionId).FirstOrDefaultAsync();
            if (response == null) { throw new Exception("Database response null[ss-01]"); }
            else if(response != null)
            {
                _context.Questions.Remove(response);
            }
        }
    }
}
