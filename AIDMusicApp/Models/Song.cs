namespace AIDMusicApp.Models
{
    public class Song
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public short Time { get; set; }

        public bool IsCover { get; set; }

        public Song Copy()
        {
            return new Song
            {
                Id = Id,
                Name = Name,
                Time = Time,
                IsCover = IsCover
            };
        }
    }
}
