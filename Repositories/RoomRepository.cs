using BookingApp.Models.Rooms.Contracts;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingApp.Repositories
{
    public class RoomRepository : IRepository<IRoom>
    {
        private readonly HashSet<IRoom> models = new HashSet<IRoom>();

        public RoomRepository()
        {

        }
        public void AddNew(IRoom model)
        {
            this.models.Add(model);
        }

        public IReadOnlyCollection<IRoom> All()
        {
            return models;
        }
        public IRoom Select(string criteria)
        {
            return models.FirstOrDefault(m => m.GetType().Name == criteria);
        }
    }
}
