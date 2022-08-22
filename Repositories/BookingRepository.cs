using BookingApp.Models.Bookings.Contracts;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingApp.Repositories
{
    public class BookingRepository : IRepository<IBooking>
    {
        private readonly HashSet<IBooking> models = new HashSet<IBooking>();
        public void AddNew(IBooking model)
        {
            this.models.Add(model);
        }

        public IReadOnlyCollection<IBooking> All()
        {
            return this.models;
        }

        public IBooking Select(string criteria)
        {
            return this.models.FirstOrDefault(m => m.BookingNumber.ToString() == criteria);
        }
    }
}
