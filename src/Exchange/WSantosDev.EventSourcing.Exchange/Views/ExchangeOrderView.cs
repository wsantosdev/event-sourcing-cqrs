namespace WSantosDev.EventSourcing.Exchange
{
    public record ExchangeOrderView(Guid AccountId, Guid OrderId, string Side,
                                         int Quantity, string Symbol, decimal Price, string Status);
}
