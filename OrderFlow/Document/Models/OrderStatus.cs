namespace OrderFlow.Document.Models;

public enum OrderStatus
{
    Opened,
    AwaitingPayment,
    Paid,
    Canceled,
    Completed,
}