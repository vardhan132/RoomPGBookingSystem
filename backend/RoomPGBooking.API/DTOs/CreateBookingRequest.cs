using System.ComponentModel.DataAnnotations;

namespace RoomPGBooking.API.DTOs;

public class CreateBookingRequest
{
    [Required]
    public int RoomId { get; set; }

    [Required, MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateTime MoveInDate { get; set; }
}
