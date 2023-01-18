using AutoMapper;
using FluentValidation;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;
using System.IO;
using System.Net.Mime;
using System.Reflection.PortableExecutable;
using static ProjektZTP.Mediator.Abstract;
using System.Reflection;
using ProjektZTP.Strategy;

namespace ProjektZTP.Features.OrderFeatures.Commands;

public class AddOrder
{
    private readonly IProductsRepository _repositoryProducts;
    private readonly IUserRepository _repositoryUsers;

    public AddOrder(IProductsRepository repositoryProducts, IUserRepository repositoryUsers = null)
    {
        _repositoryProducts = repositoryProducts;
        _repositoryUsers = repositoryUsers;
    }

    public class AddOrderValidator : AbstractValidator<AddOrderCommand>
    {
        public AddOrderValidator()
        {
            //there will be another validation for UserId or ProductId which doesnt exist in database.
            RuleFor(x => x.Customer).NotEmpty().NotNull();
            RuleFor(x => x.Address).NotEmpty().NotNull();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.Products).NotEmpty().NotNull();
        }
    }

    public record AddOrderCommand(
    string Customer,
    string Address,
    Guid UserId,
    ICollection<AddOrderCommandDto> Products) : IRequest<AddOrderCommandResult>;

    public class AddOrderCommandHandler : IHandler<AddOrderCommand, AddOrderCommandResult>
    {
        private readonly IUserRepository _repositoryuser;
        private readonly IProductsRepository _repositoryproduct;
        private readonly IOrdersRepository _repository;
        private readonly IMapper _mapper;
        private Logger _logger;




        public AddOrderCommandHandler(IProductsRepository productRepository, IUserRepository userRepository, IOrdersRepository repository, IMapper mapper)
        {
            _logger = Logger.GetInstance();
            _repositoryproduct = productRepository;
            _repositoryuser = userRepository;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AddOrderCommandResult> HandleAsync(AddOrderCommand request)
        {
            var validator = new AddOrderValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var entry = _mapper.Map<Order>(request);
            await _repository.Add(entry);
            //to change

            User user = await _repositoryuser.Get(request.UserId);


            List<ProductAndAmount> lista = new List<ProductAndAmount>();

            foreach (var productid in request.Products)
            {

                var prod = await _repositoryproduct.Get(productid.Id);

                ProductAndAmount temp = new ProductAndAmount(prod, productid.Amount);
                lista.Add(temp);

                prod.Amount = prod.Amount - productid.Amount;
                await _repositoryproduct.Update(prod);
            }

            PdfBuilder builderInvoice;

            builderInvoice = new InvoiceBuilder();

            PdfDirector pdfDirector = new PdfDirector();
            pdfDirector.BuildStandardFile(builderInvoice, request.Customer, request.Address, lista, user);
            _logger.Log("Invoice created.");
            SaveFile(builderInvoice.File, "Invoice");

            PdfBuilder builderReceipt;
            builderReceipt = new ReceiptBuilder();
            pdfDirector.BuildStandardFile(builderReceipt, request.Customer, request.Address, lista, user);
            _logger.Log("Receipt created.");
            SaveFile(builderReceipt.File, "Receipt");

            return new AddOrderCommandResult(entry.Id);
        }


        private static FileStreamResult SaveFile(File file, string name)
        {
            Document document = new Document();
            var stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;
            document.Open();

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
                table.AddCell(product.Product.Name);
                table.AddCell(product.Product.Price.ToString());
                table.AddCell(product.Product.Vat.ToString());
                table.AddCell(product.Amount.ToString());
            }

            document.Add(table);
            document.Add(new Paragraph(file.Total.ToString()));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(file.Employee));

            document.Close();

            var result = new FileStreamResult(stream, MediaTypeNames.Application.Pdf);
            result.FileDownloadName = "GeneratedFile.pdf";

            var context = new Context();

            if (name == "Invoice")
            {
                context.SetStrategy(new CloudStorage("DefaultEndpointsProtocol=https;AccountName=ztprichie;AccountKey=SA9HdPlCoRpeXGbDAnz4H+jTDtviEuPW2y6S5lLiz3WVa5qkUp5GQyb6risga3kxWsS9yzFAxfHy+AStloAPTg==;EndpointSuffix=core.windows.net", "pdfztp"));
            }
            else
            {
                context.SetStrategy(new LocalFileStorage());
            }

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            // Return the PDF document as a FileStreamResult
            context.ExecuteStrategy(stream, name);

           // fileStorageStrategy.SaveFile(stream, name);


            return result;
        }
    }

    public record AddOrderCommandResult(Guid id);

    public class AddOrderCommandDto
    {
        public Guid Id { get; set; }

        public int Amount { get; set; }
    }

    public class ProductAndAmount
    {
        public Product Product;
        public int Amount;

        public ProductAndAmount(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }
    }
}


