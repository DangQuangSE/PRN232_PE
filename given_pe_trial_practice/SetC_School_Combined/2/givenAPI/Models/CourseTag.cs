namespace givenAPI.Models;

public partial class CourseTag
{
    public int CourseId { get; set; }
    public int TagId { get; set; }

    public virtual Course Course { get; set; } = null!;
    public virtual Tag Tag { get; set; } = null!;
}
