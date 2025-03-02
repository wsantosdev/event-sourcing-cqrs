using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions.Queries
{
    public class PositionBySymbolQuery(IPositionReadModelStore readModelStore)
    {
        public async Task<Option<PositionReadModel>> ExecuteAsync(PositionBySymbolQueryParams queryParams) =>
            await readModelStore.GetBySymbolAsync(queryParams.AccountId, queryParams.Symbol);
    }

    public record PositionBySymbolQueryParams(AccountId AccountId, Symbol Symbol);
}
