using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Payer.API.DTOs;
using PriorAuthorization.Payer.API.Services.Interfaces;
using PriorAuthorization.Shared.Common;
using PriorAuthorization.Shared.Exceptions;
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
            var result = await _payerService.GetFacilities();
            return Ok(
                ApiResponse<List<FacilityDto>>
                    .SuccessResponse(
                        result,
                        "Facilities fetched successfully."));
        }

        [HttpGet("facilities/{facilityId}/authorization-requests")]
        public async Task<IActionResult> GetRequestsByFacility(
            int facilityId)
                {
                    var result =
                        await _payerService
                            .GetRequestsByFacility(
                                facilityId);

                    return Ok(
                        ApiResponse<List<RequestLists>>
                            .SuccessResponse(
                                result,
                                "Authorization requests fetched successfully."));
                }


        [HttpGet("{authId}")]
        public async Task<IActionResult> GetAuthorizationDetails(
    int authId)
        {
            var result =
                await _payerService
                    .GetAuthorizationDetails(
                        authId);

            return Ok(
                ApiResponse<RequestsDetails>
                    .SuccessResponse(
                        result,
                        "Authorization details fetched successfully."));
        }


        [HttpPatch("{authId}/review")]
        public async Task<IActionResult> ReviewAuthorization(
            int authId,
            [FromBody] ReviewRequest dto)
        {
            await _payerService
                .ReviewAuthorization(
                    authId,
                    dto);

            return Ok(
                ApiResponse<string>
                    .SuccessResponse(
                        "Review completed successfully.",
                        "Authorization request reviewed successfully."));
        }


        [HttpGet("emergency")]
        public async Task<IActionResult> GetEmergencyRequests()
        {
            var result =
                await _payerService
                    .GetEmergencyRequests();

            return Ok(
                ApiResponse<List<RequestLists>>
                    .SuccessResponse(
                        result,
                        "Emergency authorization requests fetched successfully."));
        }



        [HttpGet("Reminders")]
        public async Task<IActionResult> GetReminders()
        {
            var result =
                await _payerService
                    .GetReminders();

            return Ok(
                ApiResponse<ReminderListResponseDto>
                    .SuccessResponse(
                        result,
                        "Reminders fetched successfully."));
        }



        [HttpGet("audit-history")]
        public async Task<IActionResult> GetAuditHistory()
        {
            var result = await _payerService.GetPayerAuditHistory();

            if (result == null || !result.Any())
                return NoContent();

            return Ok(ApiResponse<List<AuditHistoryDto>>
                .SuccessResponse(result, "Audit history fetched successfully"));
        }



    }
}
