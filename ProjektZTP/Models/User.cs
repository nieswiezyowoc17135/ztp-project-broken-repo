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
            Orders = new List<Order>();
        }

        //for get methods
        public User(
            Guid id,
            string login,
            string password,
            string email,
            string firstName,
            string lastName,
            ICollection<Order> orders) : this()
        {
            Id = id;
            Login = login;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Orders = orders;
        }


        public Guid Id { get; set; }

        [MinLength(1)]
        [MaxLength(15)]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Login { get; set; } = null!;

        [MinLength(1)]
        [MaxLength(15)]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Password { get; set; } = null!;

        [MinLength(1)]
        [MaxLength(20)]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; } = null!;

        [MinLength(1)]
        [MaxLength(15)]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirstName { get; set; } = null!;

        [MinLength(1)]
        [MaxLength(15)]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LastName { get; set; } = null!;

        //order relation
        public ICollection<Order> Orders { get; set; }
    }
}
