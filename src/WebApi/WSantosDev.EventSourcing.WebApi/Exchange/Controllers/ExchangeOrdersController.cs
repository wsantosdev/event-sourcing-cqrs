using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Exchange.Queries;

namespace WSantosDev.EventSourcing.WebApi.Exchange
{
    [Tags("Exchange")]
    [Route("api/Exchange")]
    [ApiController]
    public class ExchangeOrdersController(ListExchangeOrders query) : ControllerBase
    {
        [HttpGet("Orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get() =>
            Ok(await query.ExecuteAsync());
    }
}
