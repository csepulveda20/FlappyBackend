namespace Domain.Entities
{
    public class Score
    {
        public int Id { get; set; }
        public string Alias { get; set; } = string.Empty;
        public int Points { get; set; }
        public int? MaxCombo { get; set; }
        public int? DurationSec { get; set; }
        public string? Metadata { get; set; }
        public Guid? SessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Session Session { get; set; }

        private Score() { }

        public Score(string alias, int points, int? maxCombo = null, int? durationSec = null, string? metadata = null, Guid? sessionId = null)
        {
            Alias = alias ?? throw new ArgumentNullException(nameof(alias));
            Points = points;
            MaxCombo = maxCombo;
            DurationSec = durationSec;
            Metadata = metadata;
            SessionId = sessionId;
            CreatedAt = DateTime.UtcNow;
        }

        public static Score Create(string alias, int points, Guid? sessionId = null)
        {
            return new Score
            {
                Alias = alias,
                Points = points,
                SessionId = sessionId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}