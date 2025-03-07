using OrderFlow.Document.Models;
using OrderFlow.Document.Models.Request;
using OrderFlow.Document.Repositories;
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
        if (!await userService.HasRoleAsync(new Terminal()))
            return new OperationResult<Order>
            {
                StatusCode = 403,
                Error = "Вам недостаточно привилегий для создания заказа"
            };
        return await repository.CreateAsync();
    }

    public async Task<OperationResult<Order>> Get(int orderId)
    {
        return await repository.GetByIdAsync(orderId);
    }

    public async Task<OperationResult<Order>> Update(UpdateOrderRequest request)
    {
        if (!await userService.HasRoleAsync(new Manager()))
            return new OperationResult<Order>
            {
                StatusCode = 403,
                Error = "Вам недостаточно привилегий для обновления заказа"
            };

        var orderOperationResult = await repository.GetByIdAsync(request.OrderId);
        if (!orderOperationResult.IsSuccessful)
            return new OperationResult<Order>
            {
                StatusCode = 404,
                Error = "Заказ не найден"
            };

        if (orderOperationResult.Data.Status is OrderStatus.Canceled)
            return new OperationResult<Order>
            {
                StatusCode = 400,
                Error = "Вы не можете изменить статус отменённого заказа заказа"
            };

        if (Enum.TryParse<OrderStatus>(request.Status, true, out var orderStatus))
        {
            if (orderOperationResult.Data.Status is OrderStatus.Canceled)
                return new OperationResult<Order>
                {
                    StatusCode = 403,
                    Error = "Невозможно изменить статус отменённого заказа"
                };
            orderOperationResult.Data.Status = orderStatus;
        }
        else
        {
            return new OperationResult<Order>
            {
                StatusCode = 422,
                Error = "Передан неверный статус"
            };
        }

        return await repository.UpdateAsync(orderOperationResult.Data);
    }
}