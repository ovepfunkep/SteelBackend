using MySqlConnector;

namespace DataBase
{
    public class Methods
    {
        static MySqlConnection Connection = new("server=127.0.0.1;uid=root;pwd=8P9h1p3m4y7a4d9m5i1n5!;database=steelDataBase");
        static bool IsConnected { get; set; } = false;

        public static bool OpenConnection()
        {
            try
            {
                if (!IsConnected)
                {
                    Connection.Open();
                    IsConnected = true;
                }
                return true;
            }
            catch { return false; }
        }

        public static void CloseConnection()
        {
            if (IsConnected) 
            {
                Connection.Close();
                IsConnected = false;
            }
        }

        /// <summary>
        /// Выполнить выборку по заготовленному запросу
        /// </summary>
        /// <param name="query">Текст запроса</param>
        /// <returns>Список записей выбороки</returns>
        public static List<List<object>> GetCustom(string query)
        {
            try
            {
                OpenConnection();
                MySqlCommand mySqlCommand = new(query, Connection);
                List<List<object>> result = new();

                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    int count = 0;
                    while (mySqlDataReader.Read())
                    {
                        result.Add(new List<object>());
                        for (int i = 0; i < mySqlDataReader.FieldCount; i++)
                            result[count].Add(mySqlDataReader[i]);
                        count++;
                    }
                }
                mySqlDataReader.Close();
                CloseConnection();
                return result;
            }
            catch
            {
                CloseConnection();
                return new List<List<object>>();
            }
        }

        /// <summary>
        /// Достать записи из таблицы по названию
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <returns>Список записей, состоящие из списка объектов данных</returns>
        public static List<List<object>> Get(string tableName) => GetCustom($"Select * from {tableName}");

        /// <summary>
        /// Добавить запись в конкретную таблицу
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="dataValues">ВВОДИТЬ СТРОГО ПО ПОРЯДКУ значения для записи в таблицу</param>
        /// <returns>Истину, если получилось. Иначе ложь</returns>
        public static bool Add(string tableName, params object?[] dataValues)
        {
            try
            {
                OpenConnection();
                MySqlCommand mySqlCommand = new($"Insert into `{tableName}` values (NULL, ", Connection);

                int countParams = 0;
                foreach (object? value in dataValues)
                {
                    mySqlCommand.CommandText += $"@value{countParams}, ";
                    mySqlCommand.Parameters.AddWithValue($"value{countParams}", dataValues[countParams++] ?? DBNull.Value);
                }
                mySqlCommand.CommandText = mySqlCommand.CommandText[..^2] + ")";

                mySqlCommand.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            catch
            {
                CloseConnection();
                return false;
            }
        }

        /// <summary>
        /// Обновить конкретную запись из конкретной таблицы
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="dataValues">ВВОДИТЬ СТРОГО ПО ПОРЯДКУ значения для записи в таблицу</param>
        /// <returns>Истину, если получилось. Иначе ложь</returns>
        public static bool Update(string tableName, params object?[] dataValues)
        {
            try
            {
                OpenConnection();
                MySqlCommand mySqlCommand = new($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'", Connection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                string newQuery = $"Update {tableName} Set ";
                for (int i = 0; i < dataValues.Length; i++)
                {
                    mySqlDataReader.Read();
                    newQuery += $"{mySqlDataReader[0]}=@value{i}, ";
                    mySqlCommand.Parameters.AddWithValue($"value{i}", dataValues[i]);
                }
                mySqlDataReader.Close();
                mySqlCommand.CommandText = newQuery[..^2] + " Where Id = @id";
                mySqlCommand.Parameters.AddWithValue("id", dataValues[0]);

                mySqlCommand.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            catch 
            {
                CloseConnection();
                return false;
            }
        }

        /// <summary>
        /// Удалить конкретную запись из конкретной таблицы
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="id">Id записи</param>
        /// <returns>Истину, если получилось. Иначе ложь</returns>
        public static bool Delete(string tableName, int id) => ExecuteCustomQuery($"Delete from {tableName} WHERE Id = {id}");

        /// <summary>
        /// Выполнить запрос в базу данных
        /// </summary>
        /// <param name="query">Текст запроса</param>
        /// <returns>Истину, если получилось. Иначе ложь</returns>
        public static bool ExecuteCustomQuery(string query)
        {
            try
            {
                OpenConnection();
                MySqlCommand mySqlCommand = new(query, Connection);

                mySqlCommand.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            catch 
            {
                CloseConnection();
                return false;
            }
        }
    }
}