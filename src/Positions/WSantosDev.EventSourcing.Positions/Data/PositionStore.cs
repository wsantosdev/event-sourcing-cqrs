using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions
{
    public class PositionStore(EventStore eventStore) : IPositionStore
    {
        public Option<Position> GetBySymbol(AccountId accountId, Symbol symbol)
        {
            var stream = eventStore.Load(StreamId(accountId, symbol));

            return stream.Any()
                ? Position.Restore(stream)
                : Option.None<Position>();
        }

        public async Task<Result<IError>> StoreAsync(Position position) =>
            await eventStore.AppendAsync(StreamId(position.AccountId, position.Symbol), position.UncommittedEvents);
            
        private static string StreamId(AccountId accountId, Symbol symbol) =>
            $"Position_{accountId}_{symbol}";
    }
}
