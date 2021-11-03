namespace AIDMusicApp.Models
{
    public class AlbumGenres
    {
        public int Id { get; set; }

        public int AlbumId { get; set; }

        public int GenreId { get; set; }

        public AlbumGenres Copy()
        {
            return new AlbumGenres
            {
                Id = Id,
                AlbumId = AlbumId,
                GenreId = GenreId
            };
        }
    }
}
