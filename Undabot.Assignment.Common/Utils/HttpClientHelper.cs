using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Undabot.Assignment.Common.Utils
{
    public class HttpClientHelper
    {

        public async Task<string> GetJsonFromApi(string url)
        {
            string resultJson = "";
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage result = null;

                result = await httpClient.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    var respoMessage = result.Content.ReadAsStringAsync();
                    resultJson = respoMessage.Result;
                }
                else
                {
                    var respoMessage = result.Content.ReadAsStringAsync();
                }
            }

            return resultJson;
        }
    }
}
