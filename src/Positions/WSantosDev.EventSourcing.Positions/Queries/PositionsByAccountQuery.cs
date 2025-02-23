using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions.Queries
{
    public class PositionsByAccountQuery(IPositionReadModelStore readModelStore)
    {
        public IEnumerable<PositionReadModel> Execute(PositionsByAccountQueryParams queryParams) =>
            readModelStore.GetByAccount(queryParams.AccountId);
    }

    public record PositionsByAccountQueryParams(AccountId AccountId);
}
