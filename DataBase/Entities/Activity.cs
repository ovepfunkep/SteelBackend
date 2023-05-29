using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int LastsInMinutes { get; set; }

        public Activity(int id, string name, string description, string photo, int lastsInMinutes)
        {
            Id = id;
            Name = name;
            Description = description;
            Photo = photo;
            LastsInMinutes = lastsInMinutes;
        }
    }
}
