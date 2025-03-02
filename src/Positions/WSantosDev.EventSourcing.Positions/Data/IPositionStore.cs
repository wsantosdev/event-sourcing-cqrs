using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions
{
    public interface IPositionStore
    {
        Option<Position> GetBySymbol(AccountId accountId, Symbol symbol);
        Task<Result<IError>> StoreAsync(Position position);
    }
}
