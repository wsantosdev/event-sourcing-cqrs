using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Accounts.Queries;

namespace WSantosDev.EventSourcing.WebApi.Accounts
{
    [Tags("Account")]
    [Route("api/Account")]
    [ApiController]
    public class BalanceController(AccountById query) : ControllerBase
    {
        [HttpGet("Balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Balance()
        {
            var stored = await query.ExecuteAsync(new AccountByIdParams(Constants.DefaultAccountId));
            return stored
                    ? Ok(stored.Get().Balance)
                    : NotFound();
        }
    }
}
