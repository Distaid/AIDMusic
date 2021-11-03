namespace AIDMusicApp.Models
{
    public class CurrentMembers
    {
        public int Id { get; set; }

        public int MusicianId { get; set; }

        public int GroupId { get; set; }

        public CurrentMembers Copy()
        {
            return new CurrentMembers
            {
                Id = Id,
                MusicianId = MusicianId,
                GroupId = GroupId
            };
        }
    }
}
