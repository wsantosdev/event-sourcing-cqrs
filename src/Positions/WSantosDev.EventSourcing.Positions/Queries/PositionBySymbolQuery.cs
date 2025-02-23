using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions.Queries
{
    public class PositionBySymbolQuery(IPositionReadModelStore readModelStore)
    {
        public Option<PositionReadModel> Execute(PositionBySymbolQueryParams queryParams) =>
            readModelStore.GetBySymbol(queryParams.AccountId, queryParams.Symbol);
    }

    public record PositionBySymbolQueryParams(AccountId AccountId, Symbol Symbol);
}
