namespace OptimisticConcurrencyExample.Models;

public class Meal
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte[] ConcurrencyToken { get; set; }
}