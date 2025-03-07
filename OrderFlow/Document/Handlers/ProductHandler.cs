using OrderFlow.Document.Models.Request;
using OrderFlow.Document.Repositories;
using OrderFlow.Handlers;
using OrderFlow.Models;

namespace OrderFlow.Document.Handlers;

public class ProductHandler(Validator validator, IProductRepository repository)
{
    public async Task<OperationResult<Models.Product>> Create(CreateProductRequest request)
    {
        validator.Validate(request.Price, price => price >= 1, "Цена не может быть меньше 1")
            .Validate(request.Article, article => !string.IsNullOrEmpty(article), "Артикул не может быть пустым")
            .Validate(request.Title, title => !string.IsNullOrEmpty(title), "Заголовок не может быть пустым");

        if (!validator.IsValid())
            return new OperationResult<Models.Product>
            {
                Error = string.Join(", ", validator.GetErrors()),
                StatusCode = 422
            };

        return await repository.AddProductAsync(request);
    }

    public async Task<OperationResult<Models.Product>> Update(UpdateProductRequest request)
    {
        validator.Validate(request.Price, price => price >= 1, "Цена не может быть меньше 1")
            .Validate(request.Article, article => !string.IsNullOrEmpty(article), "Артикул не может быть пустым")
            .Validate(request.Title, title => !string.IsNullOrEmpty(title), "Заголовок не может быть пустым");

        if (!validator.IsValid())
            return new OperationResult<Models.Product>
            {
                Error = string.Join(", ", validator.GetErrors()),
                StatusCode = 422
            };

        var product = new Models.Product
        {
            Id = request.Id,
            Title = request.Title,
            Price = request.Price,
            Article = request.Article
        };

        return await repository.UpdateProductAsync(product);
    }

    public async Task<OperationResult<Models.Product>> Get(GetProductQueryRequest request)
    {
        if (!string.IsNullOrEmpty(request.Article)) return await repository.GetProductByArticle(request.Article);
        if (request.Id > 0) return await repository.GetProductById(request.Id);
        return new OperationResult<Models.Product>
        {
            Error = "Заказ не найден",
            StatusCode = 404
        };
    }

    public async Task<OperationResult<IEnumerable<Models.Product>>> Get()
    {
        return await repository.GetAllProductsAsync();
    }

    public async Task<OperationResult<string>> Delete(DeleteProductQueryRequest request)
    {
        return await repository.RemoveProductByIdAsync(request.Id);
    }
}