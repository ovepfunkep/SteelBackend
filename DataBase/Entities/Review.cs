using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class Review
    {
        public int Id { get; set; } 
        public int TeacherId { get; set; } 
        public int UserId { get; set; } 
        public string Text { get; set; }
        public int Rate { get; set; } 

        public Review(int id = default, int teacherId = default, int userId = default, string text = "", int rate = default)
        {
            Id = id;
            TeacherId = teacherId;
            UserId = userId;
            Text = text;
            Rate = rate;
        }
    }
}
