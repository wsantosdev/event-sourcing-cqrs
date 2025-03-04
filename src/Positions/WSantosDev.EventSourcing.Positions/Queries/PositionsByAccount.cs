using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions.Queries
{
    public class PositionsByAccount(IPositionReadModelStore readModelStore)
    {
        public async Task<IEnumerable<PositionReadModel>> ExecuteAsync(PositionsByAccountParams queryParams) =>
            await readModelStore.GetByAccountAsync(queryParams.AccountId);
    }

    public record PositionsByAccountParams(AccountId AccountId);
}
