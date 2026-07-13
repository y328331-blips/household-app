using System.Net.Http.Json;
using HouseholdApp.Client.Models;

namespace HouseholdApp.Client.Services;

public class TransactionApiClient(HttpClient http)
{
    public async Task<List<TransactionItem>> GetAllAsync()
    {
        var items = await http.GetFromJsonAsync<List<TransactionItem>>("api/transactions");
        return items ?? [];
    }

    public async Task AddAsync(TransactionItem item)
    {
        var response = await http.PostAsJsonAsync("api/transactions", item);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await http.DeleteAsync($"api/transactions/{id}");
        response.EnsureSuccessStatusCode();
    }
}
