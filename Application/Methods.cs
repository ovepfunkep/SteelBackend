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
                        try { result.Add(DataBase.Methods.GetCustom($"Select * from {tableName} where `Id` = {id}")[0].ToClassInstance<T>()); }
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
WHERE `Преподаватели направлений`.`Id` = {teacherId}")
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
                DataBase.Methods.Add("Направления", new object[] { teacher.UserId,
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
                    Teacher trainingTeacher = Teachers.GetExtended(new() { training.TeacherId })[0];

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

            public static List<Training> Get(DateTime? start = null, DateTime? end = null)
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
                List<Training> trainings = Get(start, end);

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

            public static Appointment Add(int trainingId, int userId)
            {
                DataBase.Methods.Add("Записи на занятия", new object[] { trainingId, userId });

                return Get(trainingId, userId);
            }

            public static Appointment Update(Appointment appointment)
            {
                DataBase.Methods.Update("Записи на занятия", new object[] { appointment.Id,
                                                                            appointment.TrainingId,
                                                                            appointment.UserId});

                return Get(new List<int>() { appointment.Id })[0];
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Записи на занятия", id);
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
                DataBase.Methods.Add("Направления", new object[] { achievement.Name,
                                                                   achievement.Description,
                                                                   achievement.Photo});

                return Get(achievement.Name);
            }

            public static Achievement Update(Achievement achievement)
            {
                DataBase.Methods.Update("Занятия", new object[] { achievement.Id,
                                                                  achievement.Name,
                                                                  achievement.Description,
                                                                  achievement.Photo});

                return Get(new List<int>() { achievement.Id })[0];
            }

            public static bool Delete(int id) => DataBase.Methods.Delete("Ачивки", id);
        }

        public static class Reviews
        {
            public static Review? Get(int teacherId = 0, int userId = 0)
            {
                string query = @"SELECT `Id`, `Id преподавателя`, `Id пользователя`, `Текст`, `Оценка` 
FROM `Отзывы преподавателям`";
                if (teacherId > 0) query += $@"
WHERE `Id преподавателя` = {teacherId}" + (userId > 0 ? $" And `Id пользователя` = {userId}" : string.Empty);
                else if (userId > 0) query += $@"
WHERE `Id пользователя` = {userId}";

                List<Review> result = DataBase.Methods.GetCustom(query).Select(lo => lo.ToReview()).ToList();

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

            public static bool Delete(int teacherId, int userId) =>
                DataBase.Methods.Delete("Отзывы преподавателям", Get(teacherId, userId).Id);
        }
    }
}
