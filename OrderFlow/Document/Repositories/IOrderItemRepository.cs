using OrderFlow.Document.Models;
using OrderFlow.Document.Models.Request;
using OrderFlow.Models;

namespace OrderFlow.Document.Repositories;

public interface IOrderItemRepository
{
    public Task<OperationResult<OrderItem>> AddAsync(OrderItem request);
    public Task<OperationResult<OrderItem>> UpdateAsync(OrderItem orderItem);
    public Task<OperationResult<IEnumerable<OrderItem>>> GetAllItemsByOrderIdAsync(int orderId);
    public Task<OperationResult<OrderItem>> GetByOrderIdAndProductId(int orderId, int productId);
    public Task<OperationResult<OrderItem>> GetItemByIdAsync(int id);
}