namespace WSantosDev.EventSourcing.Exchange
{
    public record ExchangeOrderReadModel(Guid AccountId, Guid OrderId, string Side,
                                         int Quantity, string Symbol, decimal Price, string Status);
}
