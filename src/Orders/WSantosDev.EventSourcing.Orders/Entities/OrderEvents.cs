﻿using WSantosDev.EventSourcing.Commons;
using WSantosDev.EventSourcing.Commons.Modeling;

namespace WSantosDev.EventSourcing.Orders
{
    public sealed partial class Order
    {
        private record struct OrderCreated(int Id, AccountId AccountId, OrderId OrderId, string Side,
                                           int Quantity, string Symbol, decimal Price) : IEvent;

        private record struct OrderExecuted(int Id, AccountId AccountId, OrderId OrderId, string Side,
                                            int Quantity, string Symbol, decimal Price) : IEvent;
    }
}
