namespace AIDMusicApp.Models
{
    public class TrackList
    {
        public int Id { get; set; }

        public int SongId { get; set; }

        public int AlbumId { get; set; }

        public TrackList Copy()
        {
            return new TrackList
            {
                Id = Id,
                SongId = SongId,
                AlbumId = AlbumId
            };
        }
    }
}
