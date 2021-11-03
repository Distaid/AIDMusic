namespace AIDMusicApp.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public short YearOfFoundation { get; set; }

        public short YearOfBreakup { get; set; }

        public Country CountryId { get; set; }

        public Group Copy()
        {
            return new Group
            {
                Id = Id,
                Name = Name,
                Description = Description,
                YearOfFoundation = YearOfFoundation,
                YearOfBreakup = YearOfBreakup,
                CountryId = CountryId.Copy()
            };
        }
    }
}
