using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Accounts;
using WSantosDev.EventSourcing.Accounts.Commands;

namespace WSantosDev.EventSourcing.WebApi.Accounts
{
    [Tags("Account")]
    [Route("api/Account")]
    [ApiController]
    public class CreditController(Credit command) : ControllerBase
    {
        [HttpPost("Deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Credit(CreditRequest request)
        {
            var credited = await command.ExecuteAsync(new CreditParams(Constants.DefaultAccountId, request.Amount));
            if (credited)
                return Ok();

            if (credited.ErrorValue is InvalidAmountError)
                return BadRequest($"Invalid amount. The amount should be greater than zero.");

            if (credited.ErrorValue is StorageUnavailableError)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Storage unavailable.");

            return StatusCode(StatusCodes.Status500InternalServerError, "Unspecified error.");
        }
    }

    public record struct CreditRequest(decimal Amount);
}
