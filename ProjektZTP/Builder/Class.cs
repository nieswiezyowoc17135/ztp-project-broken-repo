using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using ProjektZTP.Models;
using System.Net.Mime;

public abstract class InvoiceBuilder
{
    protected User _user;
    protected Order _order;
    protected List<Product> _products;

    public InvoiceBuilder WithWorker(User user)
    {
        _user = user;
        return this;
    }

    public InvoiceBuilder WithOrder(Order order)
    {
        _order = order;
        return this;
    }

    public InvoiceBuilder WithProducts(List<Product> products)
    {
        _products = products;
        return this;
    }

    public abstract FileStreamResult Build();
}

public class BasicInvoiceBuilder : InvoiceBuilder
{
    public override FileStreamResult Build()
    {
        // Create a new PDF document
        var document = new Document();
        var stream = new MemoryStream();

        PdfWriter pdf = PdfWriter.GetInstance(document, stream);
        pdf.CloseStream = false;
        // Open the document for writing
        document.Open();

        // Add invoice details
        var table = new PdfPTable(4);
        table.WidthPercentage = 100;
        table.AddCell("Product Name");
        table.AddCell("Price");
        table.AddCell("Amount");
        table.AddCell("Vat");

        // Add products to the table

        foreach (var product in _products.ToList())
        {
            table.AddCell(product.Name);
            table.AddCell(product.Price.ToString());
            table.AddCell(product.Amount.ToString());
            table.AddCell(product.Vat.ToString());
        }

        // Add customer details
        var customerDetails = new Paragraph();
        customerDetails.Add(new Chunk("Customer: " + _order.Customer + "\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));
        customerDetails.Add(new Chunk("Address: " + _order.Address + "\n", FontFactory.GetFont(FontFactory.HELVETICA)));

        // Add user details
        var userDetails = new Paragraph();
        userDetails.Add(new Chunk("User: " + _user.FirstName + " " + _user.LastName + "\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));
        userDetails.Add(new Chunk("Email: " + _user.Email + "\n", FontFactory.GetFont(FontFactory.HELVETICA)));

        // Add the table and customer/user details to the document

        document.Add(customerDetails);
        document.Add(table);
        document.Add(userDetails);

        // Close the document
        document.Close();
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        // Return the PDF document as a FileStreamResult
        var result = new FileStreamResult(stream, MediaTypeNames.Application.Pdf);
        result.FileDownloadName = "JEfTest.pdf";
        return result;
    }




    public class PremiumInvoiceBuilder : InvoiceBuilder
    {
        public override FileStreamResult Build()
        {
            // Create a new PDF document
            var document = new Document();
            var stream = new MemoryStream();
            PdfWriter.GetInstance(document, stream);

            // Open the document for writing
            document.Open();

            // Add invoice details
            var table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.AddCell("Product Name");
            table.AddCell("Price");
            table.AddCell("Amount");
            table.AddCell("Vat");
            table.AddCell("Total");

            // Add products to the table
            float total = 0;
            foreach (var product in _products.ToList())
            {
                table.AddCell(product.Name);
                table.AddCell(product.Price.ToString());
                table.AddCell(product.Amount.ToString());
                table.AddCell(product.Vat.ToString());
                float productTotal = product.Price * product.Amount * (1 + product.Vat);
                total += productTotal;
                table.AddCell(productTotal.ToString());
            }

            // Add customer details
            var customerDetails = new Paragraph();
            customerDetails.Add(new Chunk("Customer: " + _order.Customer + "\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));
            customerDetails.Add(new Chunk("Address: " + _order.Address + "\n", FontFactory.GetFont(FontFactory.HELVETICA)));

            // Add user details
            var userDetails = new Paragraph();
            userDetails.Add(new Chunk("Worker: " + _user.FirstName + " " + _user.LastName + "\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));
            userDetails.Add(new Chunk("Email: " + _user.Email + "\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));

            // Add total
            var totalParagraph = new Paragraph();
            totalParagraph.Add(new Chunk("Total: " + total.ToString() + "\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));

            // Add the table and customer/user details to the document
            document.Add(table);
            document.Add(customerDetails);
            document.Add(userDetails);
            document.Add(totalParagraph);




            // Close the document
            document.Close();

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }
            stream.Position = 0;

            // Return the PDF document as a FileStreamResult
            return new FileStreamResult(stream, "application/pdf");
        }
    }

}
