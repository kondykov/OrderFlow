using OrderFlow.Data;
using OrderFlow.Document.Models;
using OrderFlow.Models;

namespace OrderFlow.Document.Repositories;

public class OrderItemRepository(DataContext context) : IOrderItemRepository
{
    public async Task<OperationResult<OrderItem>> AddAsync(OrderItem orderItem)
    {
        if (orderItem.Order.Status != OrderStatus.Opened)
            return new OperationResult<OrderItem>
            {
                Data = orderItem,
                StatusCode = 400,
                Error = "Order is not opened."
            };

        var existsOrderItem =
            context.OrderItems.FirstOrDefault(o =>
                o.ProductId == orderItem.ProductId && o.OrderId == orderItem.OrderId);
        if (existsOrderItem != null) return await UpdateAsync(orderItem);

        context.OrderItems.Add(orderItem);
        await context.SaveChangesAsync();
        return new OperationResult<OrderItem>
        {
            Data = orderItem,
            StatusCode = 200
        };
    }

    public async Task<OperationResult<OrderItem>> UpdateAsync(OrderItem orderItem)
    {
        if (orderItem.Order.Status != OrderStatus.Opened)
            return new OperationResult<OrderItem>
            {
                Data = orderItem,
                StatusCode = 400,
                Error = "Order is not opened."
            };

        context.OrderItems.Update(orderItem);
        await context.SaveChangesAsync();
        return new OperationResult<OrderItem>
        {
            Data = orderItem,
            StatusCode = 200
        };
    }

    public async Task<OperationResult<IEnumerable<OrderItem>>> GetAllItemsByOrderIdAsync(int orderId)
    {
        var orderItems = context.OrderItems.Where(orderItem => orderItem.OrderId == orderId).ToList();

        return new OperationResult<IEnumerable<OrderItem>>
        {
            Data = orderItems,
            StatusCode = 200
        };
    }

    public async Task<OperationResult<OrderItem>> GetByOrderIdAndProductId(int orderId, int productId)
    {
        var orderItem = context.OrderItems.FirstOrDefault(o => o.OrderId == orderId && o.ProductId == productId);
        if (orderItem == null)
            return new OperationResult<OrderItem>
            {
                StatusCode = 404,
                Error = "Order item does not exist."
            };

        return new OperationResult<OrderItem>
        {
            Data = orderItem,
            StatusCode = 200
        };
    }

    public async Task<OperationResult<OrderItem>> GetItemByIdAsync(int id)
    {
        var orderItem = await context.OrderItems.FindAsync(id);
        if (orderItem == null)
            return new OperationResult<OrderItem>
            {
                StatusCode = 404,
                Error = "Order item does not exist."
            };

        return new OperationResult<OrderItem>
        {
            Data = orderItem,
            StatusCode = 200
        };
    }
}