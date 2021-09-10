namespace AIDMusicApp.Models
{
    public class Label
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Label Copy()
        {
            return new Label
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
