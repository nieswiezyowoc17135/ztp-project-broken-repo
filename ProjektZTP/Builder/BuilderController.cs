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
using Org.BouncyCastle.Utilities.Net;
using System.Reflection.PortableExecutable;

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
        PdfBuilder builder;
        PdfDirector director = new PdfDirector();

        if (type == "invoice")
        {
            _logger.Log("Invoice created.");
            builder = new InvoiceBuilder();
            director.BuildStandardFile(builder);
            return SaveFile(builder.File);
        }
        else
        {
            _logger.Log("Receipt created.");
            builder = new ReceiptBuilder();
            director.BuildStandardFile(builder);
            return SaveFile(builder.File);
        }

    }

    private static FileStreamResult SaveFile(File file)
    {
        Document document = new Document();
        var stream = new MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(document, stream);
        writer.CloseStream = false;
        document.Open();

        // Add the invoice details to the PDF
        document.Add(new Paragraph(file.CustomerName));
        document.Add(new Paragraph(file.Address));

        document.Add(new Paragraph(" "));

        PdfPTable table = new PdfPTable(4);
        table.AddCell("Product");
        table.AddCell("Cost");
        table.AddCell("Tax");
        table.AddCell("Amount");

        foreach (var product in file.Products)
        {
            table.AddCell(product.Name);
            table.AddCell(product.Price.ToString());
            table.AddCell(product.Vat.ToString());
            table.AddCell(product.Amount.ToString());
        }

        document.Add(table);
        document.Add(new Paragraph(file.Total.ToString()));
        document.Add(new Paragraph(" "));
        document.Add(new Paragraph(file.Worker));

        document.Close();


        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        // Return the PDF document as a FileStreamResult
        var result = new FileStreamResult(stream, MediaTypeNames.Application.Pdf);
        result.FileDownloadName = "GeneratedFile.pdf";
        return result;
    }



}
