using OrderFlow.Document.Models;
using OrderFlow.Document.Models.Request;
using OrderFlow.Document.Repositories;
using OrderFlow.Handlers;
using OrderFlow.Models;

namespace OrderFlow.Document.Handlers;

public class OrderItemHandler(
    IOrderItemRepository orderItemRepository,
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    Validator validator)
{
    public async Task<OperationResult<OrderItem>> CreateOrUpdateOrderItem(CreateOrderItemRequest request)
    {
        validator.Validate(request.Quantity, quantity => quantity < 0, "Количество не может быть меньше 0");

        if (!validator.IsValid())
            return new OperationResult<OrderItem>
            {
                Error = string.Concat(", ", validator.GetErrors()),
                StatusCode = 422
            };

        var orderOperationResult = await orderRepository.GetByIdAsync(request.OrderId);
        if (!orderOperationResult.IsSuccessful)
            return new OperationResult<OrderItem>
            {
                StatusCode = orderOperationResult.StatusCode,
                Error = orderOperationResult.Error
            };

        var productOperationResult = await productRepository.GetProductById(request.ProductId);
        if (!productOperationResult.IsSuccessful)
            return new OperationResult<OrderItem>
            {
                StatusCode = productOperationResult.StatusCode,
                Error = productOperationResult.Error
            };

        var product = productOperationResult.Data;
        var order = orderOperationResult.Data;

        var orderItemOperationResult = await orderItemRepository.GetByOrderIdAndProductId(order.Id, product.Id);
        var orderItem = orderItemOperationResult.Data;
        
        if (orderItemOperationResult.IsSuccessful)
        {
            orderItem.Quantity += request.Quantity;
            return await orderItemRepository.UpdateAsync(orderItem);
        }

        orderItem = new OrderItem
        {
            Order = order,
            Product = product,
            Quantity = request.Quantity
        };

        return await orderItemRepository.AddAsync(orderItem);
    }

    public async Task<OperationResult<IEnumerable<OrderItem>>> GetByOrderIdAsync(int orderId)
    {
        return await orderItemRepository.GetAllItemsByOrderIdAsync(orderId);
    }
}