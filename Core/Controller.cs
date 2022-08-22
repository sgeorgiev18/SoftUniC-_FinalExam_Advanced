using BookingApp.Core.Contracts;
using BookingApp.Models.Hotels;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Repositories;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Models.Bookings;
using BookingApp.Models.Rooms;
using BookingApp.Utilities.Messages;

namespace BookingApp.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IHotel> hotels = new HotelRepository();
        public string AddHotel(string hotelName, int category)
        {
            IHotel hotel = this.hotels.Select(hotelName);
            if (hotel != null)
            {
                return $"Hotel {hotelName} is already registered in our platform.";
            }
            Hotel newHotel = new Hotel(hotelName, category);
            hotels.AddNew(newHotel);
            return $"{category} stars hotel {hotelName} is registered in our platform and expects room availability to be uploaded.";
        }

        public string BookAvailableRoom(int adults, int children, int duration, int category)
        {
            var orderedHotels = this.hotels.All()
                .OrderBy(x => x.FullName);
            ICollection<IHotel> orderedHotelsCheckCateogry = new List<IHotel>();
            foreach (var hotel in orderedHotels)
            {
                if (hotel.Category == category)
                {
                    orderedHotelsCheckCateogry.Add(hotel);
                }
            }
            if (orderedHotelsCheckCateogry.Count == 0)
            {
                return $"{category} star hotel is not available in our platform.";
            }
            foreach (var hotel in orderedHotelsCheckCateogry)
            {
                var hotelRooms = hotel.Rooms.All();
                ICollection<IRoom> validRooms = new List<IRoom>();
                foreach(var room in hotelRooms)
                {
                    if (room.PricePerNight > 0)
                    {
                        validRooms.Add(room);
                    }
                }
                var orderedRooms = validRooms.OrderBy(room => room.BedCapacity);
                foreach (var room in orderedRooms)
                {
                    if (room.BedCapacity >= adults + children)
                    {
                        int numberOfBookings = hotel.Bookings.All().Count;
                        Booking book = new Booking(room, duration, adults, children, numberOfBookings+1);
                        hotel.Bookings.AddNew(book);
                        return $"Booking number {book.BookingNumber} for {hotel.FullName} hotel is successful!";
                    }
                }
            }
            return "We cannot offer appropriate room for your request.";


        }

        public string HotelReport(string hotelName)
        {
            var hotel = hotels.Select(hotelName);
            if (hotel == null)
            {
                return $"Profile {hotelName} doesn’t exist!";
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Hotel name: { hotelName}");
            sb.AppendLine($"--{hotel.Category} star hotel");
            sb.AppendLine($"--Turnover: {hotel.Turnover:F2} $");
            sb.AppendLine("--Bookings:");
            sb.AppendLine("");
            foreach (var booking in hotel.Bookings.All())
            {
                sb.AppendLine(booking.BookingSummary());
            }

            return sb.ToString();
        }

        public string SetRoomPrices(string hotelName, string roomTypeName, double price)
        {
            IHotel hotel = hotels.Select(hotelName);
            if (hotel == null)
            {
                return $"Profile {hotelName} doesn’t exist!";
            }
            if (roomTypeName != "Apartment" && roomTypeName != "DoubleBed" && roomTypeName != "Studio")
            {
                throw new ArgumentException(ExceptionMessages.RoomTypeIncorrect);
            }
            bool typeIsCreated = false;
            foreach (var room in hotel.Rooms.All())
            {
                if (room.GetType().Name == roomTypeName)
                {
                    typeIsCreated = true;
                    break;
                }
            }
            if (typeIsCreated)
            {
                var room = hotel.Rooms.Select(roomTypeName);
                if (room.PricePerNight == 0)
                {
                    room.SetPrice(price);
                    return $"Price of {roomTypeName} room type in {hotelName} hotel is set!";
                }
                else
                {
                    return "Price is already set!";
                }
            }
            else
            {
                return "Room type is not created yet!";
            }
        }

        public string UploadRoomTypes(string hotelName, string roomTypeName)
        {
            IHotel hotel = hotels.Select(hotelName);
            if (hotel == null)
            {
                return $"Profile {hotelName} doesn’t exist!";
            }
            bool typeIsCreated = false;
            foreach (var room in hotel.Rooms.All())
            {
                if (room.GetType().Name == roomTypeName)
                {
                    typeIsCreated = true;
                    break;
                }
            }
            if (typeIsCreated)
            {
                return "Room type is already created!";
            }

            IRoom newRoom = roomTypeName switch
            {
                nameof(Apartment) => new Apartment(),
                nameof(Studio) => new Studio(),
                nameof(DoubleBed) => new DoubleBed(),
                _ => throw new ArgumentException(ExceptionMessages.RoomTypeIncorrect)
            };
            hotel.Rooms.AddNew(newRoom);
            return $"Successfully added {roomTypeName} room type in {hotelName} hotel!";
        }
    }
}
