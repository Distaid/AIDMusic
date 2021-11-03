namespace AIDMusicApp.Models
{
    public class SongGenres
    {
        public int Id { get; set; }

        public int SongId { get; set; }

        public int GenreId { get; set; }

        public SongGenres Copy()
        {
            return new SongGenres
            {
                Id = Id,
                SongId = SongId,
                GenreId = GenreId
            };
        }
    }
}
