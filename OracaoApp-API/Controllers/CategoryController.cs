using Microsoft.AspNetCore.Mvc;

namespace OracaoApp.API.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class CategoryController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await context.Categories.ToListAsync();
    }


    [HttpGet("Category/{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await context.Categories.FindAsync(id);

        if (category == null)
            return NotFound();

        return category;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, CategoryUpdateRequest model)
    {
        var category = context.Categories.Find(id);

        if (category == null)
            return NotFound();

        category.Name = model.Name;
        category.HexColor = model.HexColor;
        category.UpdatedDate = DateTime.Now;

        context.Update(category);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(CategoryCreateRequest model)
    {
        var newCategory = new Category
        {
            Name = model.Name,
            HexColor = model.HexColor,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        context.Categories.Add(newCategory);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetCategory", new { id = newCategory.Id }, newCategory);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryExists(int id)
    {
        return context.Categories.Any(e => e.Id == id);
    }
}
