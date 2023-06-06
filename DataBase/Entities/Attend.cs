﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class Attend
    {
        public int Id { get; set; }
        public int TrainingId { get; set; }
        public int UserId { get; set; }

        public Attend(int id = default, int trainingId = default, int userId = default)
        {
            Id = id;
            TrainingId = trainingId;
            UserId = userId;
        }
    }
}
