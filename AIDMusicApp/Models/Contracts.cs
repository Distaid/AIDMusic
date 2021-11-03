namespace AIDMusicApp.Models
{
    public class Contracts
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int LabelId { get; set; }

        public Contracts Copy()
        {
            return new Contracts
            {
                Id = Id,
                GroupId = GroupId,
                LabelId = LabelId
            };
        }
    }
}
