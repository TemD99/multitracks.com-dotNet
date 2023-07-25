using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultitracksAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SongController : ControllerBase
{
    private readonly TestAssesstmentDContext _context;

    public SongController(TestAssesstmentDContext context)
    {
        _context = context;
    }

    // GET: api/Songs/List?pageNumber=1&pageSize=10
    [HttpGet("List")]
    public async Task<ActionResult<IEnumerable<Song>>> GetSongs(int pageNumber = 1, int pageSize = 10)
    {
        return await _context.Songs
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // GET: api/Songs/ByArtist?artistId={artistId}
    [HttpGet("ByArtist")]
    public async Task<ActionResult<IEnumerable<Song>>> GetSongsByArtist(int artistId)
    {
        return await _context.Songs
            .Where(s => s.ArtistId == artistId)
            .ToListAsync();
    }

    // POST: api/Songs/Add
    [HttpPost("Add")]
    public async Task<ActionResult<Song>> AddSong(Song song)
    {
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetSong", new { id = song.SongId }, song);
    }
}




