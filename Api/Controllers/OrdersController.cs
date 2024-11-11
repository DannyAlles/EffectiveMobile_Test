using Api.ViewModels.Requests;
using Api.ViewModels.Responses;
using AutoMapper;
using Domain.Models;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService,
                                IMapper mapper,
                                ILogger<OrdersController> logger)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }
        /// <summary>
        /// Получает список заказов по району
        /// </summary>
        /// <param name="districtId">Идентификатор района</param>
        /// <param name="page">Страница</param>
        /// <param name="perPage">Кол-во элементов на странице</param>
        /// <param name="firstDeliveryDateTime">Время первого заказа</param>
        /// <returns>Заказы</returns>
        /// <response code="200">Файл получен</response>
        /// <response code="400">Район не найден или пагинация неверна</response>
        [ProducesResponseType(typeof(IEnumerable<OrderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // GET: <OrdersController>
        [HttpGet("Districts/{districtId}")]
        public async Task<IActionResult> GetOrdersByDistrictAsync(Guid districtId,
                                                                  [FromQuery] DateTime firstDeliveryDateTime,
                                                                  [FromQuery] int page = 1,
                                                                  [FromQuery] int perPage = 10)
        {
            if (page <= 0)
                return BadRequest("Page must be equal to or greater than 1");
            if ((perPage <= 0 || perPage > 100))
                return BadRequest("perPage must be equal to or greater than 1 and less then 100");

            try
            {
                var orders = await _orderService.GetOrdersByDistrictAsync(districtId, firstDeliveryDateTime, page, perPage).ConfigureAwait(false);

                _logger.LogInformation($"User gets orders information for district {districtId}");

                return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(orders));
            }
            catch (DistrictNotFoundException)
            {
                _logger.LogInformation($"District does not exist");
                return BadRequest("District does not exist");
            }
            catch (OrderNotFoundException)
            {
                _logger.LogInformation($"Order with that time and district does not exist");
                return BadRequest("Order with that time and district does not exist");
            }
        }

        /// <summary>
        /// Скачивает файл с заказами по району
        /// </summary>
        /// <param name="districtId">Идентификатор района</param>
        /// <param name="firstDeliveryDateTime">Время первого заказа</param>
        /// <returns>Файл с заказами</returns>
        /// <response code="200">Файл получен</response>
        /// <response code="400">Район не найден</response>
        [ProducesResponseType(typeof(IEnumerable<OrderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // GET: <OrdersController>
        [HttpGet("Districts/{districtId}/Download")]
        public async Task<IActionResult> DownloadOrdersByDistrictAsync(Guid districtId, [FromQuery] DateTimeOffset firstDeliveryDateTime)
        {
            try
            {
                var stream = await _orderService.GetOrdersByDistrictAsync(districtId, firstDeliveryDateTime).ConfigureAwait(false);
                stream.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation($"File send to download");
                return File(stream, $"text/txt", fileDownloadName: "orders.txt");
            }
            catch (DistrictNotFoundException)
            {
                _logger.LogInformation($"District does not exist");
                return BadRequest("District does not exist");
            }
            catch (OrderNotFoundException)
            {
                _logger.LogInformation($"Order with that time and district does not exist");
                return BadRequest("Order with that time and district does not exist");
            }
        }

        /// <summary>
        /// Создает заказ
        /// </summary>
        /// <param name="createOrderViewModel">Новый заказ</param>
        /// <returns>Заказ</returns>
        /// <response code="200">Обращение создан</response>
        /// <response code="400">Район не найден</response>
        /// <response code="422">Неверно заполненные поля</response>
        [ProducesResponseType(typeof(OrderViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        // Post: <OrdersController>
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderViewModel createOrderViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"User entered unprocessable data");
                return UnprocessableEntity(ModelState);
            }

            try
            {
                Order newOrder = _mapper.Map<Order>(createOrderViewModel);

                var order = await _orderService.CreateOrderAsync(newOrder, HttpContext.Connection.RemoteIpAddress).ConfigureAwait(false);
                
                _logger.LogInformation($"User created new order");

                return Ok(_mapper.Map<OrderViewModel>(order));
            }
            catch (DistrictNotFoundException)
            {
                _logger.LogInformation($"District does not exist");
                return BadRequest("District does not exist");
            }
            catch (NegativeWeightException)
            {
                _logger.LogInformation($"Weight must be more than 0");
                return BadRequest("Weight must be more than 0");
            }
        }
    }
}
