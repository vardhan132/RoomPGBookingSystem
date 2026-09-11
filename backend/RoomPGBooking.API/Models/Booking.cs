using System.ComponentModel.DataAnnotations;

namespace RoomPGBooking.API.Models;

public class Booking
{
    public int BookingId { get; set; }

    public int RoomId { get; set; }

    [Required, MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public DateTime MoveInDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    public Room? Room { get; set; }
}
