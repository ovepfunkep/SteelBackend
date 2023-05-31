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
            public static List<User> Get(int id = 0, string phone = "", string password = "")
            {
                string query = "Select * from `Пользователи`";
                if (id == 0)
                {
                    if (!string.IsNullOrEmpty(phone)) query += @$"
Where `Телефон` = '{phone}'";
                    query += string.IsNullOrEmpty(password) ? string.Empty : $" and Пароль = {password}";
                }
                else query += @$"
Where `Id` = {id}";

                List<List<object>> values = DataBase.Methods.GetCustom(query);
                if (values.Count == 0) return new List<User>();
                return values.Select(lo => lo.ToUser()).ToList();
            }

            public static List<User> Get() => DataBase.Methods.Get("Пользователи").Select(lo => lo.ToUser()).ToList();

            public static User? Add(User user)
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

                List<User> dbUser = Get(phone: user.Phone);

                return dbUser.Count > 0 ? dbUser[0] : null;
            }

            public static User? Update(User user)
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



                List<User> dbUser = Get(phone: user.Phone);

                return dbUser.Count > 0 ? dbUser[0] : null;
            }
        }

        public static class Activities
        {
            public static List<Activity> Get(List<int>? ids = null) => Generic.Get<Activity>("Направления", ids);

            public static List<Activity> GetByTeacherId(int teacherId) =>
                DataBase.Methods.GetCustom(@$"SELECT `Направления`.`Id`, `Название`, `Описание`, `Фотография`, `Иконка`, `Длительность` 
FROM `Направления` 
INNER JOIN `Преподаватели направлений` on `Преподаватели направлений`.`Id направления` = `Направления`.`Id`
WHERE `Преподаватели направлений`.`Id` = {teacherId}")
                                .Select(lo => lo.ToActivity())
                                .ToList();
        }

        public static class Teachers
        {
            private static List<ExtendedTeacher> ConvertToExtended(List<Teacher> teachers, bool needActivities = false)
            {
                List<ExtendedTeacher> extendedTeachers = new();

                foreach (Teacher teacher in teachers)
                {
                    User user = Users.Get(teacher.UserId)[0];
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
        }

        public static class News
        {
            public static List<DataBase.Entities.News> Get(List<int>? ids = null) => Generic.Get<DataBase.Entities.News>("Новости", ids);
        }

        public static class Trainings
        {
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
        }

        public static class Appointments
        {
            public static bool Get(int trainingId, int userId) => DataBase.Methods.GetCustom(@$"Select * from `Записи на занятия`
                                                                                                   where `Id занятия` = {trainingId} 
                                                                                                   and `Id пользователя` = {userId}").Count > 0;

            public static bool Add(int trainingId, int userId) => DataBase.Methods.Add("Записи на занятия", new object[] { trainingId, userId });
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
        }

        public static class Reviews
        {
            public static List<Review> Get(int teacherId = 0, int userId = 0)
            {
                string query = @"SELECT `Id`, `Id преподавателя`, `Id пользователя`, `Текст`, `Оценка` 
FROM `Отзывы преподавателям`";
                if (teacherId > 0) query += $@"
WHERE `Id преподавателя` = {teacherId}" + (userId > 0 ? $" And `Id пользователя` = {userId}" : string.Empty);
                else if (userId > 0) query += $@"
WHERE `Id пользователя` = {userId}";

                return DataBase.Methods.GetCustom(query)
                                       .Select(lo => lo.ToReview())
                                       .ToList();
            }

            private static List<ExtendedReview> ConvertToExtended(List<Review> reviews)
            {
                List<ExtendedReview> extendedReviews = new();

                foreach (Review review in reviews)
                {
                    User user = Users.Get(review.UserId)[0];
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

            public static Review? Add(Review review)
            {
                DataBase.Methods.Add("Отзывы преподавателям", new object[] { review.TeacherId,
                                                                             review.UserId,
                                                                             review.Text,
                                                                             review.Rate});

                List<Review> dbReview = Get(review.TeacherId, review.UserId);

                return dbReview.Count > 0 ? dbReview[0] : null;
            }

            public static bool Delete(int teacherId, int userId) => DataBase.Methods.Delete("Отзывы преподавателям", Get(teacherId, userId)[0].Id);
        }
    }
}
