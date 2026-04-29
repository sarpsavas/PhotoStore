using Core.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Listings.GetListingImage
{
    public class GetListingImageQueryHandler : IRequestHandler<GetListingImageQuery, List<string>>
    {
        private readonly IImageRepository _imRepository;

        public GetListingImageQueryHandler(IImageRepository imRepository)
        {
            _imRepository = imRepository;
        }

        public async Task<List<string>> Handle(GetListingImageQuery request, CancellationToken cancellationToken)
        {
            var response = await _imRepository.GetImagesUrlByListingIdAsync(request.ListingId);
            return response;
        }
    }
}
