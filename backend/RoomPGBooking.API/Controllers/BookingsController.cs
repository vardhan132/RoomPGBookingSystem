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

    public BookingsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/Bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
    {
        return await _db.Bookings
            .Include(b => b.Room)
            .AsNoTracking()
            .OrderByDescending(b => b.BookingId)
            .ToListAsync();
    }

    // POST: api/Bookings
    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking(
        CreateBookingRequest request)
    {
        var room = await _db.Rooms.FindAsync(request.RoomId);

        if (room is null)
            return NotFound("Room not found.");

        if (!room.IsAvailable)
            return BadRequest("Room is not available.");

        // Create booking as Pending
        var booking = new Booking
        {
            RoomId = request.RoomId,
            CustomerName = request.CustomerName,
            Phone = request.Phone,
            Email = request.Email,
            MoveInDate = request.MoveInDate,
            Status = "Pending"
        };

        // IMPORTANT:
        // Do NOT mark the room as booked yet.
        // Admin must approve the booking first.

        _db.Bookings.Add(booking);

        await _db.SaveChangesAsync();

        booking.Room = room;

        return Ok(booking);
    }

    // PUT: api/Bookings/{id}/status
    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromQuery] string status)
    {
        var allowedStatuses = new[]
        {
            "Pending",
            "Approved",
            "Rejected",
            "Cancelled"
        };

        if (!allowedStatuses.Contains(
                status,
                StringComparer.OrdinalIgnoreCase))
        {
            return BadRequest("Invalid status.");
        }

        var booking = await _db.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking is null)
            return NotFound("Booking not found.");

        booking.Status = status;

        if (booking.Room is not null)
        {
            if (status.Equals(
                    "Approved",
                    StringComparison.OrdinalIgnoreCase))
            {
                // Only approved booking makes room unavailable
                booking.Room.IsAvailable = false;
            }
            else if (
                status.Equals("Rejected", StringComparison.OrdinalIgnoreCase) ||
                status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                // Rejected/cancelled booking makes room available
                booking.Room.IsAvailable = true;
            }
        }

        await _db.SaveChangesAsync();

        return NoContent();
    }
}