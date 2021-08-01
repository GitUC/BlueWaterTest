using BlueWater.OrderManagement.Test.Hooks;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using BlueWater.OrderManagement.Common.Contracts;
using HttpClient = BlueWater.OrderManagement.Test.Hooks.HttpClient;
using Xunit;

namespace BlueWater.OrderManagement.Test.Steps
{
    [Binding]
    public class ProductSteps
    {
        private readonly HttpClient _httpClient;

        public ProductSteps(HttpClient httpClient) => _httpClient = httpClient;


        [Given(@"I have a new product name (.*) unit price (.*)")]
        public void GivenIHaveANewProductNameNotebookUnitPrice( string productName, double price)
        {
            HostingContext.ProductPayload = new ProductPayload
            {   
                ProductName = productName,
                UnitPrice = price
            };
        }
        
        [When(@"I create the product in Bluewater systme")]
        public async Task WhenICreateTheProductInBluewaterSystme()
        {
            var result = await _httpClient.Post($"api/v1/products", HostingContext.ProductPayload).ConfigureAwait(false);
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content;

                if (content != null)
                {
                    HostingContext.Product = await CommonSteps.ReadAsJsonAsync<ProductDto>(content);
                }
            }
        }

        [Then(@"The result should be (.*).")]
        [Then(@"And the result should be  (.*).")]
        public void ThenTheResultShouldBe(int statusCode)
        {
            Assert.Equal(statusCode, (int)_httpClient.Response.StatusCode);
        }

        [When(@"I check the created product")]
        [When(@"I check the updated product")]
        public async Task WhenCheckTheProduct()
        {
            var productId = HostingContext.Product.ProductId;

            var result = await _httpClient.Get($"api/v1/products/{productId}");
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content;

                Assert.NotNull(content);

                if (content != null)
                {
                    var newProduct = await CommonSteps.ReadAsJsonAsync<ProductDto>(content);

                    Assert.Equal(HostingContext.ProductPayload.ProductName, newProduct.ProductName);
                    Assert.Equal(HostingContext.ProductPayload.UnitPrice, newProduct.UnitPrice);
                }
            }
        }

        [When(@"I update the unit price to (.*)")]
        public async Task WhenIUpdateTheUnitPriceTo(double newPrice)
        {
            var productId = HostingContext.Product.ProductId;

            var result = await _httpClient.Put($"api/v1/products/{productId}?productUnitprice={newPrice}");
            if (result.IsSuccessStatusCode)
            {
                HostingContext.ProductPayload.UnitPrice = newPrice;
            }
        }

        [When(@"I delete the test product")]
        public async Task WhenIDeleteTheTestProduct()
        {
            var productId = HostingContext.Product.ProductId;

            await _httpClient.Delete($"api/v1/products/{productId}");
        }

    }
}
