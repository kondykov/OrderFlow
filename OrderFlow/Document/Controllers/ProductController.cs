using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Document.Handlers;
using OrderFlow.Document.Models.Request;

namespace OrderFlow.Document.Controllers;

[Authorize]
[Route("product")]
public class ProductController(
    ProductHandler productHandler
    ) : Controller
{
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        if (!ModelState.IsValid) return UnprocessableEntity($"Необрабатываемая сущность: {ModelState}");
        var operationResult = await productHandler.Create(request);
        return StatusCode(operationResult.StatusCode, operationResult);
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
    {
        if (!ModelState.IsValid) return UnprocessableEntity($"Необрабатываемая сущность: {ModelState}");
        var operationResult = await productHandler.Update(request);
        return StatusCode(operationResult.StatusCode, operationResult);
    }

    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetById([FromQuery] GetProductQueryRequest request)
    {
        var operationResult = await productHandler.Get(request);
        return StatusCode(operationResult.StatusCode, operationResult);
    }
    
    [HttpGet]
    [Route("get-all")]
    public async Task<IActionResult> GetById()
    {
        var operationResult = await productHandler.Get();
        return StatusCode(operationResult.StatusCode, operationResult);
    }
    
    // [HttpDelete]
    // [Route("delete")]
    // public async Task<IActionResult> Delete([FromQuery] DeleteProductQueryRequest request)
    // {
    //     if (!ModelState.IsValid) return UnprocessableEntity("Unprocessable entity");
    //     var operationResult = await productHandler.Delete(request);
    //     return StatusCode(operationResult.StatusCode, operationResult);
    // }
}