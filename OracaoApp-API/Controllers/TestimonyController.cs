using Microsoft.AspNetCore.Mvc;

namespace OracaoApp.API.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class TestimonyController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Testimony>>> GetTestimonies()
    {
        return await context.Testimonies.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Testimony>> GetTestimony(int id)
    {
        var testimony = await context.Testimonies.FindAsync(id);

        if (testimony == null)
        {
            return NotFound();
        }

        return testimony;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTestimony(int id, TestimonyCreateRequest model)
    {
        var testimony = context.Testimonies.Find(id);

        if (testimony == null)
            return NotFound();

        testimony.Title = model.Title;
        testimony.Description = model.Description;
        testimony.UpdatedDate = DateTime.Now;

        context.Update(testimony);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Testimony>> PostTestimony(TestimonyCreateRequest model)
    {
        var newTestimony = new Testimony
        {
            Title = model.Title,
            Description = model.Description,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        context.Testimonies.Add(newTestimony);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetTestimonies", new { id = newTestimony.Id }, newTestimony);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestimony(int id)
    {
        var testimony = await context.Testimonies.FindAsync(id);
        if (testimony == null)
        {
            return NotFound();
        }

        context.Testimonies.Remove(testimony);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool TestimonyExists(int id)
    {
        return context.Testimonies.Any(e => e.Id == id);
    }
}
