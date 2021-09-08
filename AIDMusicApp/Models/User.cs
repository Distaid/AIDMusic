namespace AIDMusicApp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int AccessId { get; set; }

        public byte[] Avatar { get; set; }
    }
}
