namespace DataBase.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public int RoleId { get; set; } = UserRoleId;
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; } 
        public string Photo { get; set; }
        public string Vkontakte { get; set; }
        public string Telegram { get; set; }
        public string City { get; set; }

        private static int UserRoleId = Convert.ToInt32(Methods.GetCustom("Select * from `Роли` where `Название` = 'Клиент'")![default][default]);

        public User(int id = default, 
                    int roleId = default,
                    string surname = "", 
                    string name = "", 
                    string middleName = "", 
                    string email = "",
                    string phone = "",
                    string password = "",
                    string gender = "", 
                    DateTime dateOfBirth = default, 
                    string photo = "", 
                    string vkontakte = "", 
                    string telegram = "", 
                    string city = "")
        {
            Id = id;
            RoleId = roleId != default ? roleId : UserRoleId;
            Surname = surname;
            Name = name;
            MiddleName = middleName;
            Email = email;
            Phone = phone;
            Password = password;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Photo = photo;
            Vkontakte = vkontakte;
            Telegram = telegram;
            City = city;
        }

    }
}
