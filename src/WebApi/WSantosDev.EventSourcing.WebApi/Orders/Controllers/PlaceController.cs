using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WSantosDev.EventSourcing.Accounts;
using WSantosDev.EventSourcing.Accounts.Queries;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Orders;
using WSantosDev.EventSourcing.Orders.Actions;
using WSantosDev.EventSourcing.Positions;
using WSantosDev.EventSourcing.Positions.Queries;

namespace WSantosDev.EventSourcing.WebApi.Orders
{
    [Tags("Orders")]
    [Route("api/Orders")]
    [ApiController]
    public class PlaceController(AccountById accountQuery,
                                 PositionBySymbol positionBySymbolQuery,
                                 Place action) : ControllerBase
    {
        [HttpPost("Place")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Place(PlaceOrderRequest request)
        {
            if (request.Side == OrderSide.Buy)
            {
                var account = await accountQuery.ExecuteAsync(new AccountByIdParams(Constants.DefaultAccountId));
                if (!account)
                    return NotFound();

                if(account.Get().Balance < request.Quantity * request.Price)
                    return Conflict("Insuficient funds.");
            }
            
            if(request.Side == OrderSide.Sell)
            {
                var position = await positionBySymbolQuery.ExecuteAsync(new PositionBySymbolParams(Constants.DefaultAccountId, request.Symbol));
                if(!position || position.Get().Available < request.Quantity)
                    return Conflict("Insuficient shares.");
            }

            var created = await action.ExecuteAsync(new PlaceParams(Constants.DefaultAccountId, OrderId.New(), request.Side, 
                                                                    request.Quantity, request.Symbol, request.Price));
            if (created)
                return Created(created.ResultValue.OrderId.ToString(), created.ResultValue.OrderId);

            return created.ErrorValue switch
            {
                InvalidSideError => BadRequest("Invalid side. Side allowed: 'Buy' ou 'Sell'."),
                InvalidQuantityError => BadRequest("Invalid quantity. Quantity should be greater than zero."),
                InvalidSymbolError => BadRequest("Invalid symbol. Symbol cannot be empty."),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Unspecified error.")
            };
        }
    }

    public class PlaceOrderRequest
    {
        [DefaultValue("Buy")]
        [RegularExpression("Buy|Sell")]
        public required string Side { get; set; }
        [DefaultValue(100)]
        public int Quantity { get; set; }
        [DefaultValue("MODU3")]
        public required string Symbol { get; set; }
        [DefaultValue(10)]
        public decimal Price { get; set; }
    }
}
