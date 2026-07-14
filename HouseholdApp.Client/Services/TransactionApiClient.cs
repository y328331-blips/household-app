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

    public async Task<TransactionItem?> GetAsync(int id)
    {
        var response = await http.GetAsync($"api/transactions/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TransactionItem>();
    }

    public async Task AddAsync(TransactionItem item)
    {
        var response = await http.PostAsJsonAsync("api/transactions", item);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(int id, TransactionItem item)
    {
        var response = await http.PutAsJsonAsync($"api/transactions/{id}", item);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await http.DeleteAsync($"api/transactions/{id}");
        response.EnsureSuccessStatusCode();
    }
}
