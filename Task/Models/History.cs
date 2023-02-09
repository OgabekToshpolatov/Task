namespace Tasks.Models;

public class History
{
    public string? UserId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? Changed { get; set; }
    public DateTime DateTime { get; set; }
    public long ProductId { get; set; }
}