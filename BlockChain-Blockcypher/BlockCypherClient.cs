using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BlockChainBlockcypher.ConsoleWorkers;
using BlockChainBlockcypher.Models;
using BlockChainBlockcypher.Storage;
using Newtonsoft.Json;

namespace BlockChainBlockcypher
{
    public class BlockcypherClient
    {
        private readonly JsonParser _jsonParser = new JsonParser();

        private readonly string _mainUrl = "https://api.blockcypher.com/v1/beth/test";
        private readonly string _token = null; 


        public async Task<FaucetRezult> FaucetsAsync(string address, long amount)
        {
            var url = _mainUrl + $"/faucet?token={_token}";

            return await PostAsync<FaucetRezult>(url, new { address = address, amount = amount });
        }


        public async Task<TranscationRezult> SendRawTransactionAsync(string transactionHex)
        {
            var url = _mainUrl + $"/txs/push?token={_token}";

            return await PostAsync<TranscationRezult>(url, new { tx = transactionHex });
        }

        public async Task<BalanceResult> GetBalanceAsync(string address)
        {
            var url = _mainUrl + $"/addrs/{address}/balance";

            return await GetAsync<BalanceResult>(url);
        }


        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }


        private async Task<T> GetAsync<T>(string url) where T : new()
        {
            using (HttpClient client = GetClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    string content = await response.Content.ReadAsStringAsync();

                    return _jsonParser.Deserialize<T>(content);
                }
                catch (Exception ex)
                {
                    MessageHandler.SendMessage(ex.Message);
                    return await Task.FromResult<T>(new T());
                }
            }

        }

        private async Task<T> PostAsync<T>(string url, object obj) where T : new()
        {
            using (HttpClient client = GetClient())
            {
                try
                {
                    string requestJson = JsonConvert.SerializeObject(obj ?? new object());

                    HttpResponseMessage response = await client.PostAsync(url, new StringContent(requestJson, Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();

                    return _jsonParser.Deserialize<T>(content);
                }
                catch (Exception ex)
                {
                    MessageHandler.SendMessage(ex.Message);

                    return await Task.FromResult<T>(new T());
                }
            }
        }

    }

}
