using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Accounts;
using WSantosDev.EventSourcing.Accounts.Commands;

namespace WSantosDev.EventSourcing.WebApi.Accounts
{
    [Tags("Account")]
    [Route("api/Account")]
    [ApiController]
    public class DebitController(Debit command) : ControllerBase
    {
        [HttpPost("Debit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Debit(DebitRequest request)
        {
            var debited = await command.ExecuteAsync(new DebitParams(Constants.DefaultAccountId, request.Amount));
            if (debited)
                return Ok();

            return debited.ErrorValue switch
            {
                InvalidAmountError => BadRequest($"Invalid amount. The amount should be greater than zero."),
                StorageUnavailableError => StatusCode(StatusCodes.Status500InternalServerError, $"Storage unavailable."),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Unspecified error."),
            };
        }
    }

    public record struct DebitRequest(decimal Amount);
}
