namespace AIDMusicApp.Models
{
    public class FormerMembers
    {
        public int Id { get; set; }

        public int MusicianId { get; set; }

        public int GroupId { get; set; }

        public FormerMembers Copy()
        {
            return new FormerMembers
            {
                Id = Id,
                MusicianId = MusicianId,
                GroupId = GroupId
            };
        }
    }
}
