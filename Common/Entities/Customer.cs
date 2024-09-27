using Microsoft.AspNetCore.Identity;

namespace Common.Entities;

public sealed class Customer:IdentityUser
{
    public required string Name { get; set; }

    public string? Address { get; set; }
    public DateTime DateCreated { get; set; }

    public ICollection<Notify> Notifies { get; }
}