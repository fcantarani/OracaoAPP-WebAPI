using Microsoft.AspNetCore.Mvc;
using OracaoApp.API.Services;

namespace OracaoApp.API.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class PrayerController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ApplicationDbContext _context;

    public PrayerController(AuthService authService, ApplicationDbContext context)
    {
        _authService = authService;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Prayer>>> GetPrayers()
    {
        var prayers = await _context.Prayers.Include(x => x.PrayerCategory).Include(x => x.PrayerComments).ToListAsync();
        return prayers;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Prayer>> GetPrayer(int id)
    {
        var prayer = await _context.Prayers.FindAsync(id);

        if (prayer == null)
        {
            return NotFound();
        }

        return prayer;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPrayer(int id, PrayerCreateRequest model)
    {
        var prayer = _context.Prayers.Find(id);

        if (prayer == null)
            return NotFound();

        prayer.Title = model.Title;
        prayer.Description = model.Description;
        prayer.PrayingForName = model.PrayingForName;
        prayer.IsPublic = model.IsPublic;
        prayer.PrayerCategoryId = model.PrayerCategoryId;
        prayer.UpdatedDate = DateTime.Now;

        _context.Update(prayer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Prayer>> PostPrayer(PrayerCreateRequest model)
    {
        var user = _authService.Username;


        var prayer = new Prayer
        {
            Title = model.Title,
            Description = model.Description,
            PrayingForName = model.PrayingForName,
            IsPublic = model.IsPublic,
            Owner = _authService.Username,
            PrayerCategoryId = model.PrayerCategoryId,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        _context.Prayers.Add(prayer);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPrayer", new { id = prayer.Id }, prayer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrayer(int id)
    {
        var prayer = await _context.Prayers.FindAsync(id);
        if (prayer == null)
            return NotFound();

        _context.Prayers.Remove(prayer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PrayerExists(int id)
    {
        return _context.Prayers.Any(e => e.Id == id);
    }
}
