using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class News
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string Photo { get; set; }

        public News(int id = default, string name = "", string description = "", string text = "", string photo = "")
        {
            Id = id;
            Name = name;
            Description = description;
            Text = text;
            Photo = photo;
        }
    }
}
