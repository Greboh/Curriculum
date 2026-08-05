namespace Curriculum.Core.Entities;

public sealed class Education
{
    public required Guid Id { get; set; }
    public required string Institution { get; set; }
    public required string Degree { get; set; }
    public required DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}