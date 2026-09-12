using DentalClinic.Domain.Common;
using DentalClinic.Domain.Enums;

namespace DentalClinic.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }

    private User() { }

    public User(string name, string email, string passwordHash, UserRole role)
    {
        Name = name; Email = email; PasswordHash = passwordHash; Role = role;
    }
}