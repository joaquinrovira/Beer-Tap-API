using BeerTapAPI.Dtos;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace BeerTapAPI.Controllers;

[Route("dispenser")]
[ApiController]
public class DispenserController : ControllerBase
{

    DispenserService DispenserService;

    public DispenserController(DispenserService dispenserService)
    {
        DispenserService = dispenserService;
    }

    [HttpPost]
    public IActionResult Register([FromBody] RegisterDispenserRawRequest data)
    {
        // NOTE: Model validation checked implicitly by [ApiController]
        // https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-7.0#automatic-http-400-responses
        var dispenser = DispenserService.Register(data.Checked()).GetOrThrow();
        return Ok(dispenser);
    }

    [HttpPut("{id}")]
    public IActionResult SetStatus([FromRoute] Guid id, [FromBody] SetDispenserStatusRequest data)
    {
        var result = DispenserService.SetStatus(id, data);
        if (result.IsSuccess) return Accepted(result.Value);
        if (result.Error is Errors.Conflict c) return Conflict("Dispenser is already opened/closed");
        throw result.Error;
    }

    [HttpGet("{id}/spending")]
    public IActionResult UsageReport([FromRoute] Guid id)
    {
        var result = DispenserService.UsageReport(id);
        if (result.IsSuccess) return Ok(result.Value);
        if (result.Error is Errors.NotFound c) return NotFound("Requested dispenser does not exist");
        throw result.Error;
    }
}