using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Exchange.Queries;

namespace WSantosDev.EventSourcing.WebApi.Exchange
{
    [Tags("Exchange")]
    [Route("api/Exchange")]
    [ApiController]
    public class ExchangeOrdersController(ExchangeOrdersQuery query) : ControllerBase
    {
        [HttpGet("Orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            var orders = query.Execute();
            return Ok(orders);
        }
    }
}
