using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Document.Handlers;
using OrderFlow.Document.Models.Request;

namespace OrderFlow.Document.Controllers;

[Authorize]
[Route("order")]
public class OrderController(
    OrderHandler orderHandler
    ) : Controller
{
    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> Get([FromQuery] int orderId)
    {
        var operationResult = await orderHandler.Get(orderId);
        return StatusCode(operationResult.StatusCode, operationResult);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create()
    {
        var operationResult = await orderHandler.Create();
        return StatusCode(operationResult.StatusCode, operationResult);
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update([FromBody] UpdateOrderRequest request)
    {
        var operationResult = await orderHandler.Update(request);
        return StatusCode(operationResult.StatusCode, operationResult);
    }
}