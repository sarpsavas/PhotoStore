using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Core.Abstractions.UnitOfWork;
using Core.Entities;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AddListingImage
{
    public class AddListingImageService : IAddListingImageService
    {
        private readonly IRepository<Image> _repositoryIm;
        private readonly IRepository<Transaction> _repositoryTr;
        private readonly IUnitOfWork _unit;

        public AddListingImageService(IRepository<Image> repositoryIm,
            IUnitOfWork unit,
            IRepository<Transaction> repositoryTr)
        {
            _repositoryIm = repositoryIm;
            _unit = unit;
            _repositoryTr = repositoryTr;
        }

        public async Task SaveImage(string userId, string listingId, string url)
        {

            Image image = new Image(Guid.Parse(listingId),url);
            await _repositoryIm.Add(image);

            Transaction transaction = new Transaction();
            transaction.UserId = Guid.Parse(userId);
            transaction.Success = TransactionSuccess.Successful;
            transaction.Type = TransactionType.AddListingImage;
            transaction.Description = "-";
            await _repositoryTr.Add(transaction);

            await _unit.SaveChangesAsync();

        }
    }
}
