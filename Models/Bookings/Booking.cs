﻿using BookingApp.Models.Bookings.Contracts;
using BookingApp.Models.Rooms.Contracts;
using BookingApp.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.Models.Bookings
{
    public class Booking : IBooking
    {
        private int residenceDuration;
        private int adultsCount;
        private int childrenCount;

        public Booking(IRoom room, int residenceDuration, int adultsCount, int childrenCount, int bookingNumber)
        {
            this.Room = room;
            this.ResidenceDuration = residenceDuration;
            this.AdultsCount = adultsCount;
            this.ChildrenCount = childrenCount;
            this.BookingNumber = bookingNumber;
        }
        public IRoom Room { get; set; }

        public int ResidenceDuration 
        {
            get => this.residenceDuration;
            protected set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(ExceptionMessages.DurationZeroOrLess);
                }
                this.residenceDuration = value;
            }
        }

        public int AdultsCount 
        {
            get => this.adultsCount;
            protected set
            {
                if (value < 1)
                {
                    throw new ArgumentException(ExceptionMessages.AdultsZeroOrLess);
                }
                adultsCount = value;
            }

        }

        public int ChildrenCount 
        {
            get => this.childrenCount;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.ChildrenNegative);
                }
                this.childrenCount = value;
            }
        }

        public int BookingNumber { get; set; }

        public string BookingSummary()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Booking number: {this.BookingNumber}");
            sb.AppendLine($"Room type: {this.Room.GetType().Name}");
            sb.AppendLine($"Adults: {this.AdultsCount} Children: {this.ChildrenCount}");
            sb.AppendLine($"Total amount paid: {TotalPaid():F2} $");
            return sb.ToString();
        }

        private double TotalPaid() => Math.Round(this.ResidenceDuration * this.Room.PricePerNight, 2);
    }
}
