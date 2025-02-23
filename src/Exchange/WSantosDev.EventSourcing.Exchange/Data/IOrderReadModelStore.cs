namespace WSantosDev.EventSourcing.Exchange
{
    public interface IOrderReadModelStore
    {
        public IEnumerable<OrderReadModel> GetAll();

        public void Store(OrderReadModel order);
        public void Update(OrderReadModel order);
    }
}
