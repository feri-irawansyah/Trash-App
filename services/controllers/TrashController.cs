using Microsoft.AspNetCore.Mvc;
using Services.Models;

[ApiController]
[Route("api/[controller]")]
public class TrashController : ControllerBase
{
    private readonly TrashService _service;
    public TrashController(TrashService svc) => _service = svc;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _service.GetAll());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _service.Get(id);
        if (p is null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Trash trash)
    {
        await _service.Add(trash);
        return CreatedAtAction(nameof(Get), new { id = trash.TrashNID }, trash);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Trash trash)
    {
        if (id != trash.TrashNID) return BadRequest();
        await _service.Update(trash);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return NoContent();
    }
}
