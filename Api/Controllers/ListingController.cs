using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Listings.AddListing;
using Application.Listings.ViewAllListings;
using Application.Listings.ViewUserListings;
using Application.Listings.UpdateListing;
using Application.Listings.AddListingQuestion;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class ListingController : ControllerBase
    {
        private readonly ISender _sender;

        public ListingController(ISender sender)
        {
            _sender = sender;
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

        [HttpPost("listing/update")]
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

        [HttpGet("listing/all-listings")]
        public async Task<ActionResult> GetAllListingAsync([FromQuery] ViewAllListingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("listing/user-listings")]
        public async Task<ActionResult> GetUserListingAsync([FromBody] ViewUserListingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _sender.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
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
    }
}
