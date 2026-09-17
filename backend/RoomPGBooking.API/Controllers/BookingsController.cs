using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomPGBooking.API.Data;
using RoomPGBooking.API.DTOs;
using RoomPGBooking.API.Models;

namespace RoomPGBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _db;

    public BookingsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        => await _db.Bookings.Include(b => b.Room)
            .AsNoTracking()
            .OrderByDescending(b => b.BookingId)
            .ToListAsync();

    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking(CreateBookingRequest request)
    {
        var room = await _db.Rooms.FindAsync(request.RoomId);
        if (room is null) return NotFound("Room not found.");
        if (!room.IsAvailable) return BadRequest("Room is not available.");

        var booking = new Booking
        {
            RoomId = request.RoomId,
            CustomerName = request.CustomerName,
            Phone = request.Phone,
            Email = request.Email,
            MoveInDate = request.MoveInDate,
            Status = "Pending"
        };

        room.IsAvailable = false;
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        booking.Room = room;
        return Ok(booking);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
    {
        var allowed = new[] { "Pending", "Approved", "Rejected", "Cancelled" };
        if (!allowed.Contains(status, StringComparer.OrdinalIgnoreCase))
            return BadRequest("Invalid status.");

        var booking = await _db.Bookings.Include(b => b.Room).FirstOrDefaultAsync(b => b.BookingId == id);
        if (booking is null) return NotFound();

        booking.Status = status;
        if (booking.Room is not null)
            booking.Room.IsAvailable = !status.Equals("Approved", StringComparison.OrdinalIgnoreCase);

        await _db.SaveChangesAsync();
        return NoContent();
    }
}
