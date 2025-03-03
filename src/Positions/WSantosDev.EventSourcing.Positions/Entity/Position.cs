﻿using Moonad;
using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Positions
{
    public sealed partial class Position : EventBasedEntity
    {
        public AccountId AccountId { get; private set; } = AccountId.Empty;
        public Symbol Symbol { get; private set; } = Symbol.Empty;
        public Quantity Available { get; private set; } = Quantity.Zero;
        
        private Position(AccountId accountId, Symbol symbol) =>
            RaiseEvent(new PositionOpened(accountId, symbol));

        public static Result<Position,IError> Open(AccountId accountId, Symbol symbol, Quantity quantity)
        {
            if(accountId == AccountId.Empty)
                return Errors.EmptyAccountId;
            if (symbol == Symbol.Empty)
                return Errors.EmptySymbol;
            if (quantity == Quantity.Zero)
                return Errors.QuantityZero;

            var position = new Position(accountId, symbol);
            position.Deposit(quantity);
            
            return position;
        }

        private Position(IEnumerable<IEvent> events) =>
            Hydrate(events);

        private void Apply(PositionOpened @event) =>
            (AccountId, Symbol, Available) = (@event.AccountId, @event.Symbol, Available);

        public static Position Restore(IEnumerable<IEvent> events) =>
            new (events);

        public Result<IError> Deposit(Quantity quantity)
        {
            if (quantity == Quantity.Zero)
                return Result<IError>.Error(Errors.QuantityZero);

            RaiseEvent(new SharesDeposited(quantity));
            return true;
        }

        private void Apply(SharesDeposited @event) =>
            Available += @event.Quantity;
                
        public Result<IError> Withdraw(Quantity quantity)
        {
            if (quantity == Quantity.Zero)
                return Errors.QuantityZero;
            if (Available - quantity < Quantity.Zero)
                return Errors.InsuficientShares;

            if (Available > Quantity.Zero)
                RaiseEvent(new SharesWithdrawn(quantity));
            else
                RaiseEvent(new PositionClosed(quantity));

            return true;
        }

        private void Apply(SharesWithdrawn @event) =>
            Available -= @event.Quantity;

        private void Apply(PositionClosed @event) =>
            Available -= @event.Quantity;

        protected override void ProcessEvent(IEvent @event)
        {
            switch (@event)
            {
                case PositionOpened positionCreated: 
                    Apply(positionCreated); break;
                case SharesDeposited sharesDeposited: 
                    Apply(sharesDeposited); break;
                case SharesWithdrawn sharesWithdrawn:
                    Apply(sharesWithdrawn); break;
                case PositionClosed positionClosed:
                    Apply(positionClosed); break;
            }
        }
    }
}
