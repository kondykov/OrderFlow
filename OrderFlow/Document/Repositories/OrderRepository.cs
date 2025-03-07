using Microsoft.EntityFrameworkCore;
using OrderFlow.Data;
using OrderFlow.Document.Models;
using OrderFlow.Models;

namespace OrderFlow.Document.Repositories;

public class OrderRepository(DataContext context) : IOrderRepository
{
    public async Task<OperationResult<Order>> CreateAsync()
    {
        var order = new Order();
        context.Orders.Add(order);
        await context.SaveChangesAsync();
        return new OperationResult<Order>
        {
            StatusCode = 200
        };
    }

    public async Task<OperationResult<Order>> UpdateAsync(Order order)
    {
        context.Orders.Update(order);
        await context.SaveChangesAsync();
        return new OperationResult<Order>
        {
            StatusCode = 200,
            Data = order
        };
    }

    public async Task<OperationResult<Order>> GetByIdAsync(int id)
    {
        var order = context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefault(o => o.Id == id);
        if (order == null)
            return new OperationResult<Order>
            {
                StatusCode = 404,
                Error = "Заказ не найден"
            };
        
        return new OperationResult<Order>
        {
            StatusCode = 200,
            Data = order
        };
    }
}