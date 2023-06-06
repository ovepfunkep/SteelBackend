using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities.Extended
{
    public class ExtendedAppointment : Appointment
    {
        public User User { get; set; } = new();
        public Training Training { get; set; } = new();
        public ExtendedAppointment(int id,
                              int trainingId,
                              int userId,
                              User user,
                              Training training) : base(id, trainingId, userId)
        {
            User = user;
            Training = training;
        }
    }
}
