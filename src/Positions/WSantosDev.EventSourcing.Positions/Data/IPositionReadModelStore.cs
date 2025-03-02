using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions
{
    public interface IPositionReadModelStore
    {
        Task<IEnumerable<PositionReadModel>> GetByAccountAsync(AccountId accountId);
        Task<Option<PositionReadModel>> GetBySymbolAsync(AccountId accountId, Symbol symbol);

        Task StoreAsync(PositionReadModel position);

        Task RemoveAsync(PositionReadModel position);
    }
}
