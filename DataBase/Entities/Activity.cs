using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class Activity
    {
        public int Id { get; set; } = default;
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Icon { get; set; }
        public int LastsInMinutes { get; set; } = default;

        public Activity(int id = default, string name = "", string description = "", string photo = "", string icon = "", int lastsInMinutes = default)
        {
            Id = id;
            Name = name;
            Description = description;
            Photo = photo;
            Icon = icon;
            LastsInMinutes = lastsInMinutes;
        }
    }
}
