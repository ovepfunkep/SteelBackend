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
                _ => throw new ArgumentException($"There's now such class as {typeof(T).Name}")
            };

        public static User ToUser(this List<object> list) => new(Convert.ToInt32(list[0]),
                                                                 Convert.ToInt32((int)list[1]),
                                                                 list[6] as string,
                                                                 list[7] as string,
                                                                 list[2] as string,
                                                                 list[3] as string,
                                                                 list[4] as string,
                                                                 list[5] as string,
                                                                 list[8] as string,
                                                                 list[9] as DateTime?,
                                                                 list[10] as string,
                                                                 list[11] as string,
                                                                 list[12] as string,
                                                                 list[13] as string);

        public static Activity ToActivity(this List<object> list) => new(Convert.ToInt32(list[0]),
                                                                         list[1] as string,
                                                                         list[2] as string,
                                                                         list[3] as string,
                                                                         Convert.ToInt32(list[4]));

        public static Teacher ToTeacher(this List<object> list) => new(Convert.ToInt32(list[0]),
                                                                       Convert.ToInt32(list[1]),
                                                                       (float)Convert.ToDouble(list[2]),
                                                                       list[3] as string);

        public static News ToNews(this List<object> list) => new(Convert.ToInt32(list[0]),
                                                                  list[1] as string,
                                                                  list[2] as string,
                                                                  list[3] as string,
                                                                  list[4] as string);

        public static Training ToTraining(this List<object> list) => new(Convert.ToInt32(list[0]),
                                                                          Convert.ToInt32(list[1]),
                                                                          Convert.ToInt32(list[2]),
                                                                          Convert.ToDateTime(list[3]),
                                                                          Convert.ToInt32(list[4]));


        public static int LeftSeats(this Training training) => Convert.ToInt32(DataBase.Methods.GetCustom(@$"SELECT `Занятия`.`Общее кол-во мест` - Занято
FROM `Занятия`
INNER JOIN (SELECT COUNT(Id) AS Занято
            FROM `Записи на занятия`
            WHERE `Id занятия` = {training.Id}) AS Записи
WHERE `Id` = {training.Id}")[0][0]);

        public static string ToDBFormat(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
