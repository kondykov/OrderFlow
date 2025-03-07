using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Document.Handlers;
using OrderFlow.Document.Models.Request;

namespace OrderFlow.Document.Controllers;

[Authorize]
[Route("order-item")]
public class OrderItemController(OrderItemHandler handler) : Controller
{
    [HttpGet]
    [Route("get-all")]
    public async  Task<IActionResult> GetAll(int orderId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var operationResult = await handler.GetByOrderIdAsync(orderId);
        return StatusCode(operationResult.StatusCode, operationResult);
    }

    [HttpPost]
    [Route("add-or-update")]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrderItemRequest request)
    {
        var operationResult = await handler.CreateOrUpdateOrderItem(request);
        return StatusCode(operationResult.StatusCode, operationResult);
    }
}