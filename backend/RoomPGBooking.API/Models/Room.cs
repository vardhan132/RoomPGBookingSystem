using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RoomPGBooking.API.Models;

public class Room
{
    public int RoomId { get; set; }

    [Required, MaxLength(20)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string RoomType { get; set; } = string.Empty;

    public decimal Rent { get; set; }

    [Required, MaxLength(100)]
    public string Location { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Facilities { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }

    [JsonIgnore]
    public List<Booking> Bookings { get; set; } = [];
}