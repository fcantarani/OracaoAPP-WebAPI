using Microsoft.AspNetCore.Mvc;

namespace OracaoApp.API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class PrayerCommentController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrayerComment>>> GetPrayerComments()
        {
            return await context.PrayerComments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrayerComment>> GetPrayerComment(int id)
        {
            var prayerComment = await context.PrayerComments.FindAsync(id);

            if (prayerComment == null)
                return NotFound();

            return prayerComment;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrayerComment(int id, PrayerCommentCreateRequest model)
        {
            var prayerComment = context.PrayerComments.Find(id);

            if (prayerComment == null)
                return NotFound();

            prayerComment.OwnerId = model.OwnerId;
            prayerComment.Message = model.Message;
            prayerComment.PrayerId = model.PrayerId;
            prayerComment.UpdatedDate = DateTime.Now;

            context.Update(prayerComment);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PrayerComment>> PostPrayerComment(PrayerCommentCreateRequest model)
        {
            var prayerComment = new PrayerComment
            {
                Message = model.Message,
                OwnerId = model.OwnerId,
                PrayerId = model.PrayerId,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.UtcNow,
            };

            context.PrayerComments.Add(prayerComment);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetPrayerComment", new { id = prayerComment.Id }, prayerComment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrayerComment(int id)
        {
            var prayerComment = await context.PrayerComments.FindAsync(id);
            if (prayerComment == null)
                return NotFound();

            context.PrayerComments.Remove(prayerComment);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrayerCommentExists(int id)
        {
            return context.PrayerComments.Any(e => e.Id == id);
        }
    }
}
