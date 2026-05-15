namespace GameLeaderboard.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public ICollection<ScoreRecord> Scores { get; set; } = new List<ScoreRecord>();
}