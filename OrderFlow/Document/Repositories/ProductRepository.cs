using OrderFlow.Data;
using OrderFlow.Document.Models;
using OrderFlow.Document.Models.Request;
using OrderFlow.Models;

namespace OrderFlow.Document.Repositories;

public class ProductRepository(DataContext context) : IProductRepository
{
    public async Task<OperationResult<Product>> AddProductAsync(CreateProductRequest request)
    {
        var productExists = await GetProductByArticle(request.Article);
        if (productExists.IsSuccessful)
            return new OperationResult<Product>
            {
                Error = "Product article already exists",
                StatusCode = 409
            };

        context.Products.Add(new Product
        {
            Article = request.Article,
            Price = request.Price,
            Title = request.Title
        });
        await context.SaveChangesAsync();
        return new OperationResult<Product>
        {
            Data = (await GetProductByArticle(request.Article)).Data,
            StatusCode = 200
        };
    }

    public async Task<OperationResult<Product>> UpdateProductAsync(Product product)
    {
        var productExists = await GetProductByArticle(product.Article);
        if (!productExists.IsSuccessful)
            return new OperationResult<Product>
            {
                Error = "Product article does not exist",
                StatusCode = 404
            };
        context.Products.Update(product);
        await context.SaveChangesAsync();
        return new OperationResult<Product>
        {
            Data = product,
            StatusCode = 200
        };
    }

    public async Task<OperationResult<Product>> GetProductById(int productId)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == productId);
        return product != null
            ? new OperationResult<Product>
            {
                Data = product,
                StatusCode = 200
            }
            : new OperationResult<Product>
            {
                Error = "Product not found",
                StatusCode = 404
            };
    }

    public async Task<OperationResult<Product>> GetProductByArticle(string article)
    {
        var product = context.Products.FirstOrDefault(p => p.Article == article);
        return product != null
            ? new OperationResult<Product>
            {
                Data = product,
                StatusCode = 200
            }
            : new OperationResult<Product>
            {
                Error = "Product not found",
                StatusCode = 404
            };
    }

    public async Task<OperationResult<IEnumerable<Product>>> GetAllProductsAsync()
    {
        return new OperationResult<IEnumerable<Product>>()
        {
            StatusCode = 200,
            Data = context.Products.ToList()
        };
    }

    public async Task<OperationResult<string>> RemoveProductByIdAsync(int productId)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
            return new OperationResult<string>
            {
                Error = "Product not found",
                StatusCode = 404
            };
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return new OperationResult<string>
        {
            StatusCode = 200
        };
    }
}