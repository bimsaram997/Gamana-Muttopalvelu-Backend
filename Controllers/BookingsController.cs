using Gamana_Muttopalvelu_Backend.DTO;
using Gamana_Muttopalvelu_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gamana_Muttopalvelu_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _bookingService.CreateBookingAsync(dto);

                return CreatedAtAction(
                    nameof(CreateBooking),
                    new { id = result.BookingId },
                    result
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating booking for email: {Email}", dto?.Email);

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while processing your booking request.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDetailResponseDto>> GetBookingById(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
            {
                return NotFound(new { message = $"Booking with ID '{id}' was not found." });
            }

            return Ok(booking);
        }
    }
}
