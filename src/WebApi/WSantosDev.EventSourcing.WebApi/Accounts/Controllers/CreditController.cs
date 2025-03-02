using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Accounts;
using WSantosDev.EventSourcing.Accounts.Actions;

namespace WSantosDev.EventSourcing.WebApi.Accounts
{
    [Tags("Account")]
    [Route("api/Account")]
    [ApiController]
    public class CreditController(CreditAction action) : ControllerBase
    {
        [HttpPost("Credit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Credit(CreditRequest request)
        {
            var credited = await action.ExecuteAsync(new CreditActionParams(Constants.DefaultAccountId, request.Amount));
            if (credited)
                return Ok();

            if (credited.ErrorValue is InvalidAmountError)
                return BadRequest($"Invalid amount. The amount should be greater than zero.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Unspecified error.");
        }
    }

    public record struct CreditRequest(decimal Amount);
}
