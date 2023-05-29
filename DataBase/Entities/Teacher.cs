using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float? Experience { get; set; }
        public string? Description { get; set; }

        public Teacher(int id, int userId, float? expirience, string? description)
        {
            Id = id;
            UserId = userId;
            Experience = expirience;
            Description = description;
        }
    }

}
