using System.Net.Http.Json;
using GLMS_ST10104382.Models;

namespace GLMS_ST10104382.Services
{
    public class ContractApiService
    {
        private readonly HttpClient _httpClient;

        public ContractApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Contract>> GetContractsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Contract>>

            ("https://localhost:7051/api/contracts");
        }
    }
}
