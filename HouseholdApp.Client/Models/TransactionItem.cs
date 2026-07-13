namespace HouseholdApp.Client.Models;

public class TransactionItem
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Category { get; set; } = string.Empty;
    public int Amount { get; set; }
    public TransactionType Type { get; set; } = TransactionType.Expense;
}
