using DataBase.Entities;
using DataBase.Entities.Extended;

namespace Application
{
    public class Methods
    {
        public static class Generic
        {
            public static List<T> Get<T>(string tableName, List<int>? ids = null)
            {
                List<T> result = new();
                if (ids != null)
                {
                    foreach (int id in ids)
                    {
                        try { result.Add(DataBase.Methods.GetCustom($"Select * from `{tableName}` where `Id` = {id}")[0].ToClassInstance<T>()); }
                        catch { }
                    }
                    return result;
                }
                else return DataBase.Methods.Get(tableName).Select(listOfObjects => listOfObjects.ToClassInstance<T>()).ToList();
            }
        }

        public static class Users
        {
            public static User? Get(int id = 0, string phone = "", string password = "")
            {
                string query = "Select * from `Пользователи`";
                if (id == 0)
                {
                    if (!string.IsNullOrEmpty(phone)) query += @$"
Where `Телефон` = '{phone}'";
                    query += string.IsNullOrEmpty(password) ? string.Empty : $" and Пароль = '{password}'";
                }
                else query += @$"
Where `Id` = {id}";

                List<User> users = DataBase.Methods.GetCustom(query)
                                                   .Select(lo => lo.ToUser())
                                                   .ToList();

                return users.Count > 0 ? users[0] : null;
            }

            public static List<User> Get() => DataBase.Methods.Get("Пользователи").Select(lo => lo.ToUser()).ToList();

            public static User Add(User user)
            {
                int roleId = Convert.ToInt32(DataBase.Methods.GetCustom("Select * from `Роли` where `Название` = 'Клиент'")![0][0]);

                DataBase.Methods.Add("Пользователи", new object[] { roleId,
                                                                    user.Surname,
                                                                    user.Name,
                                                                    user.MiddleName,
                                                                    user.Email,
                                                                    user.Phone,
                                                                    user.Password,
                                                                    user.Gender,
                                                                    user.DateOfBirth,
                                                                    user.Photo,
                                                                    user.Vkontakte,
                                                                    user.Telegram,
                                                                    user.City});

                return Get(phone: user.Phone);
            }

            public static User Update(User user)
            {
                DataBase.Methods.Update("Пользователи", new object[] { user.Id,
                                                                       user.RoleId,
                                                                       user.Surname,
                                                                       user.Name,
                                                                       user.MiddleName,
                                                                       user.Email,
                                                                       user.Phone,
                                                                       user.Password,
                                                                       user.Gender,
                                                                       user.DateOfBirth,
                                                                       user.Photo,
                                                                       user.Vkontakte,
                                                                       user.Telegram,
                                                                       user.City});

                return Get(user.Id);
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Пользователи", id);
        }

        public static class Activities
        {
            public static List<Activity> Get(List<int>? ids = null) => Generic.Get<Activity>("Направления", ids);
            
