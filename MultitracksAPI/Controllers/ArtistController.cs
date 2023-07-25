using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultitracksAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ArtistController : ControllerBase
{
    private readonly TestAssesstmentDContext _context;

    public ArtistController(TestAssesstmentDContext context)
    {
        _context = context;
    }

    // GET: api/Artists/Search?name={name}
    [HttpGet("Search")]
    public async Task<ActionResult<IEnumerable<Artist>>> GetArtistsByName(string name)
    {
        return await _context.Artists
            .Where(a => a.Title.Contains(name))
            .ToListAsync();
    }

    // GET: api/Artists/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Artist>> GetArtist(int id)
    {
        var artist = await _context.Artists.FindAsync(id);

        if (artist == null)
        {
            return NotFound();
        }

        return artist;
    }

    // POST: api/Artists/Add
    [HttpPost("Add")]
    public async Task<ActionResult<Artist>> AddArtist(Artist artist)
    {
        _context.Artists.Add(artist);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArtist), new { id = artist.ArtistId }, artist);
    }
}