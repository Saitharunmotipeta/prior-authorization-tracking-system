using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Payer.API.DTOs;

namespace PriorAuthorization.Payer.API.Controllers
{
    [ApiController]
    [Route("api/payers")]
    public class PayerController : ControllerBase
    {
        private readonly IPayerService _payerService;
        private readonly ILogger<PayerController> _logger;

        public PayerController(
            IPayerService payerService,
            ILogger<PayerController> logger)
        {
            _payerService = payerService;
            _logger = logger;
        }

        
        [HttpGet("{payerId}/authorization-requests")]
        public async Task<IActionResult> GetAuthorizationRequests(
            int payerId,
            [FromQuery] RequestsFilter filter)
        {
            _logger.LogInformation(
                "Fetching authorization requests for PayerId: {PayerId} with filters {@Filter}",
                payerId, filter);

            var result = await _payerService.GetAuthorizationRequests(payerId, filter);

            if (result == null || !result.Any())
            {
                return NoContent(); 
            }

            return Ok(result);
        }


        [HttpGet("{authId}")]
        public async Task<IActionResult> GetAuthorizationDetails(int authId)
        {
            _logger.LogInformation(
                "Fetching authorization details for AuthId: {AuthId}",
                authId);

            var result = await _payerService.GetAuthorizationDetails(authId);

            if (result == null)
            {
                return NotFound($"Authorization request with id {authId} not found");
            }

            return Ok(result);
        }


        [HttpPatch("{authId}/review")]
        public async Task<IActionResult> ReviewAuthorization(
    int authId,
    [FromBody] ReviewRequest dto)
        {
            _logger.LogInformation(
                "Review action {Action} started for AuthId: {AuthId}",
                dto.Action, authId);

            var result = await _payerService.ReviewAuthorization(authId, dto);

            if (!result)
            {
                return NotFound($"Authorization request with id {authId} not found");
            }

            return Ok("Review completed successfully");
        }

    }
    }
