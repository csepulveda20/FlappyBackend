namespace Domain.Entities
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public string? Alias { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public string? Metadata { get; set; }

        private Session() { }

        public static Session Create(string alias, DateTime startedAt, DateTime endedAt, string? metadata = null)
        {
            return new Session
            {
                SessionId = Guid.NewGuid(),
                Alias = alias,
                StartedAt = startedAt,
                EndedAt = endedAt,
                Metadata = metadata
            };
        }
    }
}