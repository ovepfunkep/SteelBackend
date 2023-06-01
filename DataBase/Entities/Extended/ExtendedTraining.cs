using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities.Extended
{
    public class ExtendedTraining : Training
    {
        public Activity Activity { get; set; }
        public ExtendedTeacher ExtendedTeacher { get; set; }
        public DateTime DateTimeEnd { get; set; } 
        public int LeftSeats { get; set; } 

        public ExtendedTraining(int id, 
                                int activityId, 
                                int teacherId, 
                                DateTime dateTimeStart, 
                                int totalSeats,
                                Activity activity, 
                                ExtendedTeacher teacher, 
                                DateTime dateTimeEnd, 
                                int leftSeats) : base(id, activityId, teacherId, dateTimeStart, totalSeats)
        {
            Activity = activity;
            ExtendedTeacher = teacher;
            DateTimeEnd = dateTimeEnd;
            LeftSeats = leftSeats;
        }
    }
}
