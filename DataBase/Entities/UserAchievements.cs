using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities
{
    public class UserAchievements
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievementId { get; set; }

        public UserAchievements(int id = default, int userId = default, int achievementId = default)
        {
            Id = id;
            UserId = userId;
            AchievementId = achievementId;
        }
    }
}
