using OrderFlow.Document.Models;
using OrderFlow.Models;

namespace OrderFlow.Document.Repositories;

public interface IOrderRepository
{
    public Task<OperationResult<Order>> CreateAsync();
    public Task<OperationResult<Order>> UpdateAsync(Order order);
    public Task<OperationResult<Order>> GetByIdAsync(int id);
}