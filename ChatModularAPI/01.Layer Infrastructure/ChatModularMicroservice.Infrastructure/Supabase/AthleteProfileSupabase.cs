using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain;

[Table("athlete_profiles")]
public class AthleteProfileSupabase : BaseModel
{
    [PrimaryKey("id", false)]
    public string Id { get; set; } = string.Empty;

    [Column("user_id")]
    public string UserId { get; set; } = string.Empty;

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("birth_date")]
    public DateTime? BirthDate { get; set; }

    [Column("height")]
    public decimal? Height { get; set; }

    [Column("weight")]
    public decimal? Weight { get; set; }

    [Column("experience_level")]
    public string ExperienceLevel { get; set; } = string.Empty;

    [Column("goal")]
    public string Goal { get; set; } = string.Empty;

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("emergency_contact")]
    public string? EmergencyContact { get; set; }

    [Column("medical_conditions")]
    public string? MedicalConditions { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}