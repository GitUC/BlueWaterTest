using BlueWater.OrderManagement.Test.Hooks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace BlueWater.OrderManagement.Test.Steps
{
    public static class CommonSteps
    {
        /// helper method to read HttpContent to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        internal static async Task<T> ReadAsJsonAsync<T>(HttpContent content)
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
