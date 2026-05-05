using Application.Listings.ViewAllListings;
using Application.Transactions.GetAllUserTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ISender _sender;
        public TransactionController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("transaction/user-transactions")]
        public async Task<ActionResult> GetAllUserTransactionsAsync([FromQuery] GetAllUserTransactionsQuery request, CancellationToken cancellationToken)
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
