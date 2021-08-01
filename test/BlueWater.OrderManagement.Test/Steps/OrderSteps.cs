using BlueWater.OrderManagement.Common.Contracts;
using BlueWater.OrderManagement.Test.Hooks;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;
using HttpClient = BlueWater.OrderManagement.Test.Hooks.HttpClient;

namespace BlueWater.OrderManagement.Test.Steps
{
    [Binding]
    public class OrderSteps
    {

        private readonly HttpClient _httpClient;

        public OrderSteps(HttpClient httpClient) => _httpClient = httpClient;

        [Given(@"Payload as JSON")]
        public void GivenPayloadAsJSON(string request)
        {
            HostingContext.OrderPayload = JsonConvert.DeserializeObject<Orders>(request);
        }


        [Given(@"Schedule datetime Payload as JSON")]
        public void GivenScheduleDatetimePayloadAsJSON(string request)
        {
            HostingContext.ScheduleDateTime = JsonConvert.DeserializeObject<ScheduleDateTime>(request);
        }



        [When(@"the Admin process the order")]
        public async Task WhenTheAdminProcessTheOrder()
        {
            var result = await _httpClient.Post($"api/v1/orders/createOrder", HostingContext.OrderPayload);
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content;

                if (content != null)
                {
                    HostingContext.JobId = await content.ReadAsStringAsync();
                }
            }
        }

        [When(@"the Admin process the schedule Job")]
        public async Task WhenTheAdminProcessTheScheduleJob()
        {
            var result = await _httpClient.Post($"api/v1/orders/scheduleJob", HostingContext.ScheduleDateTime);
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content;

                if (content != null)
                {
                    HostingContext.JobId = await content.ReadAsStringAsync();
                }
            }
        }



        [When(@"I get job status as (.*)")]
        public async Task WhenIGetJobStatus(string expectedStatus)
        {
            var result = await _httpClient.Get($"api/v1/orders/getOrderStatus/{HostingContext.JobId}");

            Assert.True(result.IsSuccessStatusCode);

            var content = result.Content;
            Assert.NotNull(content);

            var status = await content.ReadAsStringAsync();
            Assert.Equal(expectedStatus, status);
        }

        [Given(@"Sleep (.*) seconds")]
        public void GivenSleepSeconds(int secs)
        {
            System.Threading.Thread.Sleep(secs * 1000);
        }

    }
}
