using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Shared.Common;
using PriorAuthorization.Specialist.API.DTOs;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace PriorAuthorization.Specialist.API.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(
        IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult>
        GetDepartments([FromQuery] int facilityId)
    {
        var departments =
            await _departmentService
                .GetDepartmentsByFacilityAsync(facilityId);

        return Ok(
            ApiResponse<IEnumerable<DepartmentDto>>
                .SuccessResponse(
                    departments,
                    "Departments retrieved successfully."));
    }
}