namespace AIDMusicApp.Models
{
    public class AlbumFormat
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AlbumFormat Copy()
        {
            return new AlbumFormat
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
