using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Photo { get; set; }
        public string? Vkontakte { get; set; }
        public string? Telegram { get; set; }
        public string? City { get; set; }

        public User(int id, int roleId, string phoneNumber, string password, string? surname = null, string? name = null, string? middleName = null, string? email = null, string? gender = null, DateTime? dateOfBirth = null, string? photo = null, string? vkontakte = null, string? telegram = null, string? city = null)
        {
            Id = id;
            RoleId = roleId;
            Surname = surname;
            Name = name;
            MiddleName = middleName;
            Email = email;
            PhoneNumber = phoneNumber;
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
