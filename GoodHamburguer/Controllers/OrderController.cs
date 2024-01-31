using GoodHamburguer.Models;
using GoodHamburguer.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Controllers
{
    /// <summary>
    /// Controller responsible for operations related to orders.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly RuleService _ruleService;

        /// <summary>
        /// Constructor of the OrderController class.
        /// </summary>
        /// <param name="ruleService">Rule service.</param>
        public OrderController(RuleService ruleService)
        {
            _ruleService = ruleService ?? throw new ArgumentNullException(nameof(ruleService));
        }

        /// <summary>
        /// Gets a list of all available sandwiches and extras.
        /// </summary>
        /// <returns>A dictionary containing lists of sandwiches and extras.</returns>
        [HttpGet("get/all")]
        public ActionResult<Dictionary<string, List<string>>> ListAll()
        {
            try
            {
                var result = _ruleService.ListAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a list of all available sandwiches.
        /// </summary>
        /// <returns>A list of available sandwiches.</returns>
        [HttpGet("get/sandwiches")]
        public ActionResult<SandwichModel> ListSandwiches()
        {
            try
            {
                var result = _ruleService.ListSandwiches();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a list of all available extras.
        /// </summary>
        /// <returns>A list of available extras.</returns>
        [HttpGet("get/extras")]
        public ActionResult<ExtraModel> ListExtras()
        {
            try
            {
                var result = _ruleService.ListExtras();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new order based on the provided information.
        /// </summary>
        /// <remarks>
        /// Request example:
        ///
        ///     POST /api/order/post/order
        ///     {
        ///        "sandwich": "X Burger",
        ///        "extra": ["Fries", "Soft drink"]
        ///     }
        ///
        /// </remarks>
        /// <param name="orderItemModel">Order item model.</param>
        [HttpPost("post/order")]
        public IActionResult CadastrarPedido([FromBody] OrderItemModel orderItemModel)
        {
            if (orderItemModel == null || (orderItemModel.Sandwich == null && orderItemModel.Extra == null))
            {
                return BadRequest("O pedido não pode ser nulo e deve conter pelo menos um item (sanduíche ou extra).");
            }

            OrderItemFullModel orderItemFull = _ruleService.AmountCharged(orderItemModel);
            orderItemFull.Price = Math.Round(orderItemFull.Price.Value, 2);

            return Ok(orderItemFull);
        }

        /// <summary>
        /// Gets the current order list model.
        /// </summary>
        /// <returns>The current order list model.</returns>
        [HttpGet("get/orderlist")]
        public IActionResult ObterOrderListModel()
        {
            OrderListModel orderListModel = _ruleService.GetOrderListModel();
            return Ok(orderListModel);
        }

        /// <summary>
        /// Updates orders based on the provided information.
        /// </summary>
        /// <param name="updateOrderModels">List of orders to update.</param>
        /// <returns>The updated order list.</returns>
        [HttpPut("update/order")]
        public IActionResult UpdateOrder([FromBody] List<PutOrderListModel> updateOrderModels)
        {
            if (updateOrderModels == null || !updateOrderModels.Any())
            {
                return BadRequest("A lista de pedidos para atualizar não pode ser nula ou vazia.");
            }

            _ruleService.UpdateOrder(updateOrderModels);

            // Return new list after update
            return Ok(_ruleService.GetOrderListModel());
        }

        /// <summary>
        /// Deletes an order based on the provided order ID.
        /// </summary>
        /// <param name="orderId">Order ID to delete.</param>
        /// <returns>The updated order list after deletion.</returns>
        [HttpDelete("remove/order/{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            _ruleService.DeleteOrder(orderId);

            // Return new list after delete
            return Ok(_ruleService.GetOrderListModel());
        }
    }
}

