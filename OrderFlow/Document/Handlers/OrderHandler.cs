using Microsoft.AspNetCore.Identity;
using OrderFlow.Document.Models;
using OrderFlow.Document.Models.Request;
using OrderFlow.Document.Repositories;
using OrderFlow.Identity.Models;
using OrderFlow.Identity.Models.Roles;
using OrderFlow.Identity.Services;
using OrderFlow.Models;

namespace OrderFlow.Document.Handlers;

public class OrderHandler(
    IOrderRepository repository,
    UserService userService,
    ILogger<OrderHandler> logger)
{
    public async Task<OperationResult<Order>> Create()
    {
        return await repository.CreateAsync();
    }

    public async Task<OperationResult<Order>> Get(int orderId)
    {
        return await repository.GetByIdAsync(orderId);
    }

    public async Task<OperationResult<Order>> Update(UpdateOrderRequest request)
    {
        if (!await userService.HasAnyRoleAsync([new Admin()]))
            return new OperationResult<Order>()
            {
                StatusCode = 403,
                Error = "You do not have permission to update this order",
            };
        
        var order = await repository.GetByIdAsync(request.OrderId);
        if (!order.IsSuccessful)
            return new OperationResult<Order>
            {
                StatusCode = 404,
                Error = "Order not found!"
            };
        if (Enum.TryParse<OrderStatus>(request.Status, true, out var orderStatus)) order.Data.Status = orderStatus;
        else
            return new OperationResult<Order>
            {
                StatusCode = 422,
                Error = "Invalid status!"
            };

        return await repository.UpdateAsync(order.Data);
    }
}