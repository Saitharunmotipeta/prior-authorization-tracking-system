using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Payer.API.DTOs;
using PriorAuthorization.Shared.Enums;

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


        [HttpGet("facilities")]
        public async Task<IActionResult> GetFacilities()
        {
            var facilities = await _payerService.GetFacilities();
            return Ok(facilities);
        }

        [HttpGet("facilities/{facilityId}/authorization-requests")]
        public async Task<IActionResult> GetRequestsByFacility(int facilityId)
        {
            var result = await _payerService.GetRequestsByFacility(facilityId);

            if (result == null || !result.Any())
                return NoContent();

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


        [HttpGet("emergency")]
        public async Task<IActionResult> GetEmergencyRequests()
        {
            var result = await _payerService.GetEmergencyRequests();

            if (result == null || !result.Any())
                return NoContent();

            return Ok(result);
        }



        [HttpGet]
        public async Task<IActionResult> GetReminders()
        {
            _logger.LogInformation("Fetching all reminders");

            var result = await _payerService.GetReminders();

            if (result == null || !result.Data.Any())
                return NoContent();

            return Ok(result);
        }


    }
}
