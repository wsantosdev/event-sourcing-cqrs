using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions.Queries
{
    public class PositionsByAccount(PositionViewDbContext dbContext)
    {
        public async Task<IEnumerable<PositionView>> ExecuteAsync(PositionsByAccountParams queryParams, CancellationToken cancellationToken = default) =>
            await dbContext.ByAccountIdAsync(queryParams.AccountId, cancellationToken);
    }

    public record PositionsByAccountParams(AccountId AccountId);
}
