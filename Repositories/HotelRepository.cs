using BookingApp.Models.Hotels.Contacts;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingApp.Repositories
{
    class HotelRepository : IRepository<IHotel>
    {
        private readonly HashSet<IHotel> models = new HashSet<IHotel>();
        public void AddNew(IHotel model)
        {
            this.models.Add(model);
        }

        public IReadOnlyCollection<IHotel> All()
        {
            return models;
        }

        public IHotel Select(string criteria)
        {
            return models.FirstOrDefault(m => m.FullName == criteria);
        }
    }
}
