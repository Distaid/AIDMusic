namespace AIDMusicApp.Models
{
    public class Discography
    {
        public int Id { get; set; }

        public int AlbumId { get; set; }

        public int GroupId { get; set; }

        public Discography Copy()
        {
            return new Discography
            {
                Id = Id,
                AlbumId = AlbumId,
                GroupId = GroupId
            };
        }
    }
}
