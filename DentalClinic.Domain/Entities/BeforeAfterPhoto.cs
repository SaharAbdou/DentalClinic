using DentalClinic.Domain.Common;

namespace DentalClinic.Domain.Entities;

public class BeforeAfterPhoto : BaseEntity
{
    public Guid DoctorId { get; private set; }
    public Doctor Doctor { get; private set; } = null!;

    public string BeforeImageUrl { get; private set; } = null!;
    public string AfterImageUrl { get; private set; } = null!;
    public string? Description { get; private set; }
    public int DisplayOrder { get; private set; }

    private BeforeAfterPhoto() { } // EF Core

    public BeforeAfterPhoto(Guid doctorId, string beforeImageUrl, string afterImageUrl, string? description, int displayOrder)
    {
        DoctorId = doctorId;
        BeforeImageUrl = beforeImageUrl;
        AfterImageUrl = afterImageUrl;
        Description = description;
        DisplayOrder = displayOrder;
    }
}