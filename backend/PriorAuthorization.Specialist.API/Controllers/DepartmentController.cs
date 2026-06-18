using Microsoft.AspNetCore.Mvc;
using PriorAuthorization.Specialist.API.Services.Interfaces;

namespace Specialist.API.Controllers;

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

        return Ok(departments);
    }
}