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

namespace Application.Listings.AddListingAnswer
{
    public class AddListingAnswerCommandHandler : IRequestHandler<AddListingAnswerCommand, Unit>
    {
        private readonly IRepository<Transaction> _transaction;
        private readonly IRepository<Question> _question;
        private readonly IQuestionRepository _quesRepository;
        private readonly IUnitOfWork _unit;

        public AddListingAnswerCommandHandler(IRepository<Transaction> transaction,
            IRepository<Question> question,
            IQuestionRepository quesRepository,
            IUnitOfWork unit)
        {
            _transaction = transaction;
            _question = question;
            _quesRepository = quesRepository;
            _unit = unit;
        }

        public async Task<Unit> Handle(AddListingAnswerCommand request, CancellationToken cancellationToken)
        {
            Transaction transaction = new Transaction();
            transaction.Description = null;
            transaction.UserId = request.UserId;
            transaction.Type = TransactionType.AddAnswer;
            
            try
            {
                var question = await _quesRepository.GetQuestionByQuestionId(request.QuestionId);
                question.AnswerText = request.AnswerText;
                question.AnswerDate = DateTime.UtcNow;
                await _question.Update(question);
                transaction.Success = TransactionSuccess.Successful;
                await _transaction.Add(transaction);
                await _unit.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            catch (Exception)
            {
                transaction.Success = TransactionSuccess.Unsuccessful;
                await _transaction.Add(transaction);
                await _unit.SaveChangesAsync(cancellationToken);
                throw new Exception("AddListingAnswer error");
            }
        }
    }
}
