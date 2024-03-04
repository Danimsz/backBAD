using Newtonsoft.Json;

namespace BadServer.Servicios {

public class CoinGeckoApi : IDisposable
{
    private const string API_URL = "https://api.coingecko.com/api/v3/";

    private HttpClient HttpClient { get; init; }

    public CoinGeckoApi()
    {
        HttpClient = new HttpClient()
        {
            BaseAddress = new Uri(API_URL)
        };
    }

        public async Task<decimal> GetEthereumPriceAsync()
        {
            string json = await HttpClient.GetStringAsync("coins/ethereum");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            decimal price = (decimal)data.market_data.current_price.eur;
            return price;
        }

        public void Dispose()
    {
        HttpClient.Dispose();
    } 
}
}