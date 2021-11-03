namespace AIDMusicApp.Models
{
    public class GroupGenres
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int GenreId { get; set; }

        public GroupGenres Copy()
        {
            return new GroupGenres
            {
                Id = Id,
                GroupId = GroupId,
                GenreId = GenreId
            };
        }
    }
}
