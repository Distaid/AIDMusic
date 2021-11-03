namespace AIDMusicApp.Models
{
    public class Cover
    {
        public int Id { get; set; }

        public int SongId { get; set; }

        public int OriginGroupId { get; set; }

        public int OriginSongId { get; set; }

        public Cover Copy()
        {
            return new Cover
            {
                Id = Id,
                SongId = SongId,
                OriginGroupId = OriginGroupId,
                OriginSongId = OriginSongId
            };
        }
    }
}
