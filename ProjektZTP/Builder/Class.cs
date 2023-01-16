using ProjektZTP.Models;


public class Invoice
{
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public List<Product> Products { get; set; }
    public float Total { get; set; }
    public float TotalTax { get; set; }
    public float TotalNoTax { get; set; }
    public string WorkerName { get; set; }
    public string WorkerLastName { get; set; }
}

class InvoiceBuilder
{
    public Invoice invoice;

    public Invoice GetInvoice()
    {
        return invoice;
    }

    public void BuildCustomerName()
    {
        invoice.CustomerName = "John Doe";
    }

    public void BuildAddress()
    {
        invoice.Address = "123 Main St";
    }

    public void BuildProducts()
    {
        invoice.Products = new List<Product> {
            new Product { Name = "Product 1", Price = 10, Vat = 1, Amount=2 },
            new Product { Name = "Product 2", Price = 20, Vat = 2, Amount=3}
        };
    }

    public void BuildTotal()
    {
        float total = 0;
        for (int i = 0; i < invoice.Products.Count; i++)
        {
            total += invoice.Products[i].Price * invoice.Products[i].Amount;
        }
        invoice.Total = total;
    }
    public void BuildTotalTax()
    {
        float total = 0;
        for(int i= 0; i< invoice.Products.Count;i++)
        {
            total += (invoice.Products[i].Vat /100) * invoice.Products[i].Price * invoice.Products[i].Amount;
        }
        invoice.TotalTax = total;
    }
    public void BuildTotalNoTax()
    {
        invoice.TotalNoTax = invoice.Total - invoice.TotalTax;
    }

    public void BuildWorker()
    {
        invoice.WorkerName = "Jane";
        invoice.WorkerLastName = "Smith";
    }
}

class InvoiceDirector
{
    private InvoiceBuilder invoiceBuilder;

    public InvoiceDirector(InvoiceBuilder builder)
    {
        invoiceBuilder = builder;
    }

    public void BuildStandardInvoice()
    {
        invoiceBuilder.BuildCustomerName();
        invoiceBuilder.BuildAddress();
        invoiceBuilder.BuildProducts();
        invoiceBuilder.BuildTotal();
        invoiceBuilder.BuildTotalTax();
        invoiceBuilder.BuildTotalNoTax();
        invoiceBuilder.BuildWorker();
    }

    public void BuildRecipeInvoice()
    {
        invoiceBuilder.BuildWorker();
        invoiceBuilder.BuildProducts();
        invoiceBuilder.BuildTotal();
    }
}