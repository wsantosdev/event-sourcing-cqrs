using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Orders.Queries;

namespace WSantosDev.EventSourcing.WebApi.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(OrdersByAccount query) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByAccount() =>
            Ok(await query.ExecuteAsync(new OrdersByAccountParams(Constants.DefaultAccountId)));
    }
}
