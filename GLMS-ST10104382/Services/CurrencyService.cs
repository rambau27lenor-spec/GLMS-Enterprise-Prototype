using System.Text.Json;

namespace GLMS_ST10104382.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://open.er-api.com/v6/latest/USD");

                if (!response.IsSuccessStatusCode)
                {
                    return 18.50m;
                }

                var json = await response.Content.ReadAsStringAsync();

                using var document = JsonDocument.Parse(json);

                var rate = document
                    .RootElement
                    .GetProperty("rates")
                    .GetProperty("ZAR")
                    .GetDecimal();

                return rate;
            }
            catch
            {
                return 18.50m;
            }
        }

        public decimal ConvertUsdToZar(decimal usdAmount, decimal rate)
        {
            return usdAmount * rate;
        }
    }
}
