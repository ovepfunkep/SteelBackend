using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class Achievement
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }

        public Achievement(int id = default, string name = "", string description = "", string photo = "")
        {
            Id = id;
            Name = name;
            Description = description;
            Photo = photo;
        }
    }
}
