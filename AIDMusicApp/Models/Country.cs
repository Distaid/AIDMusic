namespace AIDMusicApp.Models
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Country Copy()
        {
            return new Country
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
