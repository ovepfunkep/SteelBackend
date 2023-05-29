namespace DataBase.Entities
{
    public class User
    {
        public int Id { get; set; } = 0;
        public int RoleId { get; set; } = UserRoleId;
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; } = "";
        public string Password { get; set; } = "";
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Photo { get; set; }
        public string? Vkontakte { get; set; }
        public string? Telegram { get; set; }
        public string? City { get; set; }

        private static int UserRoleId = Convert.ToInt32(Methods.GetCustom("Select * from `Роли` where `Название` = 'Клиент'")![0][0]);

        public User(int id = 0, 
                    int roleId = 0,
                    string phone = "",
                    string password = "",
                    string? surname = null, 
                    string? name = null, 
                    string? middleName = null, 
                    string? email = null, 
                    string? gender = null, 
                    DateTime? dateOfBirth = null, 
                    string? photo = null, 
                    string? vkontakte = null, 
                    string? telegram = null, 
                    string? city = null)
        {
            Id = id;
            RoleId = roleId != 0 ? roleId : UserRoleId;
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
