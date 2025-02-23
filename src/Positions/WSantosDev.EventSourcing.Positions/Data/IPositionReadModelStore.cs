using Moonad;
using WSantosDev.EventSourcing.Commons;

namespace WSantosDev.EventSourcing.Positions
{
    public interface IPositionReadModelStore
    {
        IEnumerable<PositionReadModel> GetByAccount(AccountId accountId);
        Option<PositionReadModel> GetBySymbol(AccountId accountId, Symbol symbol);

        void Add(PositionReadModel position);

        void Update(PositionReadModel position);

        void Remove(PositionReadModel position);
    }
}
