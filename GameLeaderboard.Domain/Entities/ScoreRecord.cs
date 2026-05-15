namespace GameLeaderboard.Domain.Entities;

public class ScoreRecord : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public int Points { get; set; }
    public int Level { get; set; }
    public TimeSpan PlayTime { get; set; }
}