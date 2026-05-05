using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Listings.AddListing;
using Application.Listings.ViewAllListings;
using Application.Listings.ViewUserListings;
using Application.Listings.UpdateListing;
using Application.Listings.AddListingQuestion;
using Application.Listings.DeleteListing;
using Application.Listings.AddListingAnswer;
using Core.Abstractions.Services;
using Microsoft.AspNetCore.Http;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class ListingController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IWebHostEnvironment _env;
        private readonly IAddListingImageService _imageService;

        public ListingController(ISender sender,
            IWebHostEnvironment env,
            IAddListingImageService imageService)
        {
            _sender = sender;
            _env = env;
            _imageService = imageService;
        }

        [HttpPost("listing/add")]
        public async Task<ActionResult> AddListingAsync([FromBody] AddListingCommand request, CancellationToken cancellationToken )
        {
            try
            {
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }

        [HttpPatch("listing/update")]
        public async Task<ActionResult> UpdateListingAsync([FromBody] UpdateListingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }

        [HttpDelete("listing/delete")]
        public async Task<ActionResult> DeleteListingAsync([FromBody] DeleteListingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }

        [HttpGet("listing/all-listings")]
        public async Task<ActionResult> GetAllListingAsync([FromQuery] ViewAllListingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }

        [HttpGet("listing/user-listings")]
        public async Task<ActionResult> GetUserListingAsync([FromQuery] ViewUserListingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }
        [HttpPost("listing/add-question")]
        public async Task<ActionResult> AddListinQuestion([FromBody] AddListingQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }

        [HttpPatch("listing/add-answer")]
        public async Task<ActionResult> AddListinAnswer([FromBody] AddListingAnswerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException?.Message);
            }
        }

        [HttpPost("listing/add-image")]
        public async Task<ActionResult> AddListingImage(IFormFile file, string listingId)
        {
            try
            {
                if (file == null || file.Length == 0) return BadRequest("Dosya boş");

                if (!file.ContentType.StartsWith("image/")) return BadRequest("Sadece image yüklenebilir");

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                var folderPath = Path.Combine(_env.WebRootPath, "images");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                
                var url = $"/images/{fileName}";
                await _imageService.SaveImage(listingId,url);

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //TODO:[HttpGet("listing/add-imgurl")]


    }
}
