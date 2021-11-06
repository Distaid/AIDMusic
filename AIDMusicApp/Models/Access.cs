namespace AIDMusicApp.Models
{
    public class Access
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
