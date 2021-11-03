namespace AIDMusicApp.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public short Year { get; set; }

        public byte[] Cover { get; set; }

        public Album Copy()
        {
            var album = new Album
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Year = Year
            };

            if (Cover != null)
            {
                album.Cover = new byte[Cover.Length];
                Cover.CopyTo(album.Cover, 0);
            }

            return album;
        }
    }
}
