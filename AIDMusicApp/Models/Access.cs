namespace AIDMusicApp.Models
{
    public class Access
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Access Copy()
        {
            return new Access
            {
                Id = Id,
                Name = Name
            };
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
