using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Listings.AddListing;
using Application.Listings.ViewAllListings;
using Application.Listings.ViewUserListings;


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

        [HttpGet("listing/all-listings")]
        public async Task<ActionResult> GetAllListingAsync([FromBody] ViewAllListingsQuery request, CancellationToken cancellationToken)
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

        [HttpPost("listing/user-listings")]
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
    }
}
