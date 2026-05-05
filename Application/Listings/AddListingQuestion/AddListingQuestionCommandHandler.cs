using Core.Abstractions.Repositories;
using Core.Abstractions.UnitOfWork;
using Core.Entities;
using Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.AddListingQuestion
{
    public class AddListingQuestionCommandHandler : IRequestHandler<AddListingQuestionCommand,Unit>
    {
        private readonly IRepository<Transaction> _repositoryTran;
        private readonly IRepository<Question> _repositoryQuestion;
        private readonly IUnitOfWork _unit;


        public AddListingQuestionCommandHandler(IRepository<Transaction> repositoryTran,
           IRepository<Question> repositoryQuestion,
           IUnitOfWork unit)
        {
            _repositoryTran = repositoryTran;
            _repositoryQuestion = repositoryQuestion;
            _unit = unit;
        }

        public async Task<Unit> Handle(AddListingQuestionCommand request, CancellationToken cancellationToken)
        {
            Transaction transaction = new Transaction();
            transaction.Description = null;
            transaction.UserId = request.UserId;
            transaction.Type = TransactionType.AddQuestion;

            try
            {
                Question question = new Question();
                question.QuestionText = request.QuestionText;
                question.ListingId = request.ListingId;
                question.UserId = request.UserId;

                await _repositoryQuestion.Add(question);

                transaction.Success = TransactionSuccess.Successful;
                await _repositoryTran.Add(transaction); 
                await _unit.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception)
            {

                transaction.Success = TransactionSuccess.Unsuccessful;
                await _repositoryTran.Add(transaction);

                await _unit.SaveChangesAsync(cancellationToken);


                throw new Exception("unsuccessful [add question]");
            }
            

        }
    }
    
}
