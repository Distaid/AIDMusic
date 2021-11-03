namespace AIDMusicApp.Models
{
    public class Feat
    {
        public int Id { get; set; }

        public int SongId { get; set; }

        public int MusicianId { get; set; }

        public int MusicianGroupId { get; set; }

        public Feat Copy()
        {
            return new Feat
            {
                Id = Id,
                SongId = SongId,
                MusicianId = MusicianId,
                MusicianGroupId = MusicianGroupId
            };
        }
    }
}
