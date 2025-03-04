using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Exchange;
using WSantosDev.EventSourcing.Exchange.Commands;

namespace WSantosDev.EventSourcing.WebApi.Exchange
{
    [Tags("Exchange")]
    [Route("api/Exchange")]
    [ApiController]
    public class ExecuteController(Execute action) : ControllerBase
    {
        [HttpPost("Execute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Execute(ExecuteRequest command)
        {
            var executed = await action.ExecuteAsync(new ExecuteActionParams(command.OrderId));
            if (executed)
                return Ok();
            
            return executed.ErrorValue switch
            {
                OrderNotFoundError => NotFound("Order not found."),
                AlreadyFilledError => Conflict("Order already executed."),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Unspecified error.")
            };
        }
    }

    public record ExecuteRequest(Guid OrderId);
}
