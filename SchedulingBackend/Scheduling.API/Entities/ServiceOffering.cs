namespace Scheduling.API.Entities;

public class ServiceOffering
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public TimeSpan Duration { get; private set; }
    public bool IsActive { get; private set; }

    private ServiceOffering() { Name = string.Empty; }

    public ServiceOffering(string name, decimal price, TimeSpan duration, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");

        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be greater than zero.");

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
        Duration = duration;
        IsActive = true;
    }

    public void Update(string name, decimal price, TimeSpan duration, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");

        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be greater than zero.");

        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Price = price;
        Duration = duration;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
