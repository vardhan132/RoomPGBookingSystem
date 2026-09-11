using RoomPGBooking.API.Models;

namespace RoomPGBooking.API.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        if (db.Rooms.Any()) return;

        db.Rooms.AddRange(
            new Room
            {
                RoomNumber = "101",
                RoomType = "Single",
                Rent = 8000,
                Location = "Madhapur, Hyderabad",
                Facilities = "Wi-Fi, Bed, Cupboard, Attached Bathroom",
                IsAvailable = true
            },
            new Room
            {
                RoomNumber = "102",
                RoomType = "Double",
                Rent = 6000,
                Location = "Kondapur, Hyderabad",
                Facilities = "Wi-Fi, Bed, Cupboard, Parking",
                IsAvailable = true
            },
            new Room
            {
                RoomNumber = "201",
                RoomType = "Single",
                Rent = 9000,
                Location = "Gachibowli, Hyderabad",
                Facilities = "Wi-Fi, AC, Bed, Attached Bathroom",
                IsAvailable = true
            }
        );

        db.SaveChanges();
    }
}
