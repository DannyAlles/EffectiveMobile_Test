using Api.ViewModels.Responses;
using AutoMapper;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogbooksController : ControllerBase
    {
        private readonly ILogbookService _logbookService;
        private readonly IMapper _mapper;
        private readonly ILogger<LogbooksController> _logger;

        public LogbooksController(ILogbookService logbookService,
                                  IMapper mapper,
                                  ILogger<LogbooksController> logger)
        {
            _logbookService = logbookService ?? throw new ArgumentNullException(nameof(logbookService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        /// <summary>
        /// Получает список IP адресов из указанного диапазона и времени
        /// </summary>
        /// <param name="startIp">Начально диапазона IP</param>
        /// <param name="endIp">Конец диапазона IP</param>
        /// <param name="startDate">Начала интервала времени</param>
        /// <param name="endDate">Конец интервала времени</param>
        /// <returns>Список IP в формате {IP}: {AccessAt} с количеством обращений с него</returns>
        /// <response code="200">Файл получен</response>
        /// <response code="400">Неверный формат IP</response>
        [ProducesResponseType(typeof(IEnumerable<LogbookByIpViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // GET: <LogbooksController>
        [HttpGet]
        public async Task<IActionResult> GetLogbooksByIpAsync([FromQuery] string startIp,
                                                              [FromQuery] string endIp,
                                                              [FromQuery] DateTimeOffset startDate,
                                                              [FromQuery] DateTimeOffset endDate)
        {
            try
            {
                var filtredLogbooks = await _logbookService.GetLogsByIpAsync(startIp, endIp, startDate, endDate).ConfigureAwait(false);

                _logger.LogInformation($"User gets IPs from {startIp} to {endIp}");

                return Ok(filtredLogbooks.Select(x => new LogbookByIpViewModel()
                {
                    AddressAndTime = $"{x.IpAddress}: {x.AccessAt.ToString("yyyy-MM-dd HH:mm:ss")}",
                    Count = x.Count
                }));
            }
            catch (IpAddressNotValidException)
            {
                _logger.LogInformation($"User entered invalid Ip {startIp} - {endIp}");
                return BadRequest("Invalid Ip");
            }
        }

        /// <summary>
        /// Получает файл со списком IP адресов из указанного диапазона и времени
        /// </summary>
        /// <param name="startIp">Начально диапазона IP</param>
        /// <param name="endIp">Конец диапазона IP</param>
        /// <param name="startDate">Начала интервала времени</param>
        /// <param name="endDate">Конец интервала времени</param>
        /// <returns>Файл со списком IP в формате {IP}: {AccessAt} с количеством обращений с него</returns>
        /// <response code="200">Файл получен</response>
        /// <response code="400">Неверный формат IP</response>
        [ProducesResponseType(typeof(IEnumerable<LogbookByIpViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // GET: <LogbooksController>
        [HttpGet("Download")]
        public async Task<IActionResult> GetFileLogbooksByIpAsync([FromQuery] string startIp, [FromQuery] string endIp,
                                                                  [FromQuery] DateTimeOffset startDate,
                                                                  [FromQuery] DateTimeOffset endDate)
        {
            try
            {
                var stream = await _logbookService.GetFileLogsByIpAsync(startIp, endIp, startDate, endDate).ConfigureAwait(false);
                stream.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation($"User gets file with IPs from {startIp} to {endIp}");

                return File(stream, $"text/txt", fileDownloadName: "logbook.txt"); ;
            }
            catch (IpAddressNotValidException)
            {
                _logger.LogInformation($"User entered invalid Ip {startIp} - {endIp}");
                return BadRequest("Invalid Ip");
            }
        }
    }
}
