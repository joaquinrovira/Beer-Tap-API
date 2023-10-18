using BeerTapAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BeerTapAPI.Controllers;

[Route("dispenser")]
public class DispenserController : ControllerBase
{
    [HttpPost]
    public IActionResult Register([FromBody] RegisterDispenserRequest data)
    {
        return NotFound();
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