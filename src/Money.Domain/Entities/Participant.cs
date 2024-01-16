using Money.Domain.Requests;

namespace Money.Domain.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string CPF { get; set; } 
    public decimal Balance { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public Participant()
    {
        Id = Guid.NewGuid();
        Balance = 0;
    }

    public void IncreaseBalance(decimal amount)
    {
        Balance += amount;
    }

    public bool TryDecreaseBalance(decimal amount)
    {
        if (Balance < amount)
            return false;

        Balance -= amount;

        return true;
    }
}
