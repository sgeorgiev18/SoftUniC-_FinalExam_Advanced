using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.Models.Rooms
{
    class Apartment : Room
    {
        private const int apartmentBedCapacity = 6;
        public Apartment()
            : base(apartmentBedCapacity)
        {
        }
    }
}
