namespace TaskManagement.Domain;

public class TaskItem
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public string? Priority { get; set; } // Nullable for feature flag
}