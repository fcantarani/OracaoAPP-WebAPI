using Microsoft.AspNetCore.Mvc;

namespace OracaoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonyCommentController(ApplicationDbContext context) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestimonyComment>>> GetTestimonyComments()
        {
            return await context.TestimonyComments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestimonyComment>> GetTestimonyComment(int id)
        {
            var testimonyComment = await context.TestimonyComments.FindAsync(id);

            if (testimonyComment == null)
            {
                return NotFound();
            }

            return testimonyComment;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestimonyComment(int id, TestimonyComment testimonyComment)
        {
            if (id != testimonyComment.Id)
            {
                return BadRequest();
            }

            context.Entry(testimonyComment).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestimonyCommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TestimonyComment>> PostTestimonyComment(TestimonyComment testimonyComment)
        {
            context.TestimonyComments.Add(testimonyComment);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetTestimonyComment", new { id = testimonyComment.Id }, testimonyComment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestimonyComment(int id)
        {
            var testimonyComment = await context.TestimonyComments.FindAsync(id);
            if (testimonyComment == null)
            {
                return NotFound();
            }

            context.TestimonyComments.Remove(testimonyComment);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestimonyCommentExists(int id)
        {
            return context.TestimonyComments.Any(e => e.Id == id);
        }
    }
}
