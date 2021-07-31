using System;
using BlueWater.OrderManagement.Test.Hooks;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using BlueWater.OrderManagement.Common.Contracts;
using System.Net.Http;
using HttpClient = BlueWater.OrderManagement.Test.Hooks.HttpClient;
using Newtonsoft.Json;
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
                    HostingContext.Product = await ReadAsJsonAsync<ProductDto>(content);
                }
            }
        }

        [Then(@"the result should be (.*)")]
        [Then(@"And the result should be  (.*).")]
        public void ThenTheResultShouldBe(int statusCode)
        {
            Assert.Equal(statusCode, (int)_httpClient.Response.StatusCode);
        }

        [When(@"I check the created product")]
        public async Task WhenCheckTheCreatedProduct()
        {
            var productId = HostingContext.Product.ProductId;

            var result = await _httpClient.Get($"api/v1/products/{productId}");
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content;

                Assert.NotNull(content);

                if (content != null)
                {
                    var newProduct = await ReadAsJsonAsync<ProductDto>(content);

                    Assert.Equal(HostingContext.ProductPayload.ProductName, newProduct.ProductName);
                    Assert.Equal(HostingContext.ProductPayload.UnitPrice, newProduct.UnitPrice);
                }
            }
        }

        /// <summary>
        /// helper method to read HttpContent to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        async Task<T> ReadAsJsonAsync<T>(HttpContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
