using Microsoft.AspNetCore.Mvc;
using OracaoApp.API.Services;

namespace OracaoApp.API.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class PrayerController(AuthService authService, ApplicationDbContext context) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Prayer>>> GetPrayers()
    {
        var prayers = await context.Prayers.Include(x => x.PrayerCategory).Include(x => x.PrayerComments).ToListAsync();
        return prayers;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Prayer>> GetPrayer(int id)
    {
        var prayer = await context.Prayers.FindAsync(id);

        if (prayer == null)
        {
            return NotFound();
        }

        return prayer;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPrayer(int id, PrayerCreateRequest model)
    {
        var prayer = context.Prayers.Find(id);

        if (prayer == null)
            return NotFound();

        prayer.Title = model.Title;
        prayer.Description = model.Description;
        prayer.PrayingForName = model.PrayingForName;
        prayer.IsPublic = model.IsPublic;
        prayer.PrayerCategoryId = model.PrayerCategoryId;
        prayer.UpdatedDate = DateTime.Now;

        context.Update(prayer);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Prayer>> PostPrayer(PrayerCreateRequest model)
    {
        var user = authService.Username;


        var prayer = new Prayer
        {
            Title = model.Title,
            Description = model.Description,
            PrayingForName = model.PrayingForName,
            IsPublic = model.IsPublic,
            Owner = authService.Username,
            PrayerCategoryId = model.PrayerCategoryId,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        context.Prayers.Add(prayer);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetPrayer", new { id = prayer.Id }, prayer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrayer(int id)
    {
        var prayer = await context.Prayers.FindAsync(id);
        if (prayer == null)
            return NotFound();

        context.Prayers.Remove(prayer);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool PrayerExists(int id)
    {
        return context.Prayers.Any(e => e.Id == id);
    }
}
