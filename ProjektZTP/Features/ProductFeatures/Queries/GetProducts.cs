using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;
using System.Net;
using static System.Net.WebRequestMethods;

namespace ProjektZTP.Features.ProductFeatures.Queries;

public class GetProducts
{
    public record GetProdcutsQuery() : IRequest<GetProductsResult>;

    public class Handler : IHandler<GetProdcutsQuery, GetProductsResult>
    {
        private readonly IProductsRepository _repository;

        public Handler(IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProductsResult> HandleAsync(GetProdcutsQuery request)
        {
            var tmpList = new List<ProductDTO>();
            var result = await _repository.GetProducts();

            string names = "";
            string value = "";
            string taxes = "";
            string amounts = "";
            foreach (var product in result)
            {
                var tmp = new ProductDTO(
                    product.Name,
                    product.Price,
                    product.Amount,
                    product.Vat);
                tmpList.Add(tmp);

                if(value == null)
                {
                    names = product.Name;
                    value = (product.Price * product.Amount).ToString();
                    taxes=product.Vat.ToString();
                    amounts = product.Amount.ToString();
                }
                else
                {
                    names = product.Name + "," + names;
                    value = (product.Price * product.Amount).ToString() + "," + value;
                    taxes = product.Vat.ToString() + "," + taxes;
                    amounts = product.Amount.ToString() + "," + amounts;
                }
            }

            string link = "https://quickchart.io/chart/render/sm-5e8581ce-0ed7-43ca-9658-13889fc65104?title= Magazine value&labels=" + names + "&data1=" + value;
            WebClient webClient = new WebClient();
            webClient.DownloadFile(link, "WartośćMagazynu.png");

            link= "https://quickchart.io/chart/render/sm-89cd9e8a-a2a9-4cf5-bdc0-3706831298b0?title= Products Taxes&labels=" + names + "&data1=" + taxes;
            webClient.DownloadFile(link, "WysokoscVat.png");

            link = "https://quickchart.io/chart/render/sm-66cffd1b-318e-42d3-9b53-5998c3660026?title= Price/Tax &labels=" + names + "&data1=" + value + "&data2=" + amounts;
            webClient.DownloadFile(link, "Cena&Ilosc.png");
            return new GetProductsResult(tmpList);
        }
    }

    public record GetProductsResult(
        List<ProductDTO> Products);

    public record ProductDTO(
        string Name,
        float Price,
        int Amount,
        float Vat);
}

