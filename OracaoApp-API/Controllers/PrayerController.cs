﻿using Microsoft.AspNetCore.Mvc;
using OracaoApp.API.Services;

namespace OracaoApp.API.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class PrayerController(AuthService authService, ApplicationDbContext context) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Prayer>>> GetPrayers()
    {
        var prayers = await context.Prayers.Include(x => x.PrayerCategory).Include(x => x.PrayerComments).Include(x => x.PrayingFors).ToListAsync();
        return prayers;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Prayer>> GetPrayer(int id)
    {
        var prayer = await context.Prayers.FindAsync(id);

        if (prayer == null)
            return NotFound();

        return prayer;
    }

    [HttpGet("User/{userId}")]
    public async Task<ActionResult<IEnumerable<Prayer>>> GetPrayersByUserId(Guid userId)
    {
        var prayers = await context.Prayers.Include(x => x.PrayerCategory).Include(x => x.PrayerComments).Include(x => x.PrayingFors).Where(x => x.OwnerId == userId).ToListAsync();

        if (prayers == null)
            return NotFound();

        return prayers;
    }

    [HttpGet("Category/{id}")]
    public async Task<ActionResult<IEnumerable<Prayer>>> GetPrayersByCategoryId(int id)
    {
        var prayers = id != 0 ? await context.Prayers.Include(x => x.PrayerCategory).Include(x => x.PrayerComments).Include(x => x.PrayingFors).Where(x => x.PrayerCategoryId == id).ToListAsync()
            : await context.Prayers.Include(x => x.PrayerCategory).Include(x => x.PrayerComments).Include(x => x.PrayingFors).ToListAsync()
            ;

        if (prayers == null)
            return NotFound();

        return prayers;
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

        var prayer = new Prayer
        {
            Title = model.Title,
            Description = model.Description,
            PrayingForName = model.PrayingForName,
            IsPublic = model.IsPublic,
            OwnerId = authService.UserId,
            PrayerCategoryId = model.PrayerCategoryId,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        context.Prayers.Add(prayer);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetPrayer", new { id = prayer.Id }, prayer);
    }

    [HttpDelete("Delete/{id}")]
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
