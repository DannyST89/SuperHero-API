namespace SuperHero_API.Models.DTOs.SuperHero
{
    public class SuperHeroGet
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public string FirstName { get; set; } = String.Empty;

        public string LastName { get; set; } = String.Empty;

        public string Place { get; set; } = String.Empty;
    }
}
