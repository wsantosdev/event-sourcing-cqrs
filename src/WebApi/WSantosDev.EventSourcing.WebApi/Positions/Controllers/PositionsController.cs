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
        public async Task<IActionResult> List() =>
            Ok(await query.ExecuteAsync(new PositionsByAccountQueryParams(Constants.DefaultAccountId)));
    }
}
