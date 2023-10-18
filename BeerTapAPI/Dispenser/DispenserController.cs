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
        return NotFound();
    }

    [HttpGet("{id}/spending")]
    public IActionResult UsageReport()
    {
        return NotFound();
    }
}