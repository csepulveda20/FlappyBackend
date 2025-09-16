
namespace Domain.Entities
{
    public class Alias
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;

        private Alias() { }

        public Alias(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public static Alias Create(string name)
        {
            return new Alias(name);
        }   
    }
}