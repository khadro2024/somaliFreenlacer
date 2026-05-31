using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Data;

namespace SomaliFreelanceMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public CategoriesController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _db.Categories
            .Select(c => new { c.CategoryId, c.Name, c.Description })
            .ToListAsync();
        return Ok(categories);
    }
}
