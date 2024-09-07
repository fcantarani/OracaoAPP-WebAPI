using Microsoft.AspNetCore.Mvc;

namespace OracaoApp.API.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class PrayerCategoryController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrayerCategory>>> GetPrayerCategories()
    {
        return await context.PrayerCategories.ToListAsync();
    }


    [HttpGet("PrayerCategory/{id}")]
    public async Task<ActionResult<PrayerCategory>> GetPrayerCategory(int id)
    {
        var prayerCategory = await context.PrayerCategories.FindAsync(id);

        if (prayerCategory == null)
            return NotFound();

        return prayerCategory;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPrayerCategory(int id, CategoryUpdateRequest model)
    {
        var prayerCategory = context.PrayerCategories.Find(id);

        if (prayerCategory == null)
            return NotFound();

        prayerCategory.Name = model.Name;
        prayerCategory.HexColor = model.HexColor;
        prayerCategory.UpdatedDate = DateTime.Now;

        context.Update(prayerCategory);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<PrayerCategory>> PostPrayerCategory(CategoryCreateRequest model)
    {
        var prayerCategory = new PrayerCategory
        {
            Name = model.Name,
            HexColor = model.HexColor,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        context.PrayerCategories.Add(prayerCategory);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetPrayerCategory", new { id = prayerCategory.Id }, prayerCategory);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrayerCategory(int id)
    {
        var prayerCategory = await context.PrayerCategories.FindAsync(id);
        if (prayerCategory == null)
            return NotFound();

        context.PrayerCategories.Remove(prayerCategory);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool PrayerCategoryExists(int id)
    {
        return context.PrayerCategories.Any(e => e.Id == id);
    }
}
