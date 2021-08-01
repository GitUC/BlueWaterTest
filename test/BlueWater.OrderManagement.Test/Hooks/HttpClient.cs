using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlueWater.OrderManagement.Test.Hooks
{
    public class HttpClient
    {
        /// <summary>
        /// Gets http Response Message
        /// </summary>
        public HttpResponseMessage Response { get; set; }

        /// <summary>
        /// Get request
        /// </summary>
        /// <param name="request">request</param>
        public async Task<HttpResponseMessage> Get(string request)
        {
            var response = await HostingContext.Server.CreateRequest(request)
                .GetAsync().ConfigureAwait(false);

            Response = response;
            return response;
        }

        /// <summary>
        /// Post Request
        /// </summary>
        /// <typeparam name="T">Type of Payload</typeparam>
        /// <param name="request">request</param>
        /// <param name="payload">payload</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> Post<T>(string request, T payload)
        {
            var response = await HostingContext.Server.CreateRequest(request)
                .And(message => message.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"))
                .SendAsync("POST").ConfigureAwait(false);

            Response = response;
            return response;
        }

        /// <summary>
        /// Put request
        /// </summary>
        /// <typeparam name="T">Type of Payload</typeparam>
        /// <param name="request">request</param>
        /// <param name="payload">payload</param>
        public async Task<HttpResponseMessage> Put(string request)
        {
            var response = await HostingContext.Server.CreateRequest(request)
                .SendAsync("PUT")
                .ConfigureAwait(false);

            Response = response;
            return response;
        }


        /// <summary>
        /// Put request
        /// </summary>
        /// <typeparam name="T">Type of Payload</typeparam>
        /// <param name="request">request</param>
        /// <param name="payload">payload</param>
        public async Task<HttpResponseMessage> Put<T>(string request, object payload)
        {
            var response = await HostingContext.Server.CreateRequest(request)
                .And(message => message.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/text"))
                .SendAsync("PUT")
                .ConfigureAwait(false);

            Response = response;
            return response;
        }


        /// <summary>
        /// Delete request
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> Delete(string request)
        {
            var response = await HostingContext.Server.CreateRequest(request)
                .SendAsync("DELETE")
                .ConfigureAwait(false);

            Response = response;
            return response;
        }
    }
}
