namespace Application.UseCase.Dtos
{
    public class ScoreDto
    {
        public string Alias { get; set; }
        public int Points { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
    }
}
