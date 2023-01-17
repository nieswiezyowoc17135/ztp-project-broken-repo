using ProjektZTP.Models;


public class File
{
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public List<Product> Products { get; set; }
    public string Total { get; set; }
    public string Worker { get; set; }
}

public abstract class PdfBuilder
{
    protected File file;


    public File File
    {
        get { return file; }
    }


    public abstract void BuildCustomerName();
    public abstract void BuildAddress();
    public abstract void BuildProducts();
    public abstract void BuildTotal();
    public abstract void BuildWorker();
}



class InvoiceBuilder : PdfBuilder
{
    public InvoiceBuilder()
    {
        file = new File();
    }
    public override void BuildCustomerName()
    {
        file.CustomerName = "Company name: " + "Order.Customer";
    }

    public override void BuildAddress()
    {
        file.Address = "Company address:: " + "***adres firmy***";
    }

    public override void BuildProducts()
    {
        file.Products = new List<Product> {
            new Product { Name = "Product 1", Price = 10, Vat = 1, Amount=2 },
            new Product { Name = "Product 2", Price = 20, Vat = 2, Amount=3}
        };
    }

    public override void BuildTotal()
    {
        float total = 0;
        float totaltax = 0;
        for (int i = 0; i < file.Products.Count; i++)
        {
            total += file.Products[i].Price * file.Products[i].Amount;
            totaltax += (file.Products[i].Vat / 100) * file.Products[i].Price * file.Products[i].Amount;
        }
        float temp = total - totaltax;
        totaltax = (float)Math.Round(totaltax, 2);
        file.Total = "Total: " + temp.ToString() + " + " + totaltax.ToString() + "(taxes) = " + total.ToString();
    }
    public override void BuildWorker()
    {
        file.Worker = "Emplyee: " + "User.firstname" + "User.lastname";
    }

}

class ReceiptBuilder : PdfBuilder
{
    public ReceiptBuilder()
    {
        file = new File();
    }
    public override void BuildCustomerName()
    {
        file.CustomerName = "Customer: " + "Order.Customer";
    }

    public override void BuildAddress()
    {
        file.Address = "--------------------------------------";
    }

    public override void BuildProducts()
    {
        file.Products = new List<Product> {
            new Product { Name = "Product 1", Price = 10, Vat = 1, Amount=2 },
            new Product { Name = "Product 2", Price = 20, Vat = 2, Amount=3}
        };
    }

    public override void BuildTotal()
    {
        float total = 0;

        for (int i = 0; i < file.Products.Count; i++)
        {
            total += file.Products[i].Price * file.Products[i].Amount;

        }

        file.Total = "Total after taxes: " + total.ToString();
    }
    public override void BuildWorker()
    {
        file.Worker = "Emplyee: " + "User.id";
    }

}


public class PdfDirector
{
    public void BuildStandardFile(PdfBuilder pdfBuilder)
    {
        pdfBuilder.BuildCustomerName();
        pdfBuilder.BuildAddress();
        pdfBuilder.BuildProducts();
        pdfBuilder.BuildTotal();
        pdfBuilder.BuildWorker();
    }

}