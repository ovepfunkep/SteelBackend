using DataBase.Entities;
using DataBase.Entities.Extended;

namespace Application
{
    public static class Extensions
    {
        public static T ToClassInstance<T>(this List<object> list) =>
            typeof(T).Name switch
            {
                nameof(User) => (T)(object)list.ToUser(),
                nameof(Activity) => (T)(object)list.ToActivity(),
                nameof(Teacher) => (T)(object)list.ToTeacher(),
                nameof(News) => (T)(object)list.ToNews(),
                nameof(Training) => (T)(object)list.ToTraining(),
                nameof(Achievement) => (T)(object)list.ToAchievement(),
                nameof(Appointment) => (T)(object)list.ToAppointment(),
                nameof(Attend) => (T)(object)list.ToAttend(),
                nameof(Review) => (T)(object)list.ToReview(),
                nameof(UserAchievements) => (T)(object)list.ToUserAchievements(),
                nameof(ActivityTeacher) => (T)(object)list.ToActivityTeacher(),
                _ => throw new ArgumentException($"There's now such class as {typeof(T).Name}")
            };

        //List<object> to class instance converters
        
        public static User ToUser(this List<object> list) => new(list[0].ToInt(),
                                                                 list[1].ToInt(),
                                                                 list[2].ToStr(),
                                                                 list[3].ToStr(),
                                                                 list[4].ToStr(),
                                                                 list[5].ToStr(),
                                                                 list[6].ToStr(),
                                                                 list[7].ToStr(),
                                                                 list[8].ToStr(),
                                                                 list[9].ToDateTime(),
                                                                 list[10].ToStr(),
                                                                 list[11].ToStr(),
                                                                 list[12].ToStr());

        public static Activity ToActivity(this List<object> list) => new(list[0].ToInt(),
                                                                         list[1].ToStr(),
                                                                         list[2].ToStr(),
                                                                         list[3].ToStr(),
                                                                         list[4].ToStr(),
                                                                         list[5].ToInt());

        public static Teacher ToTeacher(this List<object> list) => new(list[0].ToInt(),
                                                                       list[1].ToInt(),
                                                                       list[2].ToFloat(),
                                                                       list[3].ToStr());

        public static News ToNews(this List<object> list) => new(list[0].ToInt(),
                                                                 list[1].ToStr(),
                                                                 list[2].ToStr(),
                                                                 list[3].ToStr(),
                                                                 list[4].ToStr());

        public static Training ToTraining(this List<object> list) => new(list[0].ToInt(),
                                                                         list[1].ToInt(),
                                                                         list[2].ToInt(),
                                                                         list[3].ToDateTime(),
                                                                         list[4].ToInt());

        public static Achievement ToAchievement(this List<object> list) => new(list[0].ToInt(),
                                                                               list[1].ToStr(),
                                                                               list[2].ToStr(),
                                                                               list[3].ToStr());
        
        public static Appointment ToAppointment(this List<object> list) => new(list[0].ToInt(),
                                                                               list[1].ToInt(),
                                                                               list[2].ToInt());
        
        public static UserAchievements ToUserAchievements(this List<object> list) => new(list[0].ToInt(),
                                                                               list[1].ToInt(),
                                                                               list[2].ToInt());
        
        public static Attend ToAttend(this List<object> list) => new(list[0].ToInt(),
                                                                               list[1].ToInt(),
                                                                               list[2].ToInt());
        
        public static ActivityTeacher ToActivityTeacher(this List<object> list) => new(list[0].ToInt(),
                                                                               list[1].ToInt(),
                                                                               list[2].ToInt());
        
        public static Review ToReview(this List<object> list) => new(list[0].ToInt(),
                                                                     list[1].ToInt(),
                                                                     list[2].ToInt(),
                                                                     list[3].ToStr(),
                                                                     list[4].ToInt());

        public static int LeftSeats(this Training training) => DataBase.Methods.GetCustom(@$"SELECT `Занятия`.`Общее кол-во мест` - Занято
FROM `Занятия`
INNER JOIN (SELECT COUNT(Id) AS Занято
            FROM `Записи на занятия`
            WHERE `Id занятия` = {training.Id}) AS Записи
WHERE `Id` = {training.Id}")[0][0].ToInt();

        public static string ToDBFormat(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd HH:mm:ss");

        //Objects to types converters
        public static int ToInt(this object value) => Convert.IsDBNull(value) ? default : Convert.ToInt32(value);

        public static string ToStr(this object value) => Convert.IsDBNull(value) ? string.Empty : Convert.ToString(value)!;

        public static DateTime ToDateTime(this object value) => Convert.IsDBNull(value) ? default : Convert.ToDateTime(value);

        public static float ToFloat(this object value) => (float)(Convert.IsDBNull(value) ? default : Convert.ToDouble(value));
    }
}
