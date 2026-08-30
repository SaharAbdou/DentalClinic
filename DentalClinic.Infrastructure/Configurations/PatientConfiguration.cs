using DentalClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinic.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.HasIndex(p => p.PhoneNumber).IsUnique();
    }
}