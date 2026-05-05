using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Core.Abstractions.UnitOfWork;
using Core.Entities;
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
        private readonly IUnitOfWork _unit;

        public AddListingImageService(IRepository<Image> repositoryIm,
            IUnitOfWork unit)
        {
            _repositoryIm = repositoryIm;
            _unit = unit;
        }

        public async Task SaveImage(string listingId, string url)
        {
            Image image = new Image();
            await _repositoryIm.Add(image);
        }
    }
}
