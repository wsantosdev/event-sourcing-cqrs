using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions.Queries
{
    public class PositionBySymbol(PositionViewDbContext dbContext)
    {
        public async Task<Option<PositionView>> ExecuteAsync(PositionBySymbolParams queryParams, CancellationToken cancellationToken = default) =>
            await dbContext.BySymbolAsync(queryParams.AccountId, queryParams.Symbol, cancellationToken);
    }

    public record PositionBySymbolParams(AccountId AccountId, Symbol Symbol);
}
