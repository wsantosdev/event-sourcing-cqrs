using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Orders.Queries;

namespace WSantosDev.EventSourcing.WebApi.Orders.Actions
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(OrdersByAccountQuery query) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetByAccount()
        {
            var orders = query.Execute(new OrdersByAccountQueryParams(Constants.DefaultAccountId));
            return Ok(orders);
        }
    }
}
