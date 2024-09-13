using Microsoft.AspNetCore.Mvc;
using OracaoApp.API.Services;

namespace OracaoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrayingForController(ApplicationDbContext context, AuthService authService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrayingFor>>> GetPrayingFors()
        {
            return await context.PrayingFors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrayingFor>> GetPrayingFor(int id)
        {
            var prayingFor = await context.PrayingFors.FindAsync(id);

            if (prayingFor == null)
                return NotFound();

            return prayingFor;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrayingFor(int id, PrayingForCreateRequest model)
        {
            var prayingFor = context.PrayingFors.Find(id);

            if (prayingFor == null)
                return NotFound();

            prayingFor.OwnerId = model.OwnerId;
            prayingFor.Id = id;
            prayingFor.UpdatedDate = DateTime.Now;

            context.Update(prayingFor);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PrayingFor>> PostPrayingFor(PrayingForCreateRequest model)
        {
            var prayingFor = new PrayingFor
            {
                OwnerId = authService.UserId,
                PrayerId = model.PrayerId,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            context.PrayingFors.Add(prayingFor);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetPrayingFor", new { id = prayingFor.Id }, prayingFor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrayingFor(int id)
        {
            var prayingFor = await context.PrayingFors.FindAsync(id);

            if (prayingFor == null)
                return NotFound();

            context.PrayingFors.Remove(prayingFor);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrayingForExists(int id)
        {
            return context.PrayingFors.Any(e => e.Id == id);
        }
    }
}
