using System.ComponentModel.DataAnnotations;

namespace ProjektZTP.Models
{
    public class User
    {
        public User()
        {

        }

        public User(
            string login,
            string password,
            string email,
            string firstName,
            string lastName) : this()
        {
            Id = Guid.NewGuid();
            Login = login;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }


        public Guid Id { get; set; }
        [MinLength(1)]
        public string Login { get; set; }
        [MinLength(1)]
        public string Password { get; set; }
        [MinLength(1)]
        public string Email { get; set; }
        [MinLength(1)]
        public string FirstName { get; set; }
        [MinLength(1)]
        public string LastName { get; set; }

        //order relation
        public ICollection<Order> Orders { get; set; }
    }
}
