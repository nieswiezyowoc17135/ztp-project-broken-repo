using System.ComponentModel.DataAnnotations;

namespace ProjektZTP.Models
{
    public class Product
    {
        public Product()
        {
        }

        public Product(
            string name,
            float price,
            int amount,
            float vat) : this()
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Amount = amount;
            Vat = vat;
        }

        public Guid Id { get; set; }
        [MinLength(1)]
        public string Name { get; set; }
        [Range(0, 10000.99)]
        public float Price { get; set; }
        [Range(0,100)]
        public int Amount { get; set; }
        [Range(0, 60)]
        public float Vat { get; set; }

        //relation to order
        public ICollection<ProductOrder> ProductOrder { get; set; }
    }
}
