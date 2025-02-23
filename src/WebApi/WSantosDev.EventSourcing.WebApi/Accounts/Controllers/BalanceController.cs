using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Accounts.Queries;

namespace WSantosDev.EventSourcing.WebApi.Accounts
{
    [Tags("Account")]
    [Route("api/Account")]
    [ApiController]
    public class BalanceController(AccountQuery query) : ControllerBase
    {
        [HttpGet("Balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Balance()
        {
            var account = query.Execute(new AccountQueryParams(Constants.DefaultAccountId));
            return account
                    ? Ok(account.Get().Balance)
                    : NotFound();
        }
    }
}
