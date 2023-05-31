using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities.Extended
{
    public class ExtendedReview : Review
    {
        public User User { get; set; } = new();
        public Teacher Teacher { get; set; } = new();

        public ExtendedReview(int id,
                              int teacherId,
                              int userId,
                              string text,
                              int rate,
                              User user, 
                              Teacher teacher) : base(id, teacherId, userId, text, rate)
        {
            User = user;
            Teacher = teacher;
        }
    }
}
