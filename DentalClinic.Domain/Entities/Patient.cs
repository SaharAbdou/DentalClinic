using DentalClinic.Domain.Common;

namespace DentalClinic.Domain.Entities;

public class Patient : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;

    public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();

    private Patient() { } // EF Core

    public Patient(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}