using OrderFlow.Document.Models;
using OrderFlow.Document.Models.Request;
using OrderFlow.Models;

namespace OrderFlow.Document.Repositories;

public interface IProductRepository
{
    public Task<OperationResult<Product>> AddProductAsync(CreateProductRequest request);
    public Task<OperationResult<Product>> UpdateProductAsync(Product product);
    public Task<OperationResult<Product>> GetProductById(int productId);
    public Task<OperationResult<Product>> GetProductByArticle(string article);
    public Task<OperationResult<IEnumerable<Product>>> GetAllProductsAsync();
    public Task<OperationResult<string>> RemoveProductByIdAsync(int productId);
}