using System.ComponentModel.DataAnnotations;

namespace ProjektZTP.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        [MinLength(1)]
        public string Customer { get; set; }
        [MinLength(1)]
        public string Address { get; set; }
        
        //relation to User
        public Guid UserId { get; set; }
        public User User { get; set; }

        //relation to Product
        public ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
