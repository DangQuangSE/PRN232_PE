namespace givenAPI.Models;

public partial class CourseAssistant
{
    public int CourseId { get; set; }
    public int AssistantId { get; set; }

    public virtual Course Course { get; set; } = null!;
    public virtual Assistant Assistant { get; set; } = null!;
}
