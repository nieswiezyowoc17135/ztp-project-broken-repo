using Microsoft.AspNetCore.Mvc;
using ProjektZTP.Models;
using static BasicInvoiceBuilder;
using ProjektZTP.Features.OrderFeatures.Queries;
using Microsoft.EntityFrameworkCore;
using ProjektZTP.Data;

[Route("api/[controller]")]
public class InvoiceController : Controller
{
    [HttpGet]
    public FileStreamResult GetInvoice()
    {
        // Retrieve the user, order, and products from the database
        var user = GetUser();
        var order = GetOrder();
        var products = GetProducts();

        InvoiceBuilder invoiceBuilder;

        // Check the total amount of the order
        var total = products.Sum(p => p.Price * p.Amount * (1 + p.Vat));
        if (total > 100)
        {
            invoiceBuilder = new PremiumInvoiceBuilder();
        }
        else
        {
            invoiceBuilder = new BasicInvoiceBuilder();
        }

        // Use the builder to create the invoice
        invoiceBuilder.WithWorker(user)
                      .WithOrder(order)
                      .WithProducts(products);

        return invoiceBuilder.Build();
        
    }

    private User GetUser()
    {

        User user = new User
        {
            FirstName = "Ryszard",
            LastName = "Rutkowski",
            Email = "mail@gmail.com"


        };
        return user;

    }

    private Order GetOrder()
    {
        Order order = new Order
        {
            Customer = "Januszex",
            Address = "Adres 123",
        };
        return order;
    }

    private List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        {
            Product product = new Product
            {
                Name = "produkt1",
                Price = 200,
                Vat = 13
            };
            products.Add(product);

        };
        return products;
    }
}
