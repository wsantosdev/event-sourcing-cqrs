using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions.Queries
{
    public class PositionsByAccountQuery(IPositionReadModelStore readModelStore)
    {
        public async Task<IEnumerable<PositionReadModel>> ExecuteAsync(PositionsByAccountQueryParams queryParams) =>
            await readModelStore.GetByAccountAsync(queryParams.AccountId);
    }

    public record PositionsByAccountQueryParams(AccountId AccountId);
}
