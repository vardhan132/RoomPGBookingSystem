using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomPGBooking.API.Data;
using RoomPGBooking.API.Models;

namespace RoomPGBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly AppDbContext _db;

    public RoomsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        => await _db.Rooms.AsNoTracking().OrderBy(r => r.RoomId).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Room>> GetRoom(int id)
    {
        var room = await _db.Rooms.AsNoTracking().FirstOrDefaultAsync(r => r.RoomId == id);
        return room is null ? NotFound() : Ok(room);
    }

    [HttpPost]
    public async Task<ActionResult<Room>> CreateRoom(Room room)
    {
        room.RoomId = 0;
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRoom), new { id = room.RoomId }, room);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRoom(int id, Room room)
    {
        if (id != room.RoomId) return BadRequest();

        var existing = await _db.Rooms.FindAsync(id);
        if (existing is null) return NotFound();

        existing.RoomNumber = room.RoomNumber;
        existing.RoomType = room.RoomType;
        existing.Rent = room.Rent;
        existing.Location = room.Location;
        existing.Facilities = room.Facilities;
        existing.IsAvailable = room.IsAvailable;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _db.Rooms.FindAsync(id);
        if (room is null) return NotFound();

        var hasBookings = await _db.Bookings.AnyAsync(b => b.RoomId == id);
        if (hasBookings)
            return Conflict("This room has bookings and cannot be deleted.");

        _db.Rooms.Remove(room);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
