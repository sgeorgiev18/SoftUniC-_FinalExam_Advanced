using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Hotels.Contacts;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories;
using BookingApp.Repositories.Contracts;
using BookingApp.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.Models.Hotels
{
    public class Hotel : IHotel
    {
        private string fullName;
        private int category;
        private double turnover = 0;
        private readonly IRepository<IBooking> bookings;
        private readonly IRepository<IRoom> rooms;

        public Hotel(string fullName, int category)
        {
            this.bookings = new BookingRepository();
            this.rooms = new RoomRepository();
            this.FullName = fullName;
            this.Category = category;
        }

        public string FullName 
        {
            get => this.fullName;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.HotelNameNullOrEmpty);
                }
                this.fullName = value;
            }
        }

        public int Category 
        {
            get => this.category;
            protected set
            {
                if (value < 1 || value > 5)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidCategory);
                }
                this.category = value;
            }
        }

        public double Turnover 
        {
            get=> SumTurnover();
        }

        public IRepository<IRoom> Rooms 
        { 
            get => this.rooms;
            set 
            { 
            } 
        }
        public IRepository<IBooking> Bookings 
        { 
            get => this.bookings;
            set
            {
            }
        }
        private double SumTurnover ()
        {
            double sum = 0;
            foreach (var booking in this.Bookings.All())
            {
                sum += booking.ResidenceDuration * booking.Room.PricePerNight;
            }
            return sum;
        }
    }
}
