using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Positions.Queries;

namespace WSantosDev.EventSourcing.WebApi.Positions
{
    [Tags("Positions")]
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController(PositionsByAccountQuery query) : ControllerBase
    {
        [HttpGet]
        public IActionResult List()
        {
            var positions = query.Execute(new PositionsByAccountQueryParams(Constants.DefaultAccountId));
            return Ok(positions);
        }
    }
}
