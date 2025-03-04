using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions.Queries
{
    public class PositionBySymbol(IPositionReadModelStore readModelStore)
    {
        public async Task<Option<PositionReadModel>> ExecuteAsync(PositionBySymbolParams queryParams) =>
            await readModelStore.GetBySymbolAsync(queryParams.AccountId, queryParams.Symbol);
    }

    public record PositionBySymbolParams(AccountId AccountId, Symbol Symbol);
}
