using Microsoft.AspNetCore.Mvc;

[Route("dispenser")]
public class DispenserController : ControllerBase
{
    [HttpPost]
    public IActionResult Register()
    {
        return NotFound();
    }

    [HttpPut("{id}")]
    public IActionResult SetStatus()
    {
        return NotFound();
    }

    [HttpGet("{id}/spending")]
    public IActionResult UsageReport()
    {
        return NotFound();
    }
}