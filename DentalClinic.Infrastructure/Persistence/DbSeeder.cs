using DentalClinic.Domain.Entities;
using DentalClinic.Domain.Enums;
using DentalClinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Doctors.AnyAsync())
            return; // فيه بيانات بالفعل، متعملش seed تاني

        var doctor1 = new Doctor("د. أحمد سامي", DoctorSpecialty.Orthodontics, "استشاري تقويم أسنان بخبرة 10 سنين");
        var doctor2 = new Doctor("د. منى الكمال", DoctorSpecialty.RestorativeCosmetic, "أخصائية تجميل وتركيبات الأسنان");

        context.Doctors.AddRange(doctor1, doctor2);
        await context.SaveChangesAsync();

        // جدول أسبوعي: الدكتور الأول شغال سبت واتنين وأربع
        var schedule1 = new List<DoctorWeeklyAvailability>
        {
            new(doctor1.Id, DayOfWeek.Saturday, new TimeOnly(10, 0), new TimeOnly(18, 0)),
            new(doctor1.Id, DayOfWeek.Monday, new TimeOnly(10, 0), new TimeOnly(18, 0)),
            new(doctor1.Id, DayOfWeek.Wednesday, new TimeOnly(10, 0), new TimeOnly(18, 0)),
        };

        // الدكتورة التانية شغالة حد وتلات وخميس
        var schedule2 = new List<DoctorWeeklyAvailability>
        {
            new(doctor2.Id, DayOfWeek.Sunday, new TimeOnly(12, 0), new TimeOnly(20, 0)),
            new(doctor2.Id, DayOfWeek.Tuesday, new TimeOnly(12, 0), new TimeOnly(20, 0)),
            new(doctor2.Id, DayOfWeek.Thursday, new TimeOnly(12, 0), new TimeOnly(20, 0)),
        };

        context.DoctorWeeklyAvailabilities.AddRange(schedule1);
        context.DoctorWeeklyAvailabilities.AddRange(schedule2);

        // صور تجريبية (روابط placeholder - غيريها لما تجهز صور حقيقية)
        var photos = new List<BeforeAfterPhoto>
        {
            new(doctor1.Id, "https://placehold.co/400x300?text=Before", "https://placehold.co/400x300?text=After", "تقويم أسنان - 8 شهور", 1),
            new(doctor2.Id, "https://placehold.co/400x300?text=Before", "https://placehold.co/400x300?text=After", "تركيبات أسنان أمامية", 1),
        };

        context.BeforeAfterPhotos.AddRange(photos);

        await context.SaveChangesAsync();
    }
}