            public static Activity? Get(string name)
            {
                List<Activity> result = DataBase.Methods.GetCustom($@"Select * 
from `Направления`
where `Название` = '{name}'").Select(lo => lo.ToActivity()).ToList();

                return result.Count > 0 ? result[0] : null;
            }

            public static List<Activity> GetByTeacherId(int teacherId) =>
                DataBase.Methods.GetCustom(@$"SELECT `Направления`.`Id`, `Название`, `Описание`, `Фотография`, `Иконка`, `Длительность` 
FROM `Направления` 
INNER JOIN `Преподаватели направлений` on `Преподаватели направлений`.`Id направления` = `Направления`.`Id`
WHERE `Преподаватели направлений`.`Id преподавателя` = {teacherId}")
                                .Select(lo => lo.ToActivity())
                                .ToList();

            public static Activity Add(Activity activity)
            {
                DataBase.Methods.Add("Направления", new object[] { activity.Name,
                                                                   activity.Description,
                                                                   activity.Photo,
                                                                   activity.Icon,
                                                                   activity.LastsInMinutes});

                return Get(activity.Name);
            }

            public static Activity Update(Activity activity)
            {
                DataBase.Methods.Update("Направления", new object[] { activity.Id,
                                                                      activity.Name,
                                                                      activity.Description,
                                                                      activity.Photo,
                                                                      activity.Icon,
                                                                      activity.LastsInMinutes});



                return Get(new List<int>() { activity.Id })[0];
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Направления", id);
        }

        public static class Teachers
        {
            private static List<ExtendedTeacher> ConvertToExtended(List<Teacher> teachers, bool needActivities = false)
            {
                List<ExtendedTeacher> extendedTeachers = new();

                foreach (Teacher teacher in teachers)
                {
                    User user = Users.Get(teacher.UserId);
                    List<Activity>? activities = needActivities == true ? Activities.GetByTeacherId(teacher.Id) : null;

                    extendedTeachers.Add(new ExtendedTeacher(teacher.Id,
                                                             teacher.UserId,
                                                             teacher.Experience,
                                                             teacher.Description,
                                                             user,
                                                             activities));

                }

                return extendedTeachers;
            }

            public static List<Teacher> Get(List<int>? ids = null) => Generic.Get<Teacher>("Преподаватели", ids);

            public static Teacher? Get(int userId)
            {
                List<Teacher> result = DataBase.Methods.GetCustom($@"Select * 
from `Преподаватели`
where `Id пользователя` = '{userId}'").Select(lo => lo.ToTeacher()).ToList();

                return result.Count > 0 ? result[0] : null;
            }

            public static List<ExtendedTeacher> GetExtended(List<int>? ids = null, bool needActivities = false)
            {
                List<Teacher> teachers = Generic.Get<Teacher>("Преподаватели", ids);

                return ConvertToExtended(teachers, needActivities);
            }

            public static List<ExtendedTeacher> GetExtendedByActivity(int activityId)
            {
                List<Teacher> teachers = DataBase.Methods.GetCustom(@$"SELECT `Преподаватели`.`Id`, `Id пользователя`, `Стаж`, `Преподаватели`.`Описание`
FROM `Преподаватели` 
Inner join `Преподаватели направлений` on `Преподаватели направлений`.`Id преподавателя` = `Преподаватели`.`Id`
inner join `Направления` on `Направления`.`Id` = `Преподаватели направлений`.`Id направления`
WHERE `Id направления` = {activityId}")
                                                         .Select(lo => lo.ToTeacher())
                                                         .ToList();

                return ConvertToExtended(teachers);
            }

            public static Teacher Add(Teacher teacher)
            {
                DataBase.Methods.Add("Преподаватели", new object[] { teacher.UserId,
                                                                     teacher.Experience,
                                                                     teacher.Description});

                return Get(teacher.UserId);
            }

            public static Teacher Update(Teacher teacher)
            {
                DataBase.Methods.Update("Преподаватели", new object[] { teacher.Id,
                                                                        teacher.UserId,
                                                                        teacher.Experience,
                                                                        teacher.Description});

                return Get(new List<int>() { teacher.Id })[0];
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Преподаватели", id);
        }

        public static class News
        {
            public static List<DataBase.Entities.News> Get(List<int>? ids = null) =>
                Generic.Get<DataBase.Entities.News>("Новости", ids);

            public static DataBase.Entities.News? Get(string name)
            {
                List<DataBase.Entities.News> result = DataBase.Methods.GetCustom($@"Select * 
from `Новости`
where `Название` = '{name}'").Select(lo => lo.ToNews()).ToList();

                return result.Count > 0 ? result[0] : null;
            }

            public static DataBase.Entities.News Add(DataBase.Entities.News news)
            {
                DataBase.Methods.Add("Новости", new object[] { news.Name,
                                                               news.Description,
                                                               news.Text,
                                                               news.Photo});

                return Get(news.Name);
            }

            public static DataBase.Entities.News Update(DataBase.Entities.News news)
            {
                DataBase.Methods.Update("Новости", new object[] { news.Id,
                                                                  news.Name,
                                                                  news.Description,
                                                                  news.Text,
                                                                  news.Photo});

                return Get(new List<int>() { news.Id })[0];
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Новости", id);
        }

        public static class Trainings
        {
            static List<ExtendedTraining> ConvertToExtended(List<Training> trainings)
            {
                List<ExtendedTraining> extendedTrainings = new();

                foreach (Training training in trainings)
                {
                    Activity trainingActivity = Activities.Get(new List<int>() { training.ActivityId })[0];
                    ExtendedTeacher trainingTeacher = Teachers.GetExtended(new() { training.TeacherId })[0];

                    extendedTrainings.Add(new ExtendedTraining(training.Id,
                                                               training.ActivityId,
                                                               training.TeacherId,
                                                               training.DateTimeStart,
                                                               training.TotalSeats,
                                                               trainingActivity,
                                                               trainingTeacher,
                                                               training.DateTimeStart.AddMinutes(trainingActivity.LastsInMinutes),
                                                               training.LeftSeats()));

                }

                return extendedTrainings;
            }

            public static List<Training> Get(List<int>? ids = null) => Generic.Get<Training>("Занятия", ids);

            public static Training? Get(int teacherId, DateTime starts)
            {
                List<Training> result = DataBase.Methods.GetCustom($@"Select * 
from `Занятия`
where `Id преподавателя` = '{teacherId}'
and `Дата и время` = '{starts.ToDBFormat()}'").Select(lo => lo.ToTraining()).ToList();

                return result.Count > 0 ? result[0] : null;
            }

            public static List<Training> GetByDate(DateTime? start = null, DateTime? end = null)
            {
                List<List<object>> values;

                string query = "SELECT * FROM Занятия ";

                if (start != null && end != null)
                    query += $"WHERE `Дата и время` BETWEEN '{((DateTime)start).ToDBFormat()}' AND '{((DateTime)end).ToDBFormat()}'";
                else if (start == null && end != null)
                    query += $@"SELECT * FROM Занятия WHERE `Дата и время` <= '{((DateTime)end).ToDBFormat()}'";
                else if (start != null && end == null)
                    query += $@"SELECT * FROM Занятия WHERE `Дата и время` >= '{((DateTime)start).ToDBFormat()}'";

                values = DataBase.Methods.GetCustom(query);

                if (values.Count == 0) return new List<Training>();
                return values.Select(lo => lo.ToTraining()).ToList();
            }

            public static List<ExtendedTraining> GetExtended(DateTime? start = null, DateTime? end = null)
            {
                List<Training> trainings = GetByDate(start, end);

                return ConvertToExtended(trainings);
            }

            public static List<ExtendedTraining> GetExtendedByUserId(int userId, bool isUpcoming = true)
            {
                List<Training> trainings = DataBase.Methods.GetCustom($@"SELECT `Занятия`.`Id`, `Id направления`, `Id преподавателя`, `Дата и время`, `Общее кол-во мест`
FROM `Занятия` 
Inner join `Записи на занятия` on `Записи на занятия`.`Id занятия` = `Занятия`.`Id`
WHERE `Записи на занятия`.`Id пользователя` = {userId}" + (isUpcoming ? @$"
  And `Дата и время` > '{DateTime.Now.ToDBFormat()}'" : string.Empty))
                                                           .Select(lo => lo.ToTraining())
                                                           .ToList();

                return ConvertToExtended(trainings);
            }

            public static Training Add(Training training)
            {
                DataBase.Methods.Add("Занятия", new object[] { training.ActivityId,
                                                               training.TeacherId,
                                                               training.DateTimeStart,
                                                               training.TotalSeats});

                return Get(training.TeacherId, training.DateTimeStart);
            }

            public static Training Update(Training training)
            {
                DataBase.Methods.Update("Занятия", new object[] { training.Id,
                                                                  training.ActivityId,
                                                                  training.TeacherId,
                                                                  training.DateTimeStart,
                                                                  training.TotalSeats});

                return Get(new List<int>() { training.Id })[0];
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Занятия", id);
        }

        public static class Appointments
        {
            public static Appointment? Get(int trainingId, int userId)
            {
                List<Appointment> result = DataBase.Methods.GetCustom($@"Select * 
from `Записи на занятия`
where `Id занятия` = '{trainingId}'
and `Id пользователя` = {userId}").Select(lo => lo.ToAppointment()).ToList();

                return result.Count > 0 ? result[0] : null;
            }

            public static List<Appointment> Get(List<int>? ids = null) => Generic.Get<Appointment>("Записи на занятия", ids);

            public static List<Appointment> Get(int trainingId) => DataBase.Methods.GetCustom($@"Select * 
from `Записи на занятия`
where `Id занятия` = '{trainingId}'").Select(lo => lo.ToAppointment()).ToList();

            static List<ExtendedAppointment> ConvertToExtended(List<Appointment> appointments)
            {
                List<ExtendedAppointment> extendedAppointments = new();

                foreach (Appointment appointment in appointments)
                {
                    User user = Users.Get(appointment.UserId);
                    Training training = Trainings.Get(new() { appointment.TrainingId })[0];

                    extendedAppointments.Add(new ExtendedAppointment(appointment.Id,
                                                                     appointment.TrainingId, 
                                                                     appointment.UserId, 
                                                                     user, 
                                                                     training));

                }

                return extendedAppointments;
            }

            public static List<ExtendedAppointment> GetExtended(int trainingId)
            {
                List<Appointment> appointments = Get(trainingId);

                return ConvertToExtended(appointments);
            }

            public static Appointment Add(Appointment appointment)
            {
                DataBase.Methods.Add("Записи на занятия", new object[] { appointment.TrainingId, 
                                                                         appointment.UserId });

                return Get(appointment.TrainingId, appointment.UserId);
            }

            public static Appointment Update(Appointment appointment)
            {
                DataBase.Methods.Update("Записи на занятия", new object[] { appointment.Id,
                                                                            appointment.TrainingId,
                                                                            appointment.UserId});

                return Get(appointment.TrainingId, appointment.UserId);
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Записи на занятия", id);
        }
        
        public static class Attendees
        {
            public static Attend? Get(int trainingId, int userId)
            {
                List<Attend> result = DataBase.Methods.GetCustom($@"Select * 
from `Посетители занятия`
where `Id занятия` = '{trainingId}'
and `Id пользователя` = {userId}").Select(lo => lo.ToAttend()).ToList();

                return result.Count > 0 ? result[0] : null;
            }

            public static List<Attend> Get(List<int>? ids = null) => Generic.Get<Attend>("Посетители занятия", ids);

            public static Attend Add(Attend appointment)
            {
                DataBase.Methods.Add("Посетители занятия", new object[] { appointment.TrainingId, 
                                                                         appointment.UserId });

                return Get(appointment.TrainingId, appointment.UserId);
            }

            public static Attend Update(Attend appointment)
            {
                DataBase.Methods.Update("Посетители занятия", new object[] { appointment.Id,
                                                                            appointment.TrainingId,
                                                                            appointment.UserId});

                return Get(appointment.TrainingId, appointment.UserId);
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Посетители занятия", id);
        }

        public static class Achievements
        {
            public static List<Achievement> GetByUserId(int userId)
            {

                List<Achievement> achievements = DataBase.Methods.GetCustom($@"SELECT `Ачивки`.`Id`, `Название`, `Описание`, `Фотография` 
FROM `Ачивки`
INNER JOIN `Ачивки пользователей` on `Ачивки пользователей`.`Id ачивки` = `Ачивки`.`Id`
WHERE `Id пользователя` = {userId}")
                                                                 .Select(lo => lo.ToAchievement())
                                                                 .ToList();

                return achievements;
            }

            public static Achievement? Get(string name)
            {
                List<Achievement> result = DataBase.Methods.GetCustom($@"Select * 
from `Ачивки`
where `Название` = '{name}'").Select(lo => lo.ToAchievement()).ToList();

                return result.Count > 0 ? result[0] : null;
            }

            public static List<Achievement> Get(List<int>? ids = null) => Generic.Get<Achievement>("Ачивки", ids);

            public static Achievement Add(Achievement achievement)
            {
                DataBase.Methods.Add("Ачивки", new object[] { achievement.Name,
                                                                   achievement.Description,
                                                                   achievement.Photo});

                return Get(achievement.Name);
            }

            public static Achievement Update(Achievement achievement)
            {
                DataBase.Methods.Update("Ачивки", new object[] { achievement.Id,
                                                                  achievement.Name,
                                                                  achievement.Description,
                                                                  achievement.Photo});

                return Get(achievement.Name);
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Ачивки", id);
        }
        
        public static class UsersAchievements
        {
            public static UserAchievements? Get(int userId, int achievementId)
            {
                List<UserAchievements> result = DataBase.Methods.GetCustom($@"Select * 
from `Ачивки пользователей`
where `Id пользователя` = '{userId}'
and `Id ачивки` = {achievementId}").Select(lo => lo.ToUserAchievements()).ToList();

                return result.Count > 0 ? result[0] : null;
            }

            public static List<UserAchievements> Get(List<int>? ids = null) => Generic.Get<UserAchievements>("Ачивки пользователей", ids);

            public static UserAchievements Add(UserAchievements userAchievements)
            {
                DataBase.Methods.Add("Ачивки пользователей", new object[] { userAchievements.UserId,
                                                                            userAchievements.AchievementId});

                return Get(userAchievements.UserId, userAchievements.AchievementId);
            }

            public static UserAchievements Update(UserAchievements userAchievements)
            {
                DataBase.Methods.Update("Ачивки пользователей", new object[] { userAchievements.Id,
                                                                               userAchievements.UserId,
                                                                               userAchievements.AchievementId});

                return Get(userAchievements.UserId, userAchievements.AchievementId);
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Ачивки пользователей", id);
        }
        
        public static class ActivitiesTeachers
        {
            public static ActivityTeacher? Get(int activityId, int teacherId)
            {
                List<ActivityTeacher> result = DataBase.Methods.GetCustom($@"Select * 
from `Преподаватели направлений`
where `Id направления` = '{activityId}'
and `Id преподавателя` = {teacherId}").Select(lo => lo.ToActivityTeacher()).ToList();

                return result.Count > 0 ? result[0] : null;
            }

            public static List<ActivityTeacher> Get(List<int>? ids = null) => Generic.Get<ActivityTeacher>("Преподаватели направлений", ids);

            public static ActivityTeacher Add(ActivityTeacher activityTeacher)
            {
                DataBase.Methods.Add("Преподаватели направлений", new object[] { activityTeacher.ActivityId,
                                                                            activityTeacher.TeacherId});

                return Get(activityTeacher.ActivityId, activityTeacher.TeacherId);
            }

            public static ActivityTeacher Update(ActivityTeacher activityTeacher)
            {
                DataBase.Methods.Update("Преподаватели направлений", new object[] { activityTeacher.Id,
                                                                               activityTeacher.ActivityId,
                                                                               activityTeacher.TeacherId});

                return Get(activityTeacher.ActivityId, activityTeacher.TeacherId);
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Преподаватели направлений", id);
        }

        public static class Reviews
        {
            public static Review? Get(int teacherId, int userId)
            {
                List<Review> result = DataBase.Methods.GetCustom($@"SELECT `Id`, `Id преподавателя`, `Id пользователя`, `Текст`, `Оценка` 
FROM `Отзывы преподавателям`
WHERE `Id преподавателя` = {teacherId}
 And `Id пользователя` = {userId}")
                                                      .Select(lo => lo.ToReview())
                                                      .ToList();

                return result.Count > 0 ? result[0] : null;
            }

            public static List<Review> Get(List<int>? ids = null) => Generic.Get<Review>("Отзывы преподавателям", ids);

            private static List<ExtendedReview> ConvertToExtended(List<Review> reviews)
            {
                List<ExtendedReview> extendedReviews = new();

                foreach (Review review in reviews)
                {
                    User user = Users.Get(review.UserId);
                    Teacher teacher = Teachers.GetExtended(new() { review.TeacherId })[0];

                    extendedReviews.Add(new ExtendedReview(review.Id,
                                                           review.TeacherId,
                                                           review.UserId,
                                                           review.Text,
                                                           review.Rate,
                                                           user,
                                                           teacher));
                }

                return extendedReviews;
            }

            public static List<ExtendedReview> GetExtended(int teacherId = 0, int userId = 0)
            {
                string query = @"SELECT `Id`, `Id преподавателя`, `Id пользователя`, `Текст`, `Оценка` 
FROM `Отзывы преподавателям`";
                if (teacherId > 0) query += $@"
WHERE `Id преподавателя` = {teacherId}" + (userId > 0 ? $" And `Id пользователя` = {userId}" : string.Empty);
                else if (userId > 0) query += $@"
WHERE `Id пользователя` = {userId}";

                List<Review> reviews = DataBase.Methods.GetCustom(query)
                                                       .Select(lo => lo.ToReview())
                                                       .ToList();

                return ConvertToExtended(reviews);
            }

            public static Review Add(Review review)
            {
                DataBase.Methods.Add("Отзывы преподавателям", new object[] { review.TeacherId,
                                                                             review.UserId,
                                                                             review.Text,
                                                                             review.Rate});

                return Get(review.TeacherId, review.UserId);
            }

            public static Review Update(Review review)
            {
                DataBase.Methods.Update("Отзывы преподавателям", new object[] { review.Id,
                                                                                review.TeacherId,
                                                                                review.UserId,
                                                                                review.Text,
                                                                                review.Rate});

                return Get(new List<int>() { review.Id })[0];
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Отзывы преподавателям", id);
        }
    }
}
