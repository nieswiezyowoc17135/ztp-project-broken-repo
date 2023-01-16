using Microsoft.AspNetCore.Mvc;
using ProjektZTP.Models;
using ProjektZTP.Features.OrderFeatures.Queries;
using Microsoft.EntityFrameworkCore;
using ProjektZTP.Data;
using System.ComponentModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;
using System.Net.Http.Headers;
using System.IO;
using System.Net.Mime;

[Route("api/[controller]")]
public class InvoiceController : Controller
{
    private Logger _logger;

    public InvoiceController()
    {
        _logger = Logger.GetInstance();
    }


    [HttpPost]
    public FileStreamResult CreateInvoice(string type)
    {
        InvoiceBuilder builder = new InvoiceBuilder();
        builder.invoice = new Invoice();
        InvoiceDirector director = new InvoiceDirector(builder);

        if (type=="invoice")
        {
            _logger.Log("Invoice created.");
            director.BuildStandardInvoice();
        }
        else if(type=="recipe")
        {
            _logger.Log("Recipe created.");
            director.BuildRecipeInvoice();
        }

        Invoice invoice = builder.GetInvoice();

            
            Document document = new Document();
            var stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
             writer.CloseStream = false;
             document.Open();

            // Add the invoice details to the PDF
            if(invoice.CustomerName!=null)
            document.Add(new Paragraph("Invoice for " + invoice.CustomerName));
            if (invoice.Address != null)
            document.Add(new Paragraph("Address: " + invoice.Address));
            if (invoice.WorkerName != null)
            document.Add(new Paragraph("Worker: " + invoice.WorkerName + " " + invoice.WorkerLastName));
            document.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(4);
            table.AddCell("Product");
            table.AddCell("Cost");
            table.AddCell("Tax");
            table.AddCell("Amount");

        foreach (var product in invoice.Products)
            {
                table.AddCell(product.Name);
                table.AddCell(product.Price.ToString());
            table.AddCell(product.Vat.ToString());
            table.AddCell(product.Amount.ToString());
        }

        document.Add(table);
        document.Add(new Paragraph("Total: " + invoice.Total.ToString()));
        if(invoice.TotalTax!=0)
        document.Add(new Paragraph("Total Taxes: " + invoice.TotalTax.ToString()));
        if(invoice.TotalNoTax!=0)
        document.Add(new Paragraph("Total without Taxes: " + invoice.TotalNoTax.ToString()));

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
}

