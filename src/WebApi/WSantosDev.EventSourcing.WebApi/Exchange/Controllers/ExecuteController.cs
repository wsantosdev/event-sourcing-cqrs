using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Exchange;
using WSantosDev.EventSourcing.Exchange.Actions;

namespace WSantosDev.EventSourcing.WebApi.Exchange
{
    [Tags("Exchange")]
    [Route("api/Exchange")]
    [ApiController]
    public class ExecuteController(ExecuteAction action) : ControllerBase
    {
        [HttpPost("Execute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Execute(ExecuteRequest command)
        {
            var executed = action.Execute(new ExecuteActionParams(command.OrderId));
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
