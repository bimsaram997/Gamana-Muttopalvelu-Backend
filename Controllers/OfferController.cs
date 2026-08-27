using Gamana_Muttopalvelu_Backend.DTO;
using Gamana_Muttopalvelu_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gamana_Muttopalvelu_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;
        private readonly ILogger<OfferController> _logger;

        public OfferController(IOfferService offerService, ILogger<OfferController> logger)
        {
            _offerService = offerService;
            _logger = logger;

        }

        [HttpPost]
        [ProducesResponseType(typeof(OfferResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _offerService.CreateOfferAsync(dto);

                return CreatedAtAction(
                    nameof(CreateOffer),
                    new { id = result.OfferId },
                    result
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating offer for email: {Email}", dto?.Email);

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while processing your offer request.",
                    error = ex.Message
                });
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<OfferDetailResponseDto>> GetOfferById(Guid id)
        {
            var booking = await _offerService.GetOfferByIdAsync(id);

            if (booking == null)
            {
                return NotFound(new { message = $"Offer with ID '{id}' was not found." });
            }

            return Ok(booking);
        }
    }

}
