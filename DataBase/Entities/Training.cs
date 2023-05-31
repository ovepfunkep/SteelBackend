using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class Training
    {
        public int Id { get; set; } 
        public int ActivityId { get; set; } 
        public int TeacherId { get; set; } 
        public DateTime DateTimeStart { get; set; } 
        public int TotalSeats { get; set; } 

        public Training(int id = default, int activityId = default, int teacherId = default, DateTime dateTimeStart = default, int totalSeats = default)
        {
            Id = id;
            ActivityId = activityId;
            TeacherId = teacherId;
            DateTimeStart = dateTimeStart;
            TotalSeats = totalSeats;
        }
    }

}
