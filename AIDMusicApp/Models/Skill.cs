namespace AIDMusicApp.Models
{
    public class Skill
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Skill Copy()
        {
            return new Skill
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
