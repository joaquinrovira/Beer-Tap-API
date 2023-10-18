using BeerTapAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BeerTapAPI.Controllers;

[Route("dispenser")]
public class DispenserController : ControllerBase
{

    DispenserService DispenserService;

    public DispenserController(DispenserService dispenserService)
    {
        DispenserService = dispenserService;
    }

    [HttpPost]
    public IActionResult Register([FromBody] RegisterDispenserRequest data)
    {
        DispenserService.Register(data);
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult SetStatus([FromBody] SetDispenserStatusRequest data)
    {
        return NotFound();
    }

    [HttpGet("{id}/spending")]
    public IActionResult UsageReport()
    {
        return NotFound();
    }
}