using Microsoft.Identity.Client;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;
using ProjektZTP.Repository.Repositories;
using static ProjektZTP.Features.OrderFeatures.Commands.AddOrder;

public class File
{
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public List<ProductAndAmount> Products { get; set; }
    public string Total { get; set; }
    public string Employee { get; set; }
}

public abstract class PdfBuilder
{
    protected File file;


    public File File
    {
        get { return file; }
    }


    public abstract void BuildCustomerName(string customer);
    public abstract void BuildAddress(string address);
    public abstract void BuildProducts(List<ProductAndAmount> products);
    public abstract void BuildTotal();
    public abstract void BuildEmployee(User user);
}



class InvoiceBuilder : PdfBuilder
{
    public InvoiceBuilder()
    {
        file = new File();

    }
    public async override void BuildCustomerName( string customer)
    {
        file.CustomerName = "Company name: " + customer;
    }

    public override void BuildAddress(string address)
    {
        file.Address = "Company address: " + address;
    }

    public override void BuildProducts(List<ProductAndAmount> products)
    {
        file.Products = products;
    }

    public override void BuildTotal()
    {
        float total = 0;
        float totaltax = 0;
        for (int i = 0; i < file.Products.Count; i++)
        {
            total += file.Products[i].Product.Price * file.Products[i].Amount;
            totaltax += (file.Products[i].Product.Vat / 100) * file.Products[i].Product.Price * file.Products[i].Amount;
        }
        float temp = total - totaltax;
        totaltax = (float)Math.Round(totaltax, 2);
        file.Total = "Total: " + temp.ToString() + " + " + totaltax.ToString() + "(taxes) = " + total.ToString();
    }
    public override void BuildEmployee(User user)
    {
        file.Employee = "Emplyee: " + user.FirstName +" "+ user.LastName;
    }

}

class ReceiptBuilder : PdfBuilder
{

    public ReceiptBuilder()
    {
        file = new File();

    }
    public async override void BuildCustomerName(string customer)
    {
        file.CustomerName = "Customer: " + customer;
    }

    public override void BuildAddress(string address)
    {
        file.Address = "------------------------------------------------";
    }

    public override void BuildProducts(List<ProductAndAmount> products)
    {
        file.Products = products;
    }

    public override void BuildTotal()
    {
        float total = 0;

        for (int i = 0; i < file.Products.Count; i++)
        {
            total += file.Products[i].Product.Price * file.Products[i].Amount;

        }
        file.Total = "Total: " + total.ToString();
    }
    public override void BuildEmployee(User user)
    {
        file.Employee = "Emplyee's Id: " + user.Id;
    }

}


public class PdfDirector
{
    public void BuildStandardFile(PdfBuilder pdfBuilder, string customer, string address, List<ProductAndAmount> products, User user)
    {
        pdfBuilder.BuildCustomerName(customer);
        pdfBuilder.BuildAddress(address);
        pdfBuilder.BuildProducts(products);
        pdfBuilder.BuildTotal();
        pdfBuilder.BuildEmployee(user);
    }
}