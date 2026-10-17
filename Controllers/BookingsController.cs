using Gamana_Muttopalvelu_Backend.DTO;
using Gamana_Muttopalvelu_Backend.DTO.Admin.bookings;
using Gamana_Muttopalvelu_Backend.DTO.Filters;
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
        public async Task<ActionResult<AllBookingDetailResponseDto>> GetBookingById(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
            {
                return NotFound(new { message = $"Booking with ID '{id}' was not found." });
            }

            return Ok(booking);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<BookingDetailResponseDto>>> GetAllBookings([FromQuery] BookingQueryParameters queryParams)
        {
            var bookings = await _bookingService.GetAllBookingsAsync( queryParams);
            return Ok(bookings);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] UpdateBookingDto dto)
        {
            // Synchronize route parameter with body DTO

            var updatedBookingId = await _bookingService.UpdateBookingAsync(id, dto);

            if (updatedBookingId == null)
                return NotFound(new { message = $"Booking with ID {id} was not found." });

            // Returns HTTP 200 OK with { "bookingId": "e8b3f64a-0aa0-4875-b125-acebc221ac89" }
            return Ok(new { bookingId = updatedBookingId });
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatusById(Guid id, [FromBody] UpdateStatusDto dto)
        {
            var isUpdated = await _bookingService.UpdateStatusByIdAsync(id, dto.Status);

            if (!isUpdated)
            {
                return NotFound(new { message = $"Booking with ID {id} was not found." });
            }

            return Ok(new { bookingId = id, status = dto.Status });
        }
    }
}
