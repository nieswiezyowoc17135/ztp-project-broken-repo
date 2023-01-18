using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using ProjektZTP.Data;
using ProjektZTP.Models;
using static ProjektZTP.Adapter.AdapterPattern.OrderExport;

namespace ProjektZTP.Adapter
{
    public class AdapterPattern
    {
        public interface IDataExporter
        {
            Task<OrderExport> ExportData();
        }

        public class DatabaseExporter : IDataExporter
        {
            private readonly DatabaseContext _context;

            public DatabaseExporter(DatabaseContext context)
            {
                _context = context;
            }

            public async Task<OrderExport> ExportData()
            {
                var data = await _context.Orders.Include(x => x.ProductOrder).Select(x => new OrderExport.OrderDto
                {
                    Id = x.Id,
                    Customer = x.Customer,
                    Address = x.Address,
                    Products = x.ProductOrder.Select(z => new OrderExport.ProductDto
                    {
                        Id = z.Product.Id,
                        Name = z.Product.Name,
                        Price = z.Product.Price,
                        Vat = z.Product.Vat
                    }).ToList()
                }).ToListAsync();

                return new OrderExport()
                {
                    Orders = data
                };
            }
        }

        public class ExcelExporter
        {
            public async Task<byte[]> GenerateExcelFile(IDataExporter exporter)
            {
                var orders = await exporter.ExportData();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Orders");

                    worksheet.Cells[1, 1].Value = "Order Id";
                    worksheet.Cells[1, 2].Value = "Customer";
                    worksheet.Cells[1, 3].Value = "Address";
                    worksheet.Cells[1, 4].Value = "Products";


                    for (int i = 0; i < orders.Orders.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = orders.Orders[i].Id;
                        worksheet.Cells[i + 2, 2].Value = orders.Orders[i].Customer;
                        worksheet.Cells[i + 2, 3].Value = orders.Orders[i].Address;
                        var products = orders.Orders[i].Products;
                        var textField = "";
                        for(int j = 0; j < products.Count; j++)
                        {
                                textField += $"Record numer:{j} Id: " + Convert.ToString(products[j].Id)
                                + " Name: " + products[j].Name
                                + " Price: " + Convert.ToString(products[j].Price)
                                + " Vat: " + Convert.ToString(products[j].Vat) + "  |  ";

                        }
                        worksheet.Cells[i + 2, 4].Value = textField;

                    }

                    return package.GetAsByteArray();
                }
            }
        }

        public class JsonExporter
        {
            public async Task<byte[]> GenerateJsonFile(IDataExporter exporter)
            {
                var orders = await exporter.ExportData();
                var json = JsonConvert.SerializeObject(orders);
                return System.Text.Encoding.UTF8.GetBytes(json);
            }
        }

        public class OrderExport
        {
            public List<OrderDto> Orders { get; set; }

            public class OrderDto
            {
                public Guid Id { get; set; }
                public string Customer { get; set; }

                public string Address { get; set; }

                public List<ProductDto> Products { get; set; }

            }

            public class ProductDto
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public float Price { get; set; }
                public float Vat { get; set; }
            }
        }
    }
}
