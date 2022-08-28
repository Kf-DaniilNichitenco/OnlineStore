namespace Order.Domain.Enums
{
    public enum OrderStatus
    {
        PaymentAwaiting,
        PaymentCanceled,
        GoodsCollection,
        GoodsSent,
        GoodsArrived,
        OrderCompleted,
        OrderAborted
    }
}
