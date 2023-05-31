using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities.Extended
{
    public class ExtendedTeacher : Teacher
    {
        public User User { get; set; } = new();
        public List<Activity>? Activities { get; set; } = null;

        public ExtendedTeacher(int id,
                               int userId,
                               float experience,
                               string description,
                               User user,
                               List<Activity>? activities = null) : base(id, userId, experience, description)
        {
            User = user;
            Activities = activities;
        }
    }
}
