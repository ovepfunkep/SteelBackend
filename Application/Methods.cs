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
                string idPart = "", phonePart = "", passwordPart = "";
                if (id == 0)
                {
                    if (string.IsNullOrEmpty(phone)) throw new ArgumentException("Arguments were not provided to the function!");
                    phonePart = $" Телефон = '{phone}'";
                    passwordPart = string.IsNullOrEmpty(password) ? "" : $" and Пароль = {password}";
                }
                else idPart = $" Id = {id}";

                List<List<object>> values = DataBase.Methods.GetCustom($"Select * from `Пользователи` where{idPart}{phonePart}{passwordPart}");
                if (values.Count == 0) return null;
                return values[0].ToUser();
            }

            public static User Add(User user)
            {
                
                int roleId = Convert.ToInt32(DataBase.Methods.GetCustom("Select * from `Роли` where `Название` = 'Клиент'")![0][0]);
                DataBase.Methods.Add("Пользователи", new object[] { roleId,
                                                                    user.Surname == null ? DBNull.Value : user.Surname,
                                                                    user.Name == null ? DBNull.Value : user.Name,
                                                                    user.MiddleName == null ? DBNull.Value : user.MiddleName,
                                                                    user.Email == null ? DBNull.Value : user.Email,
                                                                    user.Phone == null ? DBNull.Value : user.Phone,
                                                                    user.Password == null ? DBNull.Value : user.Password,
                                                                    user.Gender == null ? DBNull.Value : user.Gender,
                                                                    user.DateOfBirth == null ? DBNull.Value : user.DateOfBirth,
                                                                    user.Photo == null ? DBNull.Value : user.Photo,
                                                                    user.Vkontakte == null ? DBNull.Value : user.Vkontakte,
                                                                    user.Telegram == null ? DBNull.Value : user.Telegram,
                                                                    user.City == null ? DBNull.Value : user.City});
                return DataBase.Methods.GetCustom($"Select * from `Пользователи` where `Телефон` = '{user.Phone}'")[0].ToUser();

            }
        }

        public static class Activities
        {
            public static List<Activity> Get(List<int>? ids = null) => Generic.Get<Activity>("Направления", ids);

            public static List<Activity> GetByTeacherId(int teacherId) => 
                DataBase.Methods.GetCustom(@$"SELECT `Направления`.`Id`, `Название`, `Описание`, `Фотография`, `Длительность` 
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
                    User user = Users.Get(teacher.UserId);
                    List<Activity>? activities = needActivities == true ? Activities.GetByTeacherId(teacher.Id) : null;

                    extendedTeachers.Add(new ExtendedTeacher(teacher.Id,
                                                             teacher.UserId,
                                                             teacher.Experience,
                                                             teacher.Description,
                                                             user,
                                                             user.Photo,
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

            public static List<ExtendedTraining> GetUpcomingExtendedByUserId(int userId)
            {
                List<Training> trainings = DataBase.Methods.GetCustom($@"SELECT `Занятия`.`Id`, `Id направления`, `Id преподавателя`, `Дата и время`, `Общее кол-во мест`
FROM `Занятия` 
Inner join `Записи на занятия` on `Записи на занятия`.`Id занятия` = `Занятия`.`Id`
WHERE `Записи на занятия`.`Id пользователя` = {userId}
  And `Дата и время` > '{DateTime.Now.ToDBFormat()}'")
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
    }
}
