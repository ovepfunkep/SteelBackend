using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class ActivityTeacher
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int TeacherId { get; set; }

        public ActivityTeacher(int id = default, int activityId = default, int teacherId = default)
        {
            Id = id;
            ActivityId = activityId;
            TeacherId = teacherId;
        }
    }
}
