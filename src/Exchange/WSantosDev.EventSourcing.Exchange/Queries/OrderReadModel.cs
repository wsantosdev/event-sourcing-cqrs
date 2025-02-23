namespace WSantosDev.EventSourcing.Exchange
{
    public record OrderReadModel(Guid AccountId, Guid OrderId, string Side,
                                 int Quantity, string Symbol, decimal Price, string Status);
}
